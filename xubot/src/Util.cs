﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace xubot
{
    public static class Util
    {
        public class Error //(SocketMessage messageParameters)
        {
            private static EmbedBuilder GetErrorBoilerplate()
            {
                return new EmbedBuilder
                {
                    Title = "Error!",
                    Color = Color.Red,
                    Description = "***That's a bad!!!***",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = Globals.EmbedFooter
                    },
                    Timestamp = DateTime.UtcNow
                };
            }

            public static async Task BuildError(IResult result, CommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = "Error Reason",
                        Value = "```" + result.ErrorReason + "```",
                        IsInline = false
                    },
                    new()
                    {
                        Name = "What it is",
                        Value = "```" + result.Error.GetType() + "```",
                        IsInline = false
                    }
                };

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task BuildError(CommandError err, CommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = "Error",
                        Value = "```" + err + "```",
                        IsInline = false
                    }
                };

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task BuildError(Exception exp, ICommandContext context)
            {
                string stack = exp.StackTrace;
                string file = Environment.CurrentDirectory + src.BotSettings.Global.Default.ExceptionLogLocation + DateTime.UtcNow.ToLongTimeString().Replace(':', '_') + ".nonfatal.txt";

                bool stacktraceToFile = exp.StackTrace.Length > 512;

                Directory.CreateDirectory(Environment.CurrentDirectory + src.BotSettings.Global.Default.ExceptionLogLocation);
                System.IO.File.WriteAllText(file, stack);

                if (stacktraceToFile)
                {
                    stack = "Stack trace is too big.";
                }

                if (src.BotSettings.Global.Default.SendStacktraceOnError)
                {
                    stack = "Settings prevent the sending of any stack trace. Check exception logs for a *.nonfatal.txt file.";
                }

                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Description = $"It's a ***{exp.GetType()}***.";
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = "Source",
                        Value = $"```{exp.Source}```",
                        IsInline = false
                    },
                    new()
                    {
                        Name = "Message",
                        Value = $"```{exp.Message}```",
                        IsInline = false
                    },
                    new()
                    {
                        Name = "Target Site",
                        Value = $"```{exp.TargetSite}```",
                        IsInline = false
                    },
                    new()
                    {
                        Name = "Stack Trace",
                        Value = $"```{stack}```",
                        IsInline = false
                    }
                };

                await context.Channel.SendMessageAsync("", false, embed.Build());
                if (stacktraceToFile && src.BotSettings.Global.Default.SendBigStacktraceOnError)
                {
                    await context.Channel.SendFileAsync(file);
                }
            }

            public static async Task BuildError(string problem, ICommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = "Details",
                        Value = "```" + problem + "```",
                        IsInline = false
                    }
                };

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task BuildError(object problem, ICommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Description = "It's a dedicated ***" + problem.GetType() + "*** issue.";
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = "Details",
                        Value = $"```{problem}```",
                        IsInline = false
                    }
                };

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task Deprecated(ICommandContext context)
            {
                await context.Channel.SendMessageAsync("", false, Embed.GetDefaultEmbed(context, "Deprecated!", "This command/feature of xubot is going to be removed in the future.", Color.DarkRed).Build());
            }
        }

        public class Json
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
                if (!System.IO.File.Exists(jsonFile))
                {
                    Log.QuickLog($"a json file was missing: {Path.GetFileName(jsonFile)}");
                    return;
                }
                Program.JsonKeys.Add(key, new Entry(jsonFile, JObject.Parse(System.IO.File.ReadAllText(jsonFile))));
            }

            public static void ProcessObject(string key, object toSerialize)
            {
                Program.JsonKeys.Add(key, new Entry(null, JObject.Parse(JsonConvert.SerializeObject(toSerialize))));
            }

            public static void SaveKeyAsJson(string key)
            {
                System.IO.File.WriteAllText(Program.JsonKeys[key].Filename, JsonConvert.SerializeObject(Program.JsonKeys[key].Contents));
            }

            public static void SaveObjectAsJson(object save, string path)
            {
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(save));
            }
        }

        public class CmdLine
        {
            public static void SetColor(ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
            }
        }

        public class String
        {
            private static readonly Dictionary<string, string> TypeToString = new() {
                { "System.Boolean", "bool"},
                { "System.Byte",    "byte" },       { "System.SByte",   "sbyte" },  { "System.Char",    "char" },
                { "System.Decimal", "decimal" },    { "System.Double",  "double" }, { "System.Single",  "float" },
                { "System.Int32",   "int" },        { "System.UInt32",  "uint" },
                { "System.IntPtr",  "nint" },       { "System.UIntPtr", "nuint" },
                { "System.Int64",   "long" },       { "System.UInt64",  "ulong" },
                { "System.Int16",   "short" },      { "System.UInt16",  "ushort" },
                { "System.String",  "string" },     { "System.Object",  "object" }
            };

            private static readonly Dictionary<string, string> TypeToGenericString = new() {
                { "System.Boolean", "switch"},
                { "System.Byte",    "byte" },      { "System.SByte",   "byte" },   { "System.Char",    "char" },
                { "System.Decimal", "number" },    { "System.Double",  "number" }, { "System.Single",  "number" },
                { "System.Int32",   "number" },    { "System.UInt32",  "number" },
                { "System.IntPtr",  "pointer" },   { "System.UIntPtr", "pointer" },
                { "System.Int64",   "number" },    { "System.UInt64",  "number" },
                { "System.Int16",   "number" },    { "System.UInt16",  "number" },
                { "System.String",  "string" },    { "System.Object",  "object" }
            };

            public static string StripHtml(string input)
            {
                return Regex.Replace(input, "<.*?>", string.Empty);
            }

            public static string SimplifyTypes(string input)
            {
                return src.BotSettings.Global.Default.SuperSimpleTypes ? TypeToGenericString[input] : TypeToString[input];
            }

            public static bool ValidateUrl(string url)
            {
                return Uri.TryCreate(url, UriKind.Absolute, out var result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
            }

            public static string RandomHexadecimal(int length = 32)
            {
                string output = "";
                for (int i = 0; i < length; i++) output += Globals.HexadecimalChars[Globals.Rng.Next(Globals.HexadecimalChars.Length)];

                return output;
            }

            public static string RandomTempFilename()
            {
                return Path.GetTempPath() + RandomHexadecimal();
            }
        }

        public class File
        {
            public static string ReturnLastAttachmentUrl(ICommandContext context)
            {
                var attach = context.Message.Attachments;
                IAttachment attached = null;

                foreach (IAttachment att in attach)
                {
                    attached = att;
                }

                return attached.Url;
            }

            public static List<string> ReturnAttachmentUrLs(ICommandContext context)
            {
                var attach = context.Message.Attachments;
                IAttachment attached = null;
                List<string> results = new List<string>();

                foreach (IAttachment att in attach)
                {
                    results.Add(att.Url);
                }

                return results;
            }

            public static async Task DownloadLastAttachmentAsync(ICommandContext context, string localurl, bool autoApplyFt = false)
            {
                string url = ReturnLastAttachmentUrl(context);
                using HttpClient client = new HttpClient();
                using HttpResponseMessage response = await client.GetAsync(url);
                using HttpContent content = response.Content;
                if (!autoApplyFt)
                {
                    System.IO.File.WriteAllBytes(localurl, await content.ReadAsByteArrayAsync());
                }
                else
                {
                    string type = Path.GetExtension(url);
                    System.IO.File.WriteAllBytes(localurl + type, await content.ReadAsByteArrayAsync());
                }
            }

            public static async Task DownloadFromUrlAsync(string localurl, string url, bool autoApplyFt = false)
            {
                using HttpClient client = new HttpClient();
                using HttpResponseMessage response = await client.GetAsync(url);
                using HttpContent content = response.Content;
                if (!autoApplyFt)
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

        public class Log
        {
            public static async Task QuickLog(string message, ICommandContext context = null)
            {
                PersistLog(message, context);
            }

            public static async Task<IUserMessage> PersistLog(string message, ICommandContext context)
            {
                string logas = $"{DateTime.Now.ToLongTimeString()}]: {message}";
                Console.WriteLine(logas);
                if (context != null) return await context.Channel.SendMessageAsync($"`{logas}`");
                return null;
            }

            public static async Task PersistLog(string message, IUserMessage append)
            {
                string logas = $"[{DateTime.Now.ToLongTimeString()}]: {message}";
                Console.WriteLine(logas);
                if (append != null) await append.ModifyAsync(x => x.Content = $"{append.Content}\n`{logas}`");
            }
        }

        public class Globals
        {
            public static readonly Random Rng = new();
            public static readonly char[] HexadecimalChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            public static readonly Emoji Working = new(src.BotSettings.Global.Default.WorkingReaction);
            public static readonly Emoji Completed = new(src.BotSettings.Global.Default.WorkCompletedReaction);
            public static readonly Emoji LongerThanExpected = new(src.BotSettings.Global.Default.WorkTakingLongerReaction);
            public static readonly string EmbedFooter = "xubot :p";
        }

        public class WorkingBlock : IDisposable
        {
            private readonly ICommandContext _context;
            private bool _started;
            private bool _completed;

            // ReSharper disable once NotAccessedField.Local
            private Task _untilLonger;

            private readonly int _delay = src.BotSettings.Global.Default.TakingLongerMilliseconds;
            private readonly int _taskPollLength = src.BotSettings.Global.Default.TaskPollLength;
            private readonly CancellationTokenSource _cancelToken = new();

            private static readonly IEmote[] RemoveEmotesOnDispose = new IEmote[] { Globals.Working, Globals.LongerThanExpected };

            public WorkingBlock(ICommandContext ctx)
            {
                _context = ctx;
                _started = false;
                Start();
            }

            public void Start()
            {
                if (_started || _completed) return;
                _context.Message.AddReactionAsync(Globals.Working);

                _untilLonger = Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < _delay / _taskPollLength; i++) {
                        Thread.Sleep(_taskPollLength);
                        if (_cancelToken.IsCancellationRequested)
                            return;
                    }
                    _context.Message.AddReactionAsync(Globals.LongerThanExpected);
                }, _cancelToken.Token);

                _started = true;
            }

            public void Dispose()
            {
                _completed = true;
                if (!_started) return;

                _context.Message.RemoveReactionsAsync(Program.XuClient.CurrentUser, RemoveEmotesOnDispose);

                _context.Message.AddReactionAsync(Globals.Completed);

                _cancelToken.Cancel();

                Thread.Sleep(_taskPollLength);
                _cancelToken.Dispose();
            }
        }

        public class Settings
        {
            public static object Get(string key)
            {
                return src.BotSettings.Global.Default.GetType().GetProperty(key).GetValue(src.BotSettings.Global.Default);
            }

            public static void Set<T>(string key, T newValue)
            {
                src.BotSettings.Global.Default.GetType().GetProperty(key).SetValue(src.BotSettings.Global.Default, newValue);
            }
        }

        public class Embed
        {
            public static EmbedBuilder GetDefaultEmbed(ICommandContext context, string title = "", string description = "", Color? color = null)
            {
                return new EmbedBuilder
                {
                    Title = title,
                    Description = description,
                    Color = color,
                    ThumbnailUrl = context.Client.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder
                    {
                        Text = Globals.EmbedFooter,
                        IconUrl = context.Client.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        public static bool IsUserTrusted(ICommandContext context)
        {
            var xdoc = XDocument.Load("Trusted.xml");

            var items = from i in xdoc.Descendants("trust")
                        select new
                        {
                            user = (string)i.Attribute("id")
                        };

            foreach (var item in items)
            {
                if (item.user == context.Message.Author.Id.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<bool> IsChannelNsfw(ICommandContext context)
        {
            if (!src.BotSettings.Global.Default.BotwideNSFWEnabled) return false;

            IDMChannel ifDm = await context.Message.Author.GetOrCreateDMChannelAsync();

            if (ifDm.Id == context.Channel.Id)
            {
                return src.BotSettings.Global.Default.DMsAlwaysNSFW;
            }
            else
            {
                ITextChannel c = context.Channel as ITextChannel;
                return c.IsNsfw;
            }
        }

        public static async Task<bool> IsDmChannel(ICommandContext context)
        {
            IDMChannel ifDm = await context.Message.Author.GetOrCreateDMChannelAsync();

            return ifDm.Id == context.Channel.Id;
        }

        public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
