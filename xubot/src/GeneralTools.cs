using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.IO;
using System.Net.Http;
using System.Xml.Linq;

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

            public static async Task BuildError(Exception exp, ICommandContext context)
            {
                string stack = exp.StackTrace;
                if (exp.StackTrace.Length > 512)
                {
                    File.WriteAllText(Path.Combine(Path.GetTempPath(), "StackTrace.txt"), stack);
                    stack = "Stack trace is too big. Reference the provided file.";
                }
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Exception!",
                    Color = Discord.Color.Red,
                    Description = "It's a ***" + exp.GetType() + "***.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Source",
                                Value = "```" + exp.Source + "```",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Message",
                                Value = "```" + exp.Message + "```",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Target Site",
                                Value = "```" + exp.TargetSite + "```",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Stack Trace",
                                Value = "```" + stack + "```",
                                IsInline = false
                            }

                        }
                };

                await context.Channel.SendMessageAsync("", false, embedd);
                if (exp.StackTrace.Length > 512)
                {
                    await context.Channel.SendFileAsync(Path.Combine(Path.GetTempPath(), "StackTrace.txt"));
                }
            }

        }
        public static string ReturnAttachmentURL(ICommandContext Context)
        {
            var attach = Context.Message.Attachments;
            IAttachment attached = null;

            foreach (var _att in attach)
            {
                attached = _att;
            }

            return attached.Url;
        }

        public static async Task DownloadAttachmentAsync(string localurl, string url)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                File.WriteAllText(localurl, await content.ReadAsStringAsync());
            }
        }

        public static bool UserTrusted(ICommandContext Context)
        {
            var xdoc = XDocument.Load("Trusted.xml");

            var items = from i in xdoc.Descendants("trust")
                        select new
                        {
                            user = (string)i.Attribute("id")
                        };

            foreach (var item in items)
            {
                if (item.user == Context.Message.Author.Id.ToString())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
