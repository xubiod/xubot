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

namespace xubot_core.src
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

                await context.Channel.SendMessageAsync("", false, embedd.Build());
                if (exp.StackTrace.Length > 512)
                {
                    await context.Channel.SendFileAsync(Path.Combine(Path.GetTempPath(), "StackTrace.txt"));
                }
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

        public class XML
        {
            private static XDocument xdoc;
            public static string filename;
            public static string element;
            public static List<string> attrib;
            public static List<string> attrib_def;
            public static bool exists = false;

            public static void AddRefresh(IUser arg)
            {
                throw new NotImplementedException();

                xdoc = XDocument.Load(filename);

                var items = from i in xdoc.Descendants(element)
                            select new
                            {
                                user = i.Attribute(attrib[0]),
                                preferred = i.Attribute(attrib[1])
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    Console.WriteLine("new user found to add to {0}, doing that now", filename);

                    XElement xelm = new XElement(element);
                    XAttribute user = new XAttribute(attrib[0], arg.Id.ToString());
                    XAttribute prefer = new XAttribute(attrib[1], attrib_def[1]);

                    xelm.Add(user);
                    xelm.Add(prefer);

                    xdoc.Root.Add(xelm);
                    xdoc.Save(filename);
                }
            }

            public static string Read(IUser arg)
            {
                throw new NotImplementedException();

                xdoc = XDocument.Load(filename);

                var items = from i in xdoc.Descendants(element)
                            select new
                            {
                                user = i.Attribute(attrib[0]),
                                preferred = i.Attribute(attrib[1])
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        return item.preferred.Value;
                    }
                }

                return "not set!";
            }

            public static void Set(IUser arg, string newVal)
            {
                throw new NotImplementedException();

                xdoc = XDocument.Load(filename);

                var items = from i in xdoc.Descendants(element)
                            select new
                            {
                                user = i.Attribute(attrib[0]),
                                preferred = i.Attribute(attrib[1])
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        item.preferred.Value = newVal;
                    }
                }

                xdoc.Save(filename);
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

        public static async Task DownloadAttachmentAsync(ICommandContext Context, string localurl, bool autoApplyFT = false)
        {
            string url = ReturnAttachmentURL(Context);
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                if (!autoApplyFT)
                {
                    File.WriteAllBytes(localurl, await content.ReadAsByteArrayAsync());
                }
                else
                {
                    string type = Path.GetExtension(url);
                    File.WriteAllBytes(localurl + type, await content.ReadAsByteArrayAsync());
                }
            }
        }

        public static async Task DownloadAttachmentAsync(string localurl, string url, bool autoApplyFT = false)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                if (!autoApplyFT)
                {
                    File.WriteAllBytes(localurl, await content.ReadAsByteArrayAsync());
                }
                else
                {
                    string type = Path.GetExtension(url);
                    File.WriteAllBytes(localurl + type, await content.ReadAsByteArrayAsync());
                }
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

        public static async Task<bool> ChannelNSFW(ICommandContext Context)
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

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string SyntaxHighlightify(string input)
        {
            switch (input)
            {
                case "System.String":  return "string";
                case "System.Boolean": return "bool";
                case "System.Decimal": return "decimal";
                case "System.Single":  return "float";
                case "System.Double":  return "double";
                case "System.Int32":   return "int";
                case "System.UInt32":  return "uint";
                case "System.Int64":   return "long";
                case "System.UInt64":  return "ulong";

                default: return input;
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
