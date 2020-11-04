using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Modular
{
    public class Frontend
    {
        [Group("module"), Alias(";"), Summary("Commands relating to running external commands with the modular command system.")]

        public class Running : ModuleBase
        {
            [Command("run"), Alias("~", ""), Summary("Runs a command from a module.")]
            public async Task Execute(string module, string command, params string[] parameters)
            {
                await ModularUtil.SendMessage(Context, Modular.ModularSystem.modules[module].Execute(command, parameters));
            }
        }

        [Group("module-util"), Alias(";;"), Summary("Commands relating to modules themselves.")]
        public class Utilities : ModuleBase
        {

            [Command("reload"), Alias("r"), Summary("Reloads a module.")]
            public async Task Reload(string module)
            {
                await ReplyAsync(ModularSystem.modules[module].Reload());
            }

            [Command("unload"), Alias("u"), Summary("Unloads a module.")]
            public async Task Unload(string module)
            {
                await ReplyAsync(ModularSystem.modules[module].Unload());
            }

            [Command("list"), Alias("l"), Summary("Lists commands in a module.")]
            public async Task List(string module)
            {
                throw new NotImplementedException();
            }
        }
    }
}
