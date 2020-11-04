using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule;

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
                string list = "";

                foreach (CommandModule cmd in ModularSystem.modules[module].commandInstances)
                    list += cmd.GetName() + " - " + cmd.GetSummary() + "\n";

                EmbedBuilder embedd = new EmbedBuilder()
                {
                    Title = "Module Command Listing",
                    Description = "For module **\"" + module.ToLower() + "\"**",
                    Color = Discord.Color.LightOrange,
                    ThumbnailUrl = Program.xuClient.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.Now,
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Listing",
                            Value = "```" + list + "```"
                        }
                    }
                };

                await ReplyAsync("", false, embedd.Build());
            }

            [Command("listall"), Alias("la"), Summary("Lists all modules.")]
            public async Task ListAll()
            {
                string list = "";

                foreach (KeyValuePair<string, ModularSystem.ModuleEntry> mod in ModularSystem.modules)
                    list += mod.Key + " - " + (mod.Value.commandInstances.Count > 0 ? mod.Value.commandInstances.Count + " cmds" : "Not loaded/no cmds") + "\n";

                EmbedBuilder embedd = new EmbedBuilder()
                {
                    Title = "Module Listing",
                    Description = "Note: *some of these might not be loaded*",
                    Color = Discord.Color.LightOrange,
                    ThumbnailUrl = Program.xuClient.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.Now,
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Listing",
                            Value = "```" + list + "```"
                        }
                    }
                };

                await ReplyAsync("", false, embedd.Build());
            }
        }
    }
}
