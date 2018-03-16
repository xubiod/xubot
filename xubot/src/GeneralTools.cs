using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace xubot
{
    public static class GeneralTools
    {
        public class CommHandler //(SocketMessage messageParameters)
        {
            public static async Task BuildError(IResult result, CommandContext context)
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Error!",
                    Color = Discord.Color.Red,
                    Description = "***That's a bad!!!***",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Error Reason",
                                Value = "```" + result.ErrorReason + "```",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "What it is",
                                Value = "```" + result.Error + "```",
                                IsInline = false
                            }

                        }
                };

                await context.Channel.SendMessageAsync("", false, embedd);
            }
        }
    }
}
