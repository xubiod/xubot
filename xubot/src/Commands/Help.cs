﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using xubot.Attributes;

namespace xubot.Commands
{
    [Group("help"), Alias("?"), Summary("The savior for the lost.")]
    public class Help : ModuleBase
    {
        [Example("1")]
        [Command("get", RunMode = RunMode.Async), Alias("")]
        public async Task _Help(int page = 1)
        {
            await HelpCmd(page);
        }

        [Example("help list")]
        [Command("get", RunMode = RunMode.Async), Alias(""), Summary("Lists data for one command.")]
        public async Task HelpCmd(params string[] lookupAsAll)
        {
            string all = "";
            var count = lookupAsAll.Length;

            if (Int32.TryParse(lookupAsAll[count - 1], out var page))
                count--;
            else
                page = 1;

            for (int i = 0; i < count; i++)
                all += lookupAsAll[i] + (i == count - 1 ? "" : " ");

            if (lookupAsAll[0].ToLower() == "search")
            {
                await Search(all.Replace("search ", ""), true, page);
                return;
            }

            await HelpHandling(all, page, true);
        }

        /*[Command, Summary("Lists data for one command.")]
        public async Task help(string lookup, bool wGroup = false, int index = 1)
        {
            await helpHandling(lookup, index, wGroup);
        }*/

