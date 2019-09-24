using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot
{
    public class React : ModuleBase
    {
        [Command("$"), RequireOwner]
        public async Task react(string echo)
        {
            await Context.Message.DeleteAsync(new Discord.RequestOptions(){});
            await ReplyAsync(echo);
        }
    }
}
