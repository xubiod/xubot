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
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using SteamKit2.Internal;
using System.Threading;

namespace xubot.src
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
                    Color = Discord.Color.Red,
                    Description = "***That's a bad!!!***",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = Util.Globals.EmbedFooter
                    },
                    Timestamp = DateTime.UtcNow
                };
            }

            public static async Task BuildError(IResult result, CommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Fields = new List<EmbedFieldBuilder>()
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
                    new EmbedFieldBuilder
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
                string file = Environment.CurrentDirectory + BotSettings.Global.Default.ExceptionLogLocation + DateTime.UtcNow.ToLongTimeString().Replace(':', '_') + ".nonfatal.txt";

                bool stacktraceToFile = exp.StackTrace.Length > 512;

                Directory.CreateDirectory(Environment.CurrentDirectory + BotSettings.Global.Default.ExceptionLogLocation);
                System.IO.File.WriteAllText(file, stack);

                if (stacktraceToFile)
                {
                    stack = "Stack trace is too big.";
                }

                if (BotSettings.Global.Default.SendStacktraceOnError)
                {
                    stack = "Settings prevent the sending of any stack trace. Check exception logs for a *.nonfatal.txt file.";
                }

                EmbedBuilder embed = GetErrorBoilerplate();

                embed.Description = $"It's a ***{exp.GetType()}***.";
                embed.Fields = new List<EmbedFieldBuilder>()
                {
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
                };

                await context.Channel.SendMessageAsync("", false, embed.Build());
                if (stacktraceToFile && BotSettings.Global.Default.SendBigStacktraceOnError)
                {
                    await context.Channel.SendFileAsync(file);
                }
            }

            public static async Task BuildError(string problem, ICommandContext context)
            {
                EmbedBuilder embed = GetErrorBoilerplate();
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
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
                    new EmbedFieldBuilder
                    {
                        Name = "Details",
                        Value = $"```{problem.ToString()}```",
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
                if (!System.IO.File.Exists(jsonFile))
                {
                    Log.QuickLog($"a json file was missing: {Path.GetFileName(jsonFile)}");
                    return;
                }
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

        public class String
        {
            private static Dictionary<string, string> TypeToString = new Dictionary<string, string>() {
                { "System.Boolean", "bool"},
                { "System.Byte",    "byte" },       { "System.SByte",   "sbyte" },  { "System.Char",    "char" },
                { "System.Decimal", "decimal" },    { "System.Double",  "double" }, { "System.Single",  "float" },
                { "System.Int32",   "int" },        { "System.UInt32",  "uint" },
                { "System.IntPtr",  "nint" },       { "System.UIntPtr", "nuint" },
                { "System.Int64",   "long" },       { "System.Uint64",  "ulong" },
                { "System.Int16",   "short" },      { "System.UInt16",  "ushort" },
                { "System.String",  "string" },     { "System.Object",  "object" }
            };

            public static string StripHTML(string input)
            {
                return Regex.Replace(input, "<.*?>", string.Empty);
            }

            public static string SimplifyTypes(string input)
            {
                return TypeToString[input];
            }

            public static bool ValidateURL(string url)
            {
                Uri result;
                return Uri.TryCreate(url, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
            }

            public static string RandomHexadecimal(int length = 32)
            {
                string output = "";
                for (int i = 0; i < length; i++) output += Globals.HexadecimalChars[Globals.RNG.Next(Globals.HexadecimalChars.Length)];

                return output;
            }

            public static string RandomTempFilename()
            {
                return Path.GetTempPath() + RandomHexadecimal();
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
            public readonly static Random RNG = new Random();
            public readonly static char[] HexadecimalChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            public readonly static Emoji Working = new Emoji("💭");
            public readonly static Emoji Completed = new Emoji("✅");
            public readonly static Emoji LongerThanExpected = new Emoji("🕒");
            public readonly static string EmbedFooter = "xubot :p";
        }

        public class WorkingBlock : IDisposable
        {
            private ICommandContext Context;
            private bool started;
            private bool completed;
            private Task UntilLonger;
            private readonly int Delay = 5000;
            private readonly int TaskPollLength = 50;
            private readonly CancellationTokenSource cancelToken = new CancellationTokenSource();

            public WorkingBlock(ICommandContext ctx)
            {
                Context = ctx;
                started = false;
                Start();
            }

            public void Start()
            {
                if (started || completed) return;
                Context.Message.AddReactionAsync(Util.Globals.Working);

                UntilLonger = Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < (Delay / TaskPollLength); i++) {
                        System.Threading.Thread.Sleep(TaskPollLength);
                        if (cancelToken.IsCancellationRequested)
                            return;
                    }
                    Context.Message.AddReactionAsync(Util.Globals.LongerThanExpected);
                }, cancelToken.Token);

                started = true;
            }

            public void Dispose()
            {
                completed = true;
                if (!started) return;

                Context.Message.RemoveReactionsAsync(Program.xuClient.CurrentUser, new IEmote[] { Util.Globals.Working, Util.Globals.LongerThanExpected });

                Context.Message.AddReactionAsync(Util.Globals.Completed);

                cancelToken.Cancel();

                System.Threading.Thread.Sleep(TaskPollLength);
                cancelToken.Dispose();
            }
        }

        public class Settings
        {
            public static object Get(string key)
            {
                return BotSettings.Global.Default.GetType().GetProperty(key).GetValue(BotSettings.Global.Default);
            }

            public static void Set<T>(string key, T newValue)
            {
                BotSettings.Global.Default.GetType().GetProperty(key).SetValue(BotSettings.Global.Default, newValue);
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
                        Text = Util.Globals.EmbedFooter,
                        IconUrl = context.Client.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow
                };
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
            if (!BotSettings.Global.Default.BotwideNSFWEnabled) return false;

            IDMChannel ifDM = await Context.Message.Author.GetOrCreateDMChannelAsync();

            if (ifDM.Id == Context.Channel.Id)
            {
                return BotSettings.Global.Default.DMsAlwaysNSFW;
            }
            else
            {
                ITextChannel _c = Context.Channel as ITextChannel;
                return _c.IsNsfw;
            }
        }

        public static async Task<bool> IsDMChannel(ICommandContext Context)
        {
            IDMChannel ifDM = await Context.Message.Author.GetOrCreateDMChannelAsync();

            return ifDM.Id == Context.Channel.Id;
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
