using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Discord;
using Discord.Net.WebSockets;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Discord.Audio;

using Tweetinvi;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Collections.Generic;
using xubot.src;
using System.Runtime.InteropServices;
using Reddit;
using Reddit.Things;
using Reddit.AuthTokenRetriever;
using System.Diagnostics;
using Tweetinvi.Controllers.Auth;

namespace xubot.src
{
    public class Program : ModuleBase
    {
        public static readonly CommandService xuCommand = new CommandService();
        public static readonly DiscordSocketClient xuClient = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Warning });

        public static string prefix = "[>";

        public static RedditClient reddit { get; private set; }
        public static Subreddit subreddit;

        public static bool redditEnabled = false;

        public readonly static Dictionary<string, Util.JSON.Entry> JSONKeys = new Dictionary<string, Util.JSON.Entry>();

        public static bool enableNSFW = false;

        public static bool spinning = false;

        public static DateTime appStart { get; private set; }
        public static DateTime connectStart { get; private set; }

        public static DateTime[] stepTimes = new DateTime[8];

        private static bool firstLaunch = true;

        public static async Task Main(string[] args)
        {
            BeginStart(args);

            xuClient.Ready += ClientReady;
            xuClient.UserJoined += XuClient_UserJoined;

            Commands.Shitpost.Populate();

            await Task.Delay(-1);
        }

        public static Task XuClient_UserJoined(SocketGuildUser arg)
        {
            return Task.CompletedTask;
        }

        private static async Task BeginStart(string[] args)
        {
            appStart = DateTime.Now;

            string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Util.JSON.ProcessFile("keys", Path.Combine(currentDir, "Keys.json"));
            Util.JSON.ProcessFile("apis", Path.Combine(currentDir, "API.json"));
            Util.JSON.ProcessFile("mood", Path.Combine(currentDir, "Moods.json"));
            Util.JSON.ProcessFile("opinion", Path.Combine(currentDir, "Opinions.json"));

            await CommandInitiation();
            await ReadMessages();

            xuClient.Log += (message) => { Console.WriteLine($"{message}"); return Task.CompletedTask; };

            Util.CMDLine.SetColor();

            Console.Write("[[ xubot ]]");
            Console.WriteLine();

            Util.CMDLine.SetColor(ConsoleColor.Magenta);

            Console.WriteLine("current build (git): {0}", ThisAssembly.Git.Tag);

            Util.CMDLine.SetColor();
            Console.WriteLine();

            if (!File.Exists(Path.Combine(currentDir, "Keys.json")))
            {
                Console.WriteLine("!!! the keys file is missing!!!");
                Console.ReadLine();
            }

            if (false)// !args.Contains("no-reddit"))
            {

            }
            Console.WriteLine("* setting up discord connection: login");

#if (DEBUG)
            Console.WriteLine("  > this version of xubot was compiled as debug build");
            prefix = "d>";
#endif
            if (prefix != "d>")
            {
                await xuClient.LoginAsync(TokenType.Bot, JSONKeys["keys"].Contents.discord.ToString());
            }
            else
            {
                await xuClient.LoginAsync(TokenType.Bot, JSONKeys["keys"].Contents.discord_dev.ToString());
            }
            stepTimes[2] = DateTime.Now;

            Console.WriteLine("* setting up discord connection: starting client");

            await xuClient.StartAsync();
        }

        public static void ReadyReddit(ICommandContext context = null)
        {
            throw new SpecialException.PleaseKillMeException();

            //IUserMessage log = Util.Log.PersistLog("setting up bot web agent for reddit use", context).Result;

            //if (JSONKeys["keys"].Contents.reddit.user.ToString() == "" && JSONKeys["keys"].Contents.reddit.pass.ToString() == "")
            //{
            //    Util.Log.PersistLog("reddit info not provided within keys, aborting", log);
            //}
            //else
            //{
            //    redditEnabled = true;
            //    //webAgent = new BotWebAgent(
            //    //    JSONKeys["keys"].Contents.reddit.user.ToString(),
            //    //    JSONKeys["keys"].Contents.reddit.pass.ToString(),
            //    //    JSONKeys["keys"].Contents.reddit.id.ToString(),
            //    //    JSONKeys["keys"].Contents.reddit.secret.ToString(),
            //    //    "https://www.reddit.com/api/v1/authorize?client_id=CLIENT_ID&response_type=TYPE&state=RANDOM_STRING&redirect_uri=URI&duration=DURATION&scope=SCOPE_STRING");

            //    Util.Log.PersistLog("setting up reddit client", log);

            //    Program.reddit = new Reddit.RedditClient(JSONKeys["keys"].Contents.reddit.id.ToString(),);
            //    //_red.Wait();

            //    stepTimes[0] = DateTime.Now;

            //    // Console.WriteLine("* setting up default subreddit of /r/xubot_subreddit");
            //    // subreddit = await reddit.GetSubredditAsync("/r/xubot_subreddit");
            //    stepTimes[1] = DateTime.Now;
            //}
        }

        public static Task ClientReady()
        {
            Util.CMDLine.SetColor(ConsoleColor.Green);

            Console.WriteLine("]] ready for action");
            Console.Beep();

            return Task.CompletedTask;
        }

        public static Task ReadMessages()
        {
            xuClient.MessageReceived += (message) =>
            {
                Util.CMDLine.SetColor();
                Console.WriteLine($"[{message.Timestamp}] {{{message.Source}}} {message.Author}: {message.Content}");

                return Task.CompletedTask;
            };

            //console logs hhheeerrrrrrrr
            xuClient.Connected += XuClient_Connected;
            xuClient.Disconnected += XuClient_Disconnected;

            xuClient.LoggedIn +=  () => { Console.WriteLine("]] logged into discord");   return Task.CompletedTask; };
            xuClient.LoggedOut += () => { Console.WriteLine("]] logged out of discord"); return Task.CompletedTask; };

            xuClient.JoinedGuild += XuClient_JoinedGuild;
            xuClient.LeftGuild += XuClient_LeftGuild;

            return Task.CompletedTask;
        }

        private static Task XuClient_JoinedGuild(SocketGuild arg)
        {
            Console.WriteLine("]] added to a guild, " + arg.Name);
            return Task.CompletedTask;
        }

        private static Task XuClient_LeftGuild(SocketGuild arg)
        {
            Console.WriteLine("]] left a guild, " + arg.Name);
            return Task.CompletedTask;
        }

        private static Task XuClient_Connected()
        {
            Util.CMDLine.SetColor(ConsoleColor.Green);
            Console.WriteLine("]] connection to discord successful");

            connectStart = DateTime.Now;

            return Task.CompletedTask;
        }

        private static Task XuClient_Disconnected(Exception arg)
        {
            Console.WriteLine("]] connection to discord lost");
            Console.WriteLine();
            Console.WriteLine("]] exception logged at: " + Environment.CurrentDirectory + "\\Exceptions\\" + DateTime.UtcNow.ToLongTimeString() + ".txt");
            Console.Beep();
            Console.Beep();
            Console.Beep();

            Directory.CreateDirectory(Environment.CurrentDirectory + "\\Exceptions\\");
            File.WriteAllText(Environment.CurrentDirectory + "\\Exceptions\\" + DateTime.UtcNow.ToLongTimeString() + ".txt", arg.ToString());

            Thread.Sleep(2500);

            //Console.Read();

            //await BeginStart();

            return Task.CompletedTask;
        }

        public static async Task CommandInitiation()
        {
            xuClient.MessageReceived += HandleCommands;
            await xuCommand.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        public static async Task HandleCommands(SocketMessage messageParameters)
        {
            SocketUserMessage message = messageParameters as SocketUserMessage;
            if (message == null) return;

            int argumentPosition = 0;

            if (!(message.HasStringPrefix(prefix, ref argumentPosition) || message.HasMentionPrefix(xuClient.CurrentUser, ref argumentPosition) || message.HasStringPrefix("xub>", ref argumentPosition)))
                return;

            CommandContext context = new CommandContext(xuClient, message);

            IResult result = await xuCommand.ExecuteAsync(context, argumentPosition, null);
            if (!result.IsSuccess)
            {
                await Util.Error.BuildError(result, context);
            }
        }
    }
}
