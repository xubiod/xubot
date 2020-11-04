using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Modular
{
    [Group("module"), Alias(";"), Summary("Commands relating to the modular command system.")]
    public class Frontend : ModuleBase
    {
        [Command("")]
        public async Task Execute(string module, string command, params string[] parameters)
        {
            await ModularUtil.SendMessage(Context, Modular.ModularSystem.modules[module].Execute(command, parameters));
        }
    }
}
