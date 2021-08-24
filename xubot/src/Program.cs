﻿using System;
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
using RedditSharp;
using System.Diagnostics;
using Tweetinvi.Controllers.Auth;

namespace xubot.src
{
    public class Program : ModuleBase
    {
        public static readonly CommandService XuCommand = new CommandService();
        public static readonly DiscordSocketClient XuClient = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Warning });

        public static bool IsOffline = false;

        public static string prefix = BotSettings.Global.Default.DefaultPrefix;

        public static BotWebAgent WebAgent { get; private set; }
        public static RedditSharp.Reddit Reddit { get; private set; }
        public static RedditSharp.Things.Subreddit Subreddit { get; set; }

        public static bool redditEnabled = false;

        public static readonly Dictionary<string, Util.Json.Entry> JsonKeys = new Dictionary<string, Util.Json.Entry>();

        public static bool enableNsfw = false;

        public static bool spinning = false;

        public static DateTime AppStart { get; private set; }
        public static DateTime ConnectStart { get; private set; }

        public static DateTime[] stepTimes = new DateTime[8];

        private static bool _firstLaunch = true;

        public static async Task Main(string[] args)
        {
            if (!args.Contains("offline"))
            {
                BeginOnlineStart(args);

                XuClient.Ready += ClientReady;
                XuClient.UserJoined += XuClient_UserJoined;

                Commands.Shitpost.Populate();
                Modular.ModularSystem.Initialize();

                await Task.Delay(-1);
            }
            else
            {
                IsOffline = true;
                BeginOfflineStart(args);

                Commands.Shitpost.Populate();
                Modular.ModularSystem.Initialize();

                bool running = true;
                string input;
                SocketUserMessage offlineMessage;

                do
                {
                    input = Console.ReadLine();
                    HandleOfflineCommands(input);
                } while (running);
            }
        }

        public static Task XuClient_UserJoined(SocketGuildUser arg)
        {
            return Task.CompletedTask;
        }

        private static async Task BeginOnlineStart(string[] args)
        {
            AppStart = DateTime.Now;

            string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Util.Json.ProcessFile("keys", Path.Combine(currentDir, "Keys.json"));
            Util.Json.ProcessFile("apis", Path.Combine(currentDir, "API.json"));
            Util.Json.ProcessFile("mood", Path.Combine(currentDir, "Moods.json"));
            Util.Json.ProcessFile("opinion", Path.Combine(currentDir, "Opinions.json"));

            await CommandInitiation();
            await ReadMessages();

            XuClient.Log += (message) => { Console.WriteLine($"{message}"); return Task.CompletedTask; };

            Util.CmdLine.SetColor();

            Console.Write("[[ xubot ]]");
            Console.WriteLine();

            Util.CmdLine.SetColor(ConsoleColor.Magenta);

            Console.WriteLine("current build (git): {0}", ThisAssembly.Git.Tag);

            Util.CmdLine.SetColor();
            Console.WriteLine();

            if (!File.Exists(Path.Combine(currentDir, "Keys.json")))
            {
                Console.WriteLine("!!! the keys file is missing!!!");
                Console.ReadLine();
            }

            // if (false) { } // !args.Contains("no-reddit"))
            Console.WriteLine("* setting up discord connection: login");

#if (DEBUG)
            Console.WriteLine("  > this version of xubot was compiled as debug build");
            prefix = BotSettings.Global.Default.DefaultDevPrefix;
#endif
            if (prefix != BotSettings.Global.Default.DefaultDevPrefix)
            {
                await XuClient.LoginAsync(TokenType.Bot, JsonKeys["keys"].Contents.discord.ToString());
            }
            else
            {
                await XuClient.LoginAsync(TokenType.Bot, JsonKeys["keys"].Contents.discord_dev.ToString());
            }
            stepTimes[2] = DateTime.Now;

            Console.WriteLine("* setting up discord connection: starting client");

            await XuClient.StartAsync();
            if (!BotSettings.Global.Default.DisableRedditOnStart) await ReadyReddit();
        }

        private static async Task BeginOfflineStart(string[] args)
        {
            AppStart = DateTime.Now;

            string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Util.Json.ProcessFile("keys", Path.Combine(currentDir, "Keys.json"));
            Util.Json.ProcessFile("apis", Path.Combine(currentDir, "API.json"));
            Util.Json.ProcessFile("mood", Path.Combine(currentDir, "Moods.json"));
            Util.Json.ProcessFile("opinion", Path.Combine(currentDir, "Opinions.json"));

            await XuCommand.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            Util.CmdLine.SetColor();

            Console.Write("[[ xubot ]]\n");
            Console.WriteLine("no connection to discord mode");
            Console.WriteLine("dubbed offline mode despite not being offline with everything else");
            Console.WriteLine();

            Util.CmdLine.SetColor(ConsoleColor.Magenta);

            Console.WriteLine("current build (git): {0}", ThisAssembly.Git.Tag);

            Util.CmdLine.SetColor();
            Console.WriteLine("skipping logging into and starting discord client\n");

            Util.CmdLine.SetColor(ConsoleColor.Green);
            Console.WriteLine("ready for console input\n");
            Console.Beep();

            Util.CmdLine.SetColor();
            if (!BotSettings.Global.Default.DisableRedditOnStart) await ReadyReddit();
        }

        public static async Task ReadyReddit(ICommandContext context = null)
        {
            IUserMessage log = Util.Log.PersistLog("setting up bot web agent for reddit use", context).Result;

            if (JsonKeys["keys"].Contents.reddit.user.ToString() == "" && JsonKeys["keys"].Contents.reddit.pass.ToString() == "")
            {
                Util.Log.PersistLog("reddit info not provided within keys, aborting", log);
            }
            else
            {
                try
                {
                    redditEnabled = true;
                    WebAgent = new BotWebAgent(
                        JsonKeys["keys"].Contents.reddit.user.ToString(),
                        JsonKeys["keys"].Contents.reddit.pass.ToString(),
                        JsonKeys["keys"].Contents.reddit.id.ToString(),
                        JsonKeys["keys"].Contents.reddit.secret.ToString(),
                        "https://www.reddit.com/api/v1/authorize?client_id=CLIENT_ID&response_type=TYPE&state=RANDOM_STRING&redirect_uri=URI&duration=DURATION&scope=SCOPE_STRING");

                    Util.Log.PersistLog("setting up reddit client", log);
                    Reddit = new Reddit(WebAgent, true);
                    //_red.Wait();

                    stepTimes[0] = DateTime.Now;

                    // Console.WriteLine("* setting up default subreddit of /r/xubot_subreddit");
                    // subreddit = await reddit.GetSubredditAsync("/r/xubot_subreddit");
                    stepTimes[1] = DateTime.Now;
                }
                catch (Exception e)
                {
                    Console.ReadLine();
                }
            }
        }

        public static Task ClientReady()
        {
            Util.CmdLine.SetColor(ConsoleColor.Green);

            Console.WriteLine("]] ready for action");
            Console.Beep();

            return Task.CompletedTask;
        }

        public static Task ReadMessages()
        {
            XuClient.MessageReceived += (message) =>
            {
                Util.CmdLine.SetColor();
                Console.WriteLine($"[{message.Timestamp}] {{{message.Source}}} {message.Author}: {message.Content}");

                return Task.CompletedTask;
            };

            //console logs hhheeerrrrrrrr
            XuClient.Connected += XuClient_Connected;
            XuClient.Disconnected += XuClient_Disconnected;

            XuClient.LoggedIn +=  () => { Console.WriteLine("]] logged into discord");   return Task.CompletedTask; };
            XuClient.LoggedOut += () => { Console.WriteLine("]] logged out of discord"); return Task.CompletedTask; };

            XuClient.JoinedGuild += (SocketGuild arg) => { Console.WriteLine($"]] added to a guild, {arg.Name}"); return Task.CompletedTask; };
            XuClient.LeftGuild +=   (SocketGuild arg) => { Console.WriteLine($"]] left a guild, {arg.Name}");     return Task.CompletedTask; };

            return Task.CompletedTask;
        }

        private static Task XuClient_Connected()
        {
            Util.CmdLine.SetColor(ConsoleColor.Green);
            Console.WriteLine("]] connection to discord successful");

            ConnectStart = DateTime.Now;

            return Task.CompletedTask;
        }

        private static Task XuClient_Disconnected(Exception arg)
        {
            Console.WriteLine("]] connection to discord lost");
            Console.WriteLine();
            Console.WriteLine($"]] exception logged at: {Environment.CurrentDirectory}\\Exceptions\\{DateTime.UtcNow.ToLongTimeString()}.txt");
            Console.Beep();
            Console.Beep();
            Console.Beep();

            Directory.CreateDirectory(Environment.CurrentDirectory + BotSettings.Global.Default.ExceptionLogLocation);
            File.WriteAllText(Environment.CurrentDirectory + BotSettings.Global.Default.ExceptionLogLocation + DateTime.UtcNow.ToLongTimeString().Replace(':', '_') + ".txt", arg.ToString());

            Thread.Sleep(2500);

            //Console.Read();

            //await BeginStart();

            return Task.CompletedTask;
        }

        public static async Task CommandInitiation()
        {
            XuClient.MessageReceived += HandleCommands;
            await XuCommand.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        public static async Task HandleCommands(SocketMessage messageParameters)
        {
            SocketUserMessage message = messageParameters as SocketUserMessage;
            if (message == null) return;

            int argumentPosition = 0;

            if (!(message.HasStringPrefix(prefix, ref argumentPosition) || message.HasMentionPrefix(XuClient.CurrentUser, ref argumentPosition) || message.HasStringPrefix(BotSettings.Global.Default.HardcodedPrefix, ref argumentPosition)))
                return;

            CommandContext context = new CommandContext(XuClient, message);

            IResult result = await XuCommand.ExecuteAsync(context, argumentPosition, null);
            if (!result.IsSuccess)
            {
                await Util.Error.BuildError(result, context);
            }
        }

        public static async Task HandleOfflineCommands(string message)
        {
            Offline.OfflineMessage msg = new Offline.OfflineMessage();
            msg.Content = message;

            int argumentPosition = 0;

            if (!(msg.HasStringPrefix(prefix, ref argumentPosition) || msg.HasMentionPrefix(XuClient.CurrentUser, ref argumentPosition) || msg.HasStringPrefix(BotSettings.Global.Default.HardcodedPrefix, ref argumentPosition)))
                return;

            CommandContext context = new CommandContext(Offline.OfflineHandlers.DefaultOfflineClient, msg);

            IResult result = await XuCommand.ExecuteAsync(context, argumentPosition, null);
            if (!result.IsSuccess)
            {
                await Util.Error.BuildError(result, context);
            }
        }
    }
}
