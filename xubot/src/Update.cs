using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    public class Update : ModuleBase
    {
        [Command("update")]
        public async Task update()
        {
            string launcher_path = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName + "\\xubot-launcher.exe";

            if (File.Exists(launcher_path))
            {
                Process.Start(launcher_path);
                Environment.Exit(0);
            } else
            {
                await ReplyAsync("The updater executable isn't where it should be. :(");
                await ReplyAsync(launcher_path);
            }
        }
    }
}
