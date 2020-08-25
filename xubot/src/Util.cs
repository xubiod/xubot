﻿using Discord.WebSocket;
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
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using SteamKit2.Internal;

namespace xubot.src
{
    public static class Util
    {
        public class Error //(SocketMessage messageParameters)
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

                await context.Channel.SendMessageAsync("", false, embedd.Build());
            }

            public static async Task BuildError(CommandError err, CommandContext context)
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
                                Name = "Error",
                                Value = "```" + err + "```",
                                IsInline = false
                            }
                        }
                };

                await context.Channel.SendMessageAsync("", false, embedd.Build());
            }

            public static async Task BuildError(Exception exp, ICommandContext context)
            {
                string stack = exp.StackTrace;
                if (exp.StackTrace.Length > 512)
                {
                    System.IO.File.WriteAllText(Path.Combine(Path.GetTempPath(), "StackTrace.txt"), stack);
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

                await context.Channel.SendMessageAsync("", false, embedd.Build());
                if (exp.StackTrace.Length > 512)
                {
                    await context.Channel.SendFileAsync(Path.Combine(Path.GetTempPath(), "StackTrace.txt"));
                }
            }

            public static async Task BuildError(string problem, ICommandContext context)
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Problem!",
                    Color = Discord.Color.Red,
                    Description = "It's an issue all right! The error builder got *a string!*",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Details",
                                Value = "```" + problem + "```",
                                IsInline = false
                            }
                        }
                };

                await context.Channel.SendMessageAsync("", false, embedd.Build());
            }

            public static async Task BuildError(object problem, ICommandContext context)
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Exception!",
                    Color = Discord.Color.Red,
                    Description = "It's a dedicated ***" + problem.GetType() + "*** issue.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder
                        {
                            Name = "Details",
                            Value = "```" + problem.ToString() + "```",
                            IsInline = false
                        }
                    }
                };

                await context.Channel.SendMessageAsync("", false, embedd.Build());
            }

            public static async Task Deprecated(ICommandContext context)
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Deprecated!",
                    Color = Discord.Color.DarkRed,
                    Description = "This command/feature of xubot is going to be removed in the future.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow
                };

                await context.Channel.SendMessageAsync("", false, embedd.Build());
            }
        }

        public class JSON
        {
            public class Entry
            {
                public string Filename { get; private set; }
                public dynamic Contents { get; set; }

                public Entry(string filename, dynamic contents)
                {
                    Filename = filename; Contents = contents;
                }
            }

            public static void ProcessFile(string key, string jsonFile)
            {
                Program.JSONKeys.Add(key, new Entry(jsonFile, JObject.Parse(System.IO.File.ReadAllText(jsonFile))));
            }

            public static void ProcessObject(string key, object toSerialize)
            {
                Program.JSONKeys.Add(key, new Entry(null, JObject.Parse(JsonConvert.SerializeObject(toSerialize))));
            }

            public static void SaveKeyAsJSON(string key)
            {
                System.IO.File.WriteAllText(Program.JSONKeys[key].Filename, JsonConvert.SerializeObject(Program.JSONKeys[key].Contents));
            }

            public static void SaveObjectAsJSON(object save, string path)
            {
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(save));
            }
        }

        public class CMDLine
        {
            public static void SetColor(ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
            }
        }

        public class Str
        {
            public static string StripHTML(string input)
            {
                return Regex.Replace(input, "<.*?>", String.Empty);
            }

            public static string SyntaxHighlightify(string input)
            {
                switch (input)
                {
                    case "System.String": return "string";
                    case "System.Boolean": return "bool";
                    case "System.Decimal": return "decimal";
                    case "System.Single": return "float";
                    case "System.Double": return "double";
                    case "System.Int32": return "int";
                    case "System.UInt32": return "uint";
                    case "System.Int64": return "long";
                    case "System.UInt64": return "ulong";

                    default: return input;
                }
            }

            public static bool ValidateURL(string url)
            {
                Uri result;
                return Uri.TryCreate(url, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
            }
        }

        public class File
        {
            public static string ReturnLastAttachmentURL(ICommandContext Context)
            {
                var attach = Context.Message.Attachments;
                IAttachment attached = null;

                foreach (IAttachment _att in attach)
                {
                    attached = _att;
                }

                return attached.Url;
            }

            public static List<string> ReturnAttachmentURLs(ICommandContext Context)
            {
                var attach = Context.Message.Attachments;
                IAttachment attached = null;
                List<string> results = new List<string>();

                foreach (IAttachment _att in attach)
                {
                    results.Add(_att.Url);
                }

                return results;
            }

            public static async Task DownloadLastAttachmentAsync(ICommandContext Context, string localurl, bool autoApplyFT = false)
            {
                string url = ReturnLastAttachmentURL(Context);
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    if (!autoApplyFT)
                    {
                        System.IO.File.WriteAllBytes(localurl, await content.ReadAsByteArrayAsync());
                    }
                    else
                    {
                        string type = Path.GetExtension(url);
                        System.IO.File.WriteAllBytes(localurl + type, await content.ReadAsByteArrayAsync());
                    }
                }
            }

            public static async Task DownloadFromURLAsync(string localurl, string url, bool autoApplyFT = false)
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    if (!autoApplyFT)
                    {
                        System.IO.File.WriteAllBytes(localurl, await content.ReadAsByteArrayAsync());
                    }
                    else
                    {
                        string type = Path.GetExtension(url);
                        System.IO.File.WriteAllBytes(localurl + type, await content.ReadAsByteArrayAsync());
                    }
                }
            }
        }

        public static bool IsUserTrusted(ICommandContext Context)
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

        public static async Task<bool> IsChannelNSFW(ICommandContext Context)
        {
            IDMChannel ifDM = await Context.Message.Author.GetOrCreateDMChannelAsync();

            if (ifDM.Id == Context.Channel.Id)
            {
                return true;
            }
            else
            {
                ITextChannel _c = Context.Channel as ITextChannel;
                return _c.IsNsfw;
            }
        }

        public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}