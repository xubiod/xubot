﻿using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Commands
{
    public class React : ModuleBase
    {
        [Command("$"), RequireOwner]
        public async Task Speak(string echo)
        {
            await Context.Message.DeleteAsync(new Discord.RequestOptions() { });
            await ReplyAsync(echo);
        }
    }
}
