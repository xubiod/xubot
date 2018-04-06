using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xubot.src
{
    [Group("pronoun")]
    public class Pronouns : ModuleBase
    {
        public static XDocument xdoc = new XDocument();

        [Command("get")]
        public async Task get(ulong id)
        {
            try
            {
                IUser usr = Program.xuClient.GetUser(id);
                PronounTools.AddRefresh(usr);
                await PronounTools.Build(Context, usr, PronounTools.Read(usr));
            }
            catch (Exception exp)
            {
                await ReplyAsync("They haven't added it yet.");
            }
        }

        [Command("get")]
        public async Task get(string user, string discrim)
        {
            try
            {
                IUser usr = Program.xuClient.GetUser(user, discrim);
                PronounTools.AddRefresh(usr);
                await PronounTools.Build(Context, usr, PronounTools.Read(usr));
            }
            catch (Exception exp)
            {
                await ReplyAsync("They haven't added it yet.");
            }
        }

        [Command("set"), RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task set(string pro)
        {
            PronounTools.AddRefresh(Context.Message.Author);
            PronounTools.Set(Context.Message.Author, pro);

            string role_name = "prefers " + pro;
            IRole role;

            if (Context.Guild.Roles.Any(x => x.Name == role_name))
            {
                role = Context.Guild.Roles.First(x => x.Name == role_name);
            } else
            {
                role = await Context.Guild.CreateRoleAsync(role_name);
            }

            await (Context.Message.Author as IGuildUser).AddRoleAsync(role);
            await ReplyAsync("Pronoun set, and role made or set.");
        }

        [Command("set-no-prefix")]
        public async Task setnp(string pro)
        {
            PronounTools.AddRefresh(Context.Message.Author);
            PronounTools.Set(Context.Message.Author, pro);

            string role_name = "prefers " + pro;
            //IRole role;

            /*if (Context.Guild.Roles.Any(x => x.Name == role_name))
            {
                role = Context.Guild.Roles.First(x => x.Name == role_name);
            }
            else
            {
                role = await Context.Guild.CreateRoleAsync(role_name);
            }*/

            //await (Context.Message.Author as IGuildUser).AddRoleAsync(role);
            await ReplyAsync("Pronoun set.");
        }

    }

    public class PronounTools
    {
        public static void AddRefresh(IUser arg)
        {
            bool exists = false;

            Pronouns.xdoc = XDocument.Load("Pronouns.xml");

            var items = from i in Pronouns.xdoc.Descendants("pronoun")
                        select new
                        {
                            user = i.Attribute("id"),
                            preferred = i.Attribute("preferred")
                        };

            foreach (var item in items)
            {
                if (item.user.Value == arg.Id.ToString())
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                Console.WriteLine("new user found to add to pronouns, doing that now");

                XElement xelm = new XElement("pronoun");
                XAttribute user = new XAttribute("id", arg.Id.ToString());
                XAttribute prefer = new XAttribute("preferred", "not set!");

                xelm.Add(user);
                xelm.Add(prefer);

                Pronouns.xdoc.Root.Add(xelm);
                Pronouns.xdoc.Save("Pronouns.xml");
            }
        }

        public static string Read(IUser arg)
        {
            Pronouns.xdoc = XDocument.Load("Pronouns.xml");

            var items = from i in Pronouns.xdoc.Descendants("pronoun")
                        select new
                        {
                            user = i.Attribute("id"),
                            preferred = i.Attribute("preferred")
                        };

            foreach (var item in items)
            {
                if (item.user.Value == arg.Id.ToString())
                {
                    return item.preferred.Value;
                }
            }

            return "not set!";
        }

        public static void Set(IUser arg, string newVal)
        {
            Pronouns.xdoc = XDocument.Load("Pronouns.xml");

            var items = from i in Pronouns.xdoc.Descendants("pronoun")
                        select new
                        {
                            user = i.Attribute("id"),
                            preferred = i.Attribute("preferred")
                        };

            foreach (var item in items)
            {
                if (item.user.Value == arg.Id.ToString())
                {
                    item.preferred.Value = newVal;
                }
            }

            Pronouns.xdoc.Save("Pronouns.xml");
        }

        public static async Task Build(ICommandContext Context, IUser arg, string prefer)
        {
            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Preferred Pronoun",
                Color = Discord.Color.Red,
                Description = arg.Username + "#" + arg.Discriminator + " 's Preferred Pronoun",

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Please use:",
                                Value = "**" + prefer + "**",
                                IsInline = false
                            }
                        }
            };

            await Context.Channel.SendMessageAsync("", false, embedd);
        }
    }
}
