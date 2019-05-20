using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace xubot_core.src
{
    public class Update : ModuleBase
    {
        [Command("update"), RequireOwner]
        public async Task UpdateBot()
        {
            // to be redone
        }
    }
}
