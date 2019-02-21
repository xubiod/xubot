using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot_core.src
{
    [Group("help")]
    public class Help : ModuleBase
    {
        public int itemsPerPage = 15;

        [Command]
        public async Task _help(int page = 1)
        {
            await help(page);
        }

        [Command, Summary("Lists data for one command.")]
        public async Task help(string lookup, int index = 1)
        {
            await helpHandling(lookup, index, false);
        }

        [Command, Summary("Lists data for one command.")]
        public async Task help(string lookup, bool wGroup = false, int index = 1)
        {
            await helpHandling(lookup, index, wGroup);
        }

        [Command("list"), Summary("Lists all commands.")]
        public async Task help(int page = 1)
        {
            List<CommandInfo> commList = Program.xuCommand.Commands.ToList();

            string items = "";

            int limit = System.Math.Min(commList.Count - ((page - 1) * itemsPerPage), itemsPerPage);
            //await ReplyAsync((limit).ToString());

            for (int i = 0; i < limit; i++)
            {
                int index = i + (page - 1) * itemsPerPage;

                if (index > commList.Count) { break; }

                string parentForm = "No parent";
                if (commList[index].Module.Parent != null)
                {
                    parentForm = commList[index].Module.Parent.Name;
                }

                items += "(" + parentForm + ") " + commList[index].Name + "\n";
            }

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Help",
                Color = Discord.Color.Magenta,
                Description = "Showing page #" + (page).ToString() + " out of " + ((commList.Count / itemsPerPage) + 1).ToString() + " pages.\nShowing a few of the **" + commList.Count.ToString() + "** cmds.",
                ThumbnailUrl = Program.xuClient.CurrentUser.GetAvatarUrl(),

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p",
                    IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "List",
                                Value = "```" + items + "```" ,
                                IsInline = true
                            }
                        }
            };
            await ReplyAsync("", false, embedd.Build());
        }
        
        public async Task helpHandling(string lookup, int index = 1, bool exact = false)
        {
            try
            {
                List<CommandInfo> commList_e = Program.xuCommand.Commands.ToList();

                commList_e = commList_e.FindAll(ci => ci.Name == (lookup.Split(' ').Last()));

                List<CommandInfo> commList = commList_e;

                if (exact)
                {
                    commList = new List<CommandInfo>();
                    foreach (var item in commList_e)
                    {
                        foreach (var alias in item.Aliases)
                        {
                            if (alias == lookup)
                            {
                                commList.Add(item);
                            }
                        }
                    }
                }

                int allMatchs = commList.Count;

                CommandInfo comm = commList[index - 1];

                string all_alias = "";
                IReadOnlyList<string> _aliases = comm.Aliases ?? new List<string>();

                string trueName = comm.Name;

                if (_aliases.ToList().Find(al => al == lookup) == lookup)
                {
                    trueName = lookup;
                }

                if (_aliases.Count != 0)
                {
                    foreach (string alias in comm.Aliases)
                    {
                        all_alias += alias + "\n";
                    }
                }

                string all_para = "No parameters.";
                IReadOnlyList<ParameterInfo> _params = comm.Parameters.ToList() ?? new List<ParameterInfo>();

                if (_params.Count != 0)
                {
                    all_para = "";
                    foreach (var para in comm.Parameters)
                    {
                        if (para.IsOptional)
                        {
                            all_para += GeneralTools.SyntaxHighlightify(para.Type.ToString()) + " " + para.Name + " (optional)\n";
                        }
                        else
                        {
                            all_para += GeneralTools.SyntaxHighlightify(para.Type.ToString()) + " " + para.Name + "\n";
                        }
                    }
                }

                string parentForm = "Nothing";

                if (comm.Module.Parent != null)
                {
                    parentForm = comm.Module.Parent.Name;
                }

                string trueSumm = "No summary given.";
                if (comm.Summary != null) trueSumm = comm.Summary;

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Help",
                    Color = Discord.Color.Magenta,
                    Description = "The newer *better* help. Showing result #" + (index).ToString() + " out of " + allMatchs.ToString() + " match(s).",
                    ThumbnailUrl = Program.xuClient.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Command Name",
                                Value = "`" + trueName + "`" ,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Part of",
                                Value = "```" + parentForm + "```",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Summary",
                                Value = trueSumm,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Known Aliases",
                                Value = "```" + all_alias + "```",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Parameters",
                                Value = "```cs\n" + all_para + "```",
                                IsInline = true
                            }
                        }
                };
                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception e)
            {
                await GeneralTools.CommHandler.BuildError(e, Context);
            }
        }
    }
}
