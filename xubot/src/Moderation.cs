using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Xml.Linq;
using System.Xml.XPath;
using xubot;

namespace xubot.src
{
    [Group("moderation"), Alias("mod")]
    public class Moderation : ModuleBase
    {
        //[Command("audit-log-relay")]
        public async Task alr(ulong cid)
        {
            ITextChannel _ch = (await Context.Guild.GetChannelAsync(cid) as ITextChannel);
            //complete
            await ALR.Edit(Context, Context.Guild.Id, _ch.Id);
            await ReplyAsync("Audit log relay made/changed.");
        }
    }

    public class ALR
    {
        public static async Task Edit(ICommandContext Context, ulong g, ulong alr)
        {
            var xdoc = XDocument.Load("Moderation.xml");

            var items = from i in xdoc.Descendants("mod")
                        select new
                        {
                            guild = i.Attribute("guild"),
                            alr = i.Attribute("alr-chan")
                        };

            foreach (var item in items)
            {
                if ((ulong)item.guild == g)
                {
                    try
                    {
                        XElement element = new XElement("mod");

                        XAttribute _att1 = new XAttribute("guild", g);
                        XAttribute _att2 = new XAttribute("alr-chan", alr);

                        element.Add(_att1);
                        element.Add(_att2);

                        xdoc.Root.Add(element);
                        xdoc.Save("Moderation.xml");

                        await Task.CompletedTask;
                    }
                    catch (Exception exp)
                    {
                        await GeneralTools.CommHandler.BuildError(exp, Context);
                    }

                    await Task.CompletedTask;
                }
            }
        }
        public static ITextChannel Read(ulong g)
        {
            var xdoc = XDocument.Load("Moderation.xml");

            var items = from i in xdoc.Descendants("mod")
                        select new
                        {
                            guild = i.Attribute("guild"),
                            alr = i.Attribute("alr-chan")
                        };

            foreach (var item in items)
            {
                if ((ulong)item.guild == g)
                {
                    return Program.xuClient.GetGuild(g).GetChannel((ulong)item.alr) as ITextChannel;
                }
            }

            return null;
        }


        public static void Starter()
        {
            Program.xuClient.ChannelCreated += _channel_c;
            Program.xuClient.ChannelDestroyed += _channel_d;
            Program.xuClient.ChannelUpdated += _channel_u;

            //Program.xuClient.GuildUpdated += _guild_u;

            //Program.xuClient.Log += _log;

            //Program.xuClient.MessageDeleted += _msg_d;

            //Program.xuClient.RoleCreated += _role_c;
            //Program.xuClient.RoleDeleted += _role_d;
            //Program.xuClient.RoleUpdated += _role_u;

            //Program.xuClient.UserBanned += _user_b;
            //Program.xuClient.UserJoined += _user_j;
            //Program.xuClient.UserLeft += _user_l;
            //Program.xuClient.UserUnbanned += _user_ub;
            //Program.xuClient.UserUpdated += _user_u;
        }

        public static async Task _channel_c(SocketChannel arg)
        {
            await Read((arg as IGuildChannel).Id).SendMessageAsync("Channel created: **" + arg.Id);
        }

        public static async Task _channel_d(SocketChannel arg)
        {
            throw new NotImplementedException();
        }

        public static async Task _channel_u(SocketChannel arg, SocketChannel arg2)
        {
            throw new NotImplementedException();
        }
    }
}
