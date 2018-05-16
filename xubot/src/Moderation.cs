using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace xubot.src
{
    [Group("moderation"), Alias("mod")]
    public class Moderation : ModuleBase
    {
        [Command("audit-log-relay")]
        public async Task alr(ulong cid)
        {
            ITextChannel _ch = (await Context.Guild.GetChannelAsync(cid) as ITextChannel);
            //complete
        }
    }

    public class ALR
    {
        public void Starter()
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

        public async Task _channel_c(SocketChannel arg)
        {
            throw new NotImplementedException();
        }

        public async Task _channel_d(SocketChannel arg)
        {
            throw new NotImplementedException();
        }

        public async Task _channel_u(SocketChannel arg, SocketChannel arg2)
        {
            throw new NotImplementedException();
        }
    }
}
