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
        [Command("help?beta")]
        public async Task helpbeta(string lookup, int index = 1)
        {
            try {
                List<CommandInfo> commList = Program.xuCommand.Commands.ToList();

                commList = commList.FindAll(ci => ci.Name == lookup);

                CommandInfo comm = commList[index-1];
                int allMatchs = commList.Count;

                string all_alias = "";
                IReadOnlyList<string> _aliases = comm.Aliases ?? new List<string>();

                if (_aliases.Count != 0)
                {
                    foreach (string alias in comm.Aliases)
                    {
                        all_alias += alias + "\n";
                    }
                }

                string all_para = "";
                IReadOnlyList<ParameterInfo> _params = comm.Parameters.ToList() ?? new List<ParameterInfo>();

                if (_params.Count != 0)
                {
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

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Help (Beta)",
                    Color = Discord.Color.Magenta,
                    Description = "The newer *better* help. Showing result #"+ (index).ToString() + " out of " + allMatchs.ToString() + " match(s).",
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
                                Value = "`" + comm.Name + "`" ,
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
            } catch (Exception e) {
                await GeneralTools.CommHandler.BuildError(e, Context);
            }
        }
    }
}
