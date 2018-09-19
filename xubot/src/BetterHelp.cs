using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    public class BetterHelp : ModuleBase
    {
        public double itemsPerPage = 10.0;

        [Command("help?beta")]
        public async Task helpbeta(string lookup, int index = 1)
        {
            try
            {
                List<CommandInfo> commList = Program.xuCommand.Commands.ToList();

                commList = commList.FindAll(ci => ci.Name == (lookup.Split(' ').Last()));

                CommandInfo comm = commList[index - 1];
                int allMatchs = commList.Count;

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
                            all_para += para.Type + " " + para.Name + " (optional)\n";
                        }
                        else
                        {
                            all_para += para.Type + " " + para.Name + "\n";
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
                    Title = "Help (Beta)",
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
                                Value = "```" + all_para + "```",
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

        [Command("help?beta list")]
        public async Task helpbeta(int page = 1)
        {
            List<CommandInfo> commList = Program.xuCommand.Commands.ToList();

            string items = "";

            
            for (int i = 0; i < itemsPerPage; i++)
            {
                int index = i + (page-1)*10;
                if (index > commList.Count) break;

                string parentForm = "No parent";
                if (commList[index].Module.Parent != null)
                {
                    parentForm = commList[index].Module.Parent.Name;
                }

                items += "(" + parentForm + ") " + commList[index].Name + "\n";
            }

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Help (Beta)",
                Color = Discord.Color.Magenta,
                Description = "The newer *better* help. Showing page #" + (page).ToString() + " out of " + Math.Ceiling(commList.Count/itemsPerPage).ToString() + " pages.",
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
    }
}
