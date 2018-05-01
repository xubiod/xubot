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
            IChannel _ch = await Context.Guild.GetChannelAsync(cid);
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
