using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xubot.src.BotSettings;

namespace xubot
{
    public static class Util
    {
        public static class Error //(SocketMessage messageParameters)
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

            public static async Task BuildErrorAsync(IResult result, CommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();

                var error = result.Error;
                if (error == null) return;

                embed.Fields =
                [
                    new EmbedFieldBuilder
                    {
                        Name = "Error Reason",
                        Value = "```" + result.ErrorReason + "```",
                        IsInline = false
                    },

                    new EmbedFieldBuilder
                    {
                        Name = "What it is",
                        Value = "```" + error.GetType() + "```",
                        IsInline = false
                    }
                ];

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task BuildErrorAsync(CommandError err, CommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Fields =
                [
                    new EmbedFieldBuilder
                    {
                        Name = "CommandError Error",
                        Value = "```" + err + "```",
                        IsInline = false
                    }
                ];

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task BuildErrorAsync(Exception exp, ICommandContext context)
            {
                string stack = exp.StackTrace;
                if (stack == null) return;

                string file = Environment.CurrentDirectory + Global.Default.ExceptionLogLocation + DateTime.UtcNow.ToLongTimeString().Replace(':', '_') + ".nonfatal.txt";

                bool stacktraceToFile = stack.Length > 512;

                Directory.CreateDirectory(Environment.CurrentDirectory + Global.Default.ExceptionLogLocation);
                await System.IO.File.WriteAllTextAsync(file, stack);

                if (stacktraceToFile)
                {
                    stack = "Stack trace is too big.";
                }

                if (Global.Default.SendStacktraceOnError)
                {
                    stack = "Settings prevent the sending of any stack trace. Check exception logs for a *.nonfatal.txt file.";
                }

                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Description = $"It's a ***{exp.GetType()}***.";
                embed.Fields =
                [
                    new EmbedFieldBuilder
                    {
                        Name = "Source",
                        Value = $"```{exp.Source}```",
                        IsInline = false
                    },

                    new EmbedFieldBuilder
                    {
                        Name = "Message",
                        Value = $"```{exp.Message}```",
                        IsInline = false
                    },

                    new EmbedFieldBuilder
                    {
                        Name = "Target Site",
                        Value = $"```{exp.TargetSite}```",
                        IsInline = false
                    },

                    new EmbedFieldBuilder
                    {
                        Name = "Stack Trace",
                        Value = $"```{stack}```",
                        IsInline = false
                    }
                ];

                await context.Channel.SendMessageAsync("", false, embed.Build());
                if (stacktraceToFile && Global.Default.SendBigStacktraceOnError)
                {
                    await context.Channel.SendFileAsync(file);
                }
            }

            public static async Task BuildErrorAsync(string problem, ICommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();
                embed.Fields =
                [
                    new EmbedFieldBuilder
                    {
                        Name = "Details",
                        Value = "```" + problem + "```",
                        IsInline = false
                    }
                ];

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task BuildErrorAsync(object problem, ICommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Description = "It's a dedicated ***" + problem.GetType() + "*** issue.";
                embed.Fields =
                [
                    new EmbedFieldBuilder
                    {
                        Name = "Details",
                        Value = $"```{problem}```",
                        IsInline = false
                    }
                ];

                await context.Channel.SendMessageAsync("", false, embed.Build());
            }

            public static async Task DeprecatedAsync(ICommandContext context)
            {
                await context.Channel.SendMessageAsync("", false, Embed.GetDefaultEmbed(context, "Deprecated!", "This command/feature of xubot is going to be removed in the future.", Color.DarkRed).Build());
            }
        }

        public static class Json
        {
            public class Entry(string filename, dynamic contents)
            {
                public string Filename { get; } = filename;
                public dynamic Contents { get; } = contents;
            }

            public static async void ProcessFile(string key, string jsonFile)
            {
                if (!System.IO.File.Exists(jsonFile))
                {
                    await Log.QuickLogAsync($"a json file was missing: {Path.GetFileName(jsonFile)}");
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
                SaveKeyAsJson(key, Program.JsonKeys[key].Filename);
            }

            public static void SaveKeyAsJson(string key, string filename)
            {
                System.IO.File.WriteAllText(filename, JsonConvert.SerializeObject(Program.JsonKeys[key].Contents));
            }

            public static void SaveObjectAsJson(object save, string path)
            {
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(save));
            }
        }

        public static class CmdLine
        {
            public static void SetColor(ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
            }
        }

        public static class String
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
                return Global.Default.SuperSimpleTypes ? TypeToGenericString[input] : TypeToString[input];
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

        public static class File
        {
            public static string ReturnLastAttachmentUrl(ICommandContext context)
            {
                return ReturnAttachmentUrls(context).Last();
            }

            public static List<string> ReturnAttachmentUrls(ICommandContext context)
            {
                var attach = context.Message.Attachments;
                List<string> results = new();

                foreach (IAttachment att in attach)
                {
                    results.Add(att.Url);
                }

                return results;
            }

            public static async Task DownloadLastAttachmentAsync(ICommandContext context, string localUrl, bool autoApplyFileType = false)
            {
                await DownloadFromUrlAsync(localUrl, ReturnLastAttachmentUrl(context), autoApplyFileType);
            }

            public static async Task DownloadFromUrlAsync(string localUrl, string url, bool autoApplyFileType = false)
            {
                using HttpClient client = new();
                using HttpResponseMessage response = await client.GetAsync(url);
                using HttpContent content = response.Content;
                if (!autoApplyFileType)
                {
                    await System.IO.File.WriteAllBytesAsync(localUrl, await content.ReadAsByteArrayAsync());
                }
                else
                {
                    string type = Path.GetExtension(url);
                    await System.IO.File.WriteAllBytesAsync(localUrl + type, await content.ReadAsByteArrayAsync());
                }
            }
        }

        public static class Log
        {
            public static async Task QuickLogAsync(string message, ICommandContext context = null)
            {
                await PersistLogAsync(message, context);
            }

            public static async Task<IUserMessage> PersistLogAsync(string message, ICommandContext context)
            {
                string logAs = $"{DateTime.Now.ToLongTimeString()}]: {message}";
                Console.WriteLine(logAs);
                if (context != null) return await context.Channel.SendMessageAsync($"`{logAs}`");
                return null;
            }

            public static async Task PersistLogAsync(string message, IUserMessage append)
            {
                string logAs = $"[{DateTime.Now.ToLongTimeString()}]: {message}";
                Console.WriteLine(logAs);
                if (append != null) await append.ModifyAsync(x => x.Content = $"{append.Content}\n`{logAs}`");
            }
        }

        public static class Globals
        {
            public static readonly Random Rng = new();
            public static readonly char[] HexadecimalChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            public static readonly Emoji Working = new(Global.Default.WorkingReaction);
            public static readonly Emoji Completed = new(Global.Default.WorkCompletedReaction);
            public static readonly Emoji LongerThanExpected = new(Global.Default.WorkTakingLongerReaction);
            public const string EmbedFooter = "xubot";
        }

        public class WorkingBlock : IDisposable
        {
            private readonly ICommandContext _context;
            private bool _started;
            private bool _completed;

            // ReSharper disable once NotAccessedField.Local
            private Task _untilLonger;

            private readonly int _delay = Global.Default.TakingLongerMilliseconds;
            private readonly int _taskPollLength = Global.Default.TaskPollLength;
            private readonly CancellationTokenSource _cancelToken = new();

            private static readonly IEmote[] RemoveEmotesOnDispose = { Globals.Working, Globals.LongerThanExpected };

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

        public static class Settings
        {
            public static object Get(string key)
            {
                var property = Global.Default.GetType().GetProperty(key);
                return property != null ? property.GetValue(Global.Default) : null;
            }

            public static void Set<T>(string key, T newValue)
            {
                var property = Global.Default.GetType().GetProperty(key);
                if (property != null) property.SetValue(Global.Default, newValue);
            }
        }

        public static class Embed
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
            return false;
        }

        public static async Task<bool> IsChannelNsfwAsync(ICommandContext context)
        {
            if (!Global.Default.BotwideNSFWEnabled) return false;

            if (await IsDmChannelAsync(context)) return Global.Default.DMsAlwaysNSFW;

            ITextChannel c = context.Channel as ITextChannel;
            return c?.IsNsfw ?? false;
        }

        public static async Task<bool> IsDmChannelAsync(ICommandContext context)
        {
            IDMChannel ifDm = await context.Message.Author.CreateDMChannelAsync();

            return ifDm.Id == context.Channel.Id;
        }

        public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