        [Example("list 1")]
        [Command("list", RunMode = RunMode.Async), Summary("Lists all commands.")]
        public async Task HelpCmd(int page = 1)
        {
            if (page < 1) page = 1;
            int itemsPerPage = src.BotSettings.Global.Default.EmbedListMaxLength;

            List<CommandInfo> commList = Program.XuCommand.Commands.ToList();

            string items = "";

            int limit = System.Math.Min(commList.Count - (page - 1) * itemsPerPage, itemsPerPage);
            //await ReplyAsync((limit).ToString());

            int index;

            for (int i = 0; i < limit; i++)
            {
                index = i + itemsPerPage * (page - 1);

                if (index > commList.Count - 1) { break; }

                string parentForm = GetAllGroups(commList[index].Module);

                items += parentForm + commList[index].Name + "\n";
            }

            if (string.IsNullOrWhiteSpace(items)) items = "There's nothing here, I think you went out of bounds.";

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Help", $"Showing page #{page} out of {System.Math.Ceiling((float)commList.Count / itemsPerPage)} pages.\nShowing a few of the **{commList.Count}** cmds.", Color.Magenta);
            embed.Fields = new List<EmbedFieldBuilder>
            {
                new()
                {
                    Name = "List",
                    Value = $"```\n{items}```" ,
                    IsInline = true
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Example("e true 1")]
        [Command("search", RunMode = RunMode.Async), Summary("Searches all commands with a search term. Deep enables searching the aliases as well, but this takes longer.")]
        public async Task Search(string lookup, bool deep = true, int page = 1)
        {
            using Util.WorkingBlock wb = new Util.WorkingBlock(Context);
            if (page < 1) page = 1;
            int itemsPerPage = src.BotSettings.Global.Default.EmbedListMaxLength;

            List<CommandInfo> commList = Program.XuCommand.Commands.ToList();
            List<CommandInfo> compatibles = new List<CommandInfo>();

            bool add;
            foreach (CommandInfo cmd in commList)
            {
                add = false;

                add |= cmd.Name.Contains(lookup);

                if (cmd.Aliases != null && deep)
                {
                    foreach (string alias in cmd.Aliases)
                        add |= alias.Contains(lookup);
                }

                if (add) compatibles.Add(cmd);
            }

            int limit = System.Math.Min(commList.Count - (page - 1) * itemsPerPage, itemsPerPage);
            int index;

            string cmds = "";
            for (int i = 0; i < limit; i++)
            {
                index = i + itemsPerPage * (page - 1);

                if (index > compatibles.Count - 1) { break; }

                cmds += GetAllGroups(compatibles[index].Module) + compatibles[index].Name + "\n";
            }
            if (string.IsNullOrWhiteSpace(cmds)) cmds = "I don't think any command called that exists...";

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Help - Search", $"Showing page #{page} out of {System.Math.Ceiling((float)compatibles.Count / itemsPerPage)} pages.\nShowing a few of the **{compatibles.Count}** cmds with the lookup.", Color.Magenta);
            embed.Fields = new List<EmbedFieldBuilder>
            {
                new()
                {
                    Name = "Search Results",
                    Value = $"```\n{cmds}```" ,
                    IsInline = true
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        public async Task HelpHandling(string lookup, int index = 1, bool exact = false)
        {
            try
            {
                List<CommandInfo> commListE = Program.XuCommand.Commands.ToList();
                // List<ModuleInfo> moduleListE = Program.XuCommand.Modules.ToList();

                commListE = commListE.FindAll(ci => ci.Name == lookup.Split(' ').Last());

                List<CommandInfo> commList = commListE;

                if (exact)
                {
                    commList = new List<CommandInfo>();
                    foreach (var item in commListE)
                    {
                        foreach (var alias in item.Aliases)
                        {
                            if (alias == lookup)
                            {
                                commList.Add(item);
                                //item.Attributes.Contains(new DeprecatedAttribute());
                            }
                        }
                    }
                }

                int allMatches = commList.Count;

                CommandInfo comm;

                try
                {
                    comm = commList[index - 1];
                }
                catch
                {
                    //not a command
                    await GroupHandling(lookup);
                    return;
                }

                string allAlias = "";
                IReadOnlyList<string> aliases = comm.Aliases ?? new List<string>();

                string trueName = comm.Name;

                if (aliases.ToList().Find(al => al == lookup) == lookup)
                {
                    trueName = lookup;
                }

                if (aliases.Count != 0)
                {
                    foreach (string alias in comm.Aliases)
                    {
                        allAlias += alias + "\n";
                    }
                }

                string allPara = "No parameters.";
                string examplePara = "";
                IReadOnlyList<ParameterInfo> @params = comm.Parameters.ToList();

                if (@params.Count != 0)
                {
                    allPara = "";
                    foreach (var para in comm.Parameters)
                    {
                        allPara += (para.IsMultiple ? "params " : "") + Util.String.SimplifyTypes(para.Type.ToString()) + " " + para.Name + (para.IsOptional ? " (optional) // default value = " + para.DefaultValue : "") +"\n";
                        examplePara += para.Name + " ";
                    }
                }

                string trueSummary = "No summary given.";
                if (comm.Summary != null) trueSummary = comm.Summary;

                bool dep = comm.Attributes.Contains(new DeprecatedAttribute()) || comm.Module.Attributes.Contains(new DeprecatedAttribute());

                string nsfwPossibility = comm.Attributes.Any(x => x is NsfwPossibilityAttribute) ? (comm.Attributes.First(x => x is NsfwPossibilityAttribute) as NsfwPossibilityAttribute).Warnings : "";
                nsfwPossibility += comm.Module.Attributes.Any(x => x is NsfwPossibilityAttribute) ? "Group-wide:\n\n" + (comm.Module.Attributes.First(x => x is NsfwPossibilityAttribute) as NsfwPossibilityAttribute).Warnings : "";

                if (comm.Attributes.Any(x => x is ExampleAttribute))
                {
                    ExampleAttribute ex = comm.Attributes.First(x => x is ExampleAttribute) as ExampleAttribute;
                    if (!string.IsNullOrWhiteSpace(ex.ExampleParameters)) examplePara = ex.ExampleParameters;
                    examplePara += ex.AttachmentNeeded ? "\n\n[You need to upload a file to use this.]" : "";
                }

                string exampleUsage = $"{Program.Prefix}{trueName} " + examplePara;

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Help", $"The newer *better* help. Showing result #{index} out of {allMatches} match(s).", Color.Magenta);
                embed.Fields = new List<EmbedFieldBuilder>
                {
                    new()
                    {
                        Name = "Command Name",
                        Value = $"`{trueName}`" ,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Summary",
                        Value = trueSummary,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Known Aliases",
                        Value = $"```\n{allAlias}```",
                        IsInline = false
                    },
                    new()
                    {
                        Name = "Parameters",
                        Value = $"```cs\n{allPara}```",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Example Usage",
                        Value = $"`{exampleUsage}`",
                        IsInline = false
                    }
                };

                if (dep) embed.Fields.Add(new EmbedFieldBuilder
                {
                    Name = "Deprecated",
                    Value = "__**All commands in this group are deprecated. They are going to be removed in a future update.**__",
                    IsInline = true
                });

                if (!string.IsNullOrEmpty(nsfwPossibility)) embed.Fields.Add(new EmbedFieldBuilder
                {
                    Name = "NSFW Possibility",
                    Value = $"This can show NSFW content. NSFW content is restricted to NSFW channels.\n**{nsfwPossibility}**",
                    IsInline = true
                });

                await ReplyAsync("", false, embed.Build());
            }
            catch (Exception e)
            {
                await Util.Error.BuildErrorAsync(e, Context);
            }
        }

        public async Task GroupHandling(string lookup)
        {
            try
            {
                List<ModuleInfo> moduList = Program.XuCommand.Modules.ToList().FindAll(ci => ci.Group == lookup.Split(' ').Last());

                // int allMatchs = moduList.Count;

                ModuleInfo group;

                try
                {
                    group = moduList[0];
                }
                catch
                {
                    //not a command OR group
                    await ReplyAsync("This command does not exist. (Trust me, I looked everywhere.)\nMaybe try the full name?");
                    return;
                }

                string allAlias = "";
                IReadOnlyList<string> aliases = group.Aliases ?? new List<string>();

                string trueName = group.Name;

                if (aliases.ToList().Find(al => al == lookup) == lookup)
                {
                    trueName = lookup;
                }

                if (aliases.Count != 0)
                {
                    foreach (string alias in group.Aliases)
                    {
                        allAlias += alias + "\n";
                    }
                }

                string commands = "None";
                string str;
                if (group.Commands.Count != 0)
                {
                    commands = "";
                    foreach (var cmd in group.Commands)
                    {
                        str = /* group.Name + " " + */ cmd.Name + "\n";
                        if (!commands.Contains(str)) commands += str;
                    }
                }

                string subgroup = "None";
                if (group.Submodules.Count != 0)
                {
                    subgroup = "";
                    foreach (var sub in group.Submodules)
                    {
                        subgroup += group.Name + " " + sub.Name + "\n";
                    }
                }

                string trueSummary = "No summary given.";
                if (group.Summary != null) trueSummary = group.Summary;

                bool dep = group.Attributes.Contains(new DeprecatedAttribute());
                string nsfwPossibility = group.Attributes.Contains(new NsfwPossibilityAttribute()) ? (group.Attributes.First(x => x is NsfwPossibilityAttribute) as NsfwPossibilityAttribute ?? new NsfwPossibilityAttribute()).Warnings : null;

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Help", "The newer *better* help. For more specifics, combine the group and command.", Color.Magenta);
                embed.Fields = new List<EmbedFieldBuilder>
                {
                    new()
                    {
                        Name = "Module Name and Summary",
                        Value = $"`{trueName}`\n*{trueSummary}*",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Known Aliases",
                        Value = $"```\n{allAlias}```",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Subgroups in Group",
                        Value = $"```\n{subgroup}```",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Commands in Group",
                        Value = $"```\n{commands}```",
                        IsInline = false
                    }
                };

                if (dep) embed.Fields.Add(new EmbedFieldBuilder
                {
                    Name = "Deprecated",
                    Value = "__**All commands in this group are deprecated. They are going to be removed in a future update.**__",
                    IsInline = true
                });

                if (nsfwPossibility != null) embed.Fields.Add(new EmbedFieldBuilder
                {
                    Name = "NSFW Possibility",
                    Value = $"This can show NSFW content. NSFW content is restricted to NSFW channels.\n**{nsfwPossibility}**",
                    IsInline = true
                 });

                await ReplyAsync("", false, embed.Build());
            }
            catch (Exception e)
            {
                await Util.Error.BuildErrorAsync(e, Context);
            }
        }

        private static string GetAllGroups(ModuleInfo module)
        {
            if (module == null) return "";
            if (module.Group == null) return "";

            string output = "";
            ModuleInfo check = module;

            do
            {
                output = check.Group + (check.Group != null ? " " : "") + output;
                check = check.Parent;
            } while (check != null);

            return output;
        }
    }
}
