using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using xubot.Attributes;
using xubot.src.Attributes;
using XubotSharedModule;

namespace xubot.Modular;

public class Frontend
{
    [Group("module"), Alias(";"), Summary("Commands relating to running external commands with the modular command system.")]

    public class Running : ModuleBase
    {
        [Example("examplemodule examplecmd exampleparams...")]
        [Command("run", RunMode = RunMode.Async), Alias("~", ""), Summary("Runs a command from a module.")]
        public void Execute(string module, string command, params string[] parameters)
        {
            ModularSystem.Execute(Context, module, command, parameters);
        }
    }

    [Group("module-util"), Alias(";;"), Summary("Commands relating to modules themselves.")]
    public class Utilities : ModuleBase
    {
        [Example("examplemodule")]
        [Command("reload", RunMode = RunMode.Async), Alias("r"), Summary("Reloads a module."), RequireOwner]
        public async Task Reload(string module)
        {
            await ReplyAsync(await ModularSystem.Modules[module].Reload());
        }

        [Example("examplemodule")]
        [Command("unload", RunMode = RunMode.Async), Alias("u"), Summary("Unloads a module."), RequireOwner]
        public async Task Unload(string module)
        {
            await ReplyAsync(await ModularSystem.Modules[module].Unload());
        }

        [Example("examplemodule")]
        [Command("list", RunMode = RunMode.Async), Alias("l"), Summary("Lists commands in a module.")]
        public async Task List(string module)
        {
            var list = "";

            foreach (var cmd in ModularSystem.Modules[module].commandInstances)
            foreach (var item in cmd.GetType().GetMethods().Where(x => string.IsNullOrEmpty((x.GetCustomAttribute<CmdNameAttribute>() ?? new CmdNameAttribute("")).Name)))
                list += item.GetCustomAttribute<CmdNameAttribute>().Name + " - " + (item.GetCustomAttribute<CmdSummaryAttribute>() != null ? item.GetCustomAttribute<CmdSummaryAttribute>().Summary : "Not set in module") + "\n";
            //list += cmd.GetType().getM + " - " + "NotImplement" + "\n";

            var embed = GetTemplate("Module Command Listing", $"For module **\"{module.ToLower()}\"**", list);

            await ReplyAsync("", false, embed.Build());
        }

        [Command("list-all", RunMode = RunMode.Async), Alias("la"), Summary("Lists all modules.")]
        public async Task ListAll()
        {
            var list = "";

            foreach (var mod in ModularSystem.Modules)
                list += $"{mod.Key} - { (mod.Value.commandInstances.Count > 0 ? mod.Value.commandInstances.Count + " cmds" : "Not loaded/no cmds")}\n";

            var embed = GetTemplate("Module Listing", "Note: *some of these might not be loaded*", list);

            await ReplyAsync("", false, embed.Build());
        }

        private EmbedBuilder GetTemplate(string title, string description, string listing)
        {
            return new EmbedBuilder
            {
                Title = title,
                Description = description,
                Color = Color.Orange,
                ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),

                Footer = new EmbedFooterBuilder
                {
                    Text = Util.Globals.EmbedFooter,
                    IconUrl = Context.Client.CurrentUser.GetAvatarUrl()
                },
                Timestamp = DateTime.Now,
                Fields =
                [
                    new()
                    {
                        Name = "Listing",
                        Value = $"```{listing}```"
                    }
                ]
            };
        }
    }
}