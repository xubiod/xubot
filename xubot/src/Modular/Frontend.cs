﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using xubot.src.Attributes;
using XubotSharedModule;

namespace xubot.src.Modular
{
    public class Frontend
    {
        [Group("module"), Alias(";"), Summary("Commands relating to running external commands with the modular command system.")]

        public class Running : ModuleBase
        {
            [ExampleAttribute("examplemodule examplecmd exampleparams...")]
            [Command("run", RunMode = RunMode.Async), Alias("~", ""), Summary("Runs a command from a module.")]
            public async Task Execute(string module, string command, params string[] parameters)
            {
                await ModularSystem.Execute(Context, module, command, parameters);
            }
        }

        [Group("module-util"), Alias(";;"), Summary("Commands relating to modules themselves.")]
        public class Utilities : ModuleBase
        {
            [ExampleAttribute("examplemodule")]
            [Command("reload", RunMode = RunMode.Async), Alias("r"), Summary("Reloads a module."), RequireOwner]
            public async Task Reload(string module)
            {
                await ReplyAsync(ModularSystem.modules[module].Reload());
            }

            [ExampleAttribute("examplemodule")]
            [Command("unload", RunMode = RunMode.Async), Alias("u"), Summary("Unloads a module."), RequireOwner]
            public async Task Unload(string module)
            {
                await ReplyAsync(ModularSystem.modules[module].Unload());
            }

            [ExampleAttribute("examplemodule")]
            [Command("list", RunMode = RunMode.Async), Alias("l"), Summary("Lists commands in a module.")]
            public async Task List(string module)
            {
                string list = "";

                foreach (ICommandModule cmd in ModularSystem.modules[module].commandInstances)
                    foreach (MethodInfo item in cmd.GetType().GetMethods().Where(x => (x.GetCustomAttribute<CmdNameAttribute>() ?? new CmdNameAttribute("")).Name != ""))
                        list += item.GetCustomAttribute<CmdNameAttribute>().Name + " - " + (item.GetCustomAttribute<CmdSummaryAttribute>() != null ? item.GetCustomAttribute<CmdSummaryAttribute>().Summary : "Not set in module") + "\n";
                //list += cmd.GetType().getM + " - " + "NotImplement" + "\n";

                EmbedBuilder embed = GetTemplate("Module Command Listing", $"For module **\"{module.ToLower()}\"**", list);

                await ReplyAsync("", false, embed.Build());
            }

            [Command("listall", RunMode = RunMode.Async), Alias("la"), Summary("Lists all modules.")]
            public async Task ListAll()
            {
                string list = "";

                foreach (KeyValuePair<string, ModularSystem.ModuleEntry> mod in ModularSystem.modules)
                    list += $"{mod.Key} - { (mod.Value.commandInstances.Count > 0 ? mod.Value.commandInstances.Count + " cmds" : "Not loaded/no cmds")}\n";

                EmbedBuilder embed = GetTemplate("Module Listing", "Note: *some of these might not be loaded*", list);

                await ReplyAsync("", false, embed.Build());
            }

            private EmbedBuilder GetTemplate(string title, string description, string listing)
            {
                return new EmbedBuilder()
                {
                    Title = title,
                    Description = description,
                    Color = Discord.Color.Orange,
                    ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder()
                    {
                        Text = Util.Globals.EmbedFooter,
                        IconUrl = Context.Client.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.Now,
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Listing",
                            Value = $"```{listing}```"
                        }
                    }
                };
            }
        }
    }
}
