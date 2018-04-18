using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    public class Wiki : ModuleBase
    {
        [Command("test")]
        public async Task test()
        {
            await ReplyAsync("", false, WikiTools.BuildEmbed());
        }
    }

    public class WikiTools
    {
        public static EmbedBuilder BuildEmbed()
        {
            return new EmbedBuilder
            {
                Title = "_",
                Color = Discord.Color.Gold,
                Description = "_",

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "_",
                                Value = "_",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Value = new EmbedFieldBuilder
                                {
                                    Name = "what",
                                    Value = "testing",
                                    IsInline = false
                                }
                            }
                        }
            };
        }
    }
}
