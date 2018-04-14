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
using Discord.Net.Providers.WS4Net;
using RedditSharp;
using Tweetinvi;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Collections.Generic;
namespace xubot
{
    public class Program : ModuleBase
    {
        public static CommandService xuCommand;
        public static DiscordSocketClient xuClient;
        
        public static string prefix = "[>";

        public static BotWebAgent webAgent;
        public static Reddit reddit;
        public static RedditSharp.Things.Subreddit subreddit;

        public static bool botf_reddit = false;
        public static dynamic keys;
        public static dynamic apiJson;
        public static JToken perserv;
        public static dynamic perserv_parsed;
        public static bool enableNSFW = false;

        public static bool first = true;

        public static async Task Main(string[] args)
        {
            Console.SetWindowSize(80, 25);

            xuClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                WebSocketProvider = WS4NetProvider.Instance,
                LogLevel = LogSeverity.Warning
            });
            xuCommand = new CommandService();

            //ImageTypeReader itr = new ImageTypeReader();
            //xuCommand.AddTypeReader<Image>(itr);

            string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            keys = JObject.Parse(File.ReadAllText(Path.Combine(currentDir, "Keys.json")));
            apiJson = JObject.Parse(File.ReadAllText(Path.Combine(currentDir, "API.json")));

            await commandInitiation();
            await readMessages();

            xuClient.Log += (message) =>
            {
                Console.WriteLine($"{message}");
                return Task.Delay(0);
            };

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("- - - - - - - - - - - - - - - -  xubot  startup  - - - - - - - - - - - - - - - -");
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine("current build (git): {0}", ThisAssembly.Git.Tag);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            if (!File.Exists(Path.Combine(currentDir, "Keys.json"))) {
                Console.WriteLine("!!! the keys file is missing!!!");
                Console.ReadLine();
            }

            Console.WriteLine("* setting up bot web agent for reddit use");
            if (keys.reddit.user.ToString() == "" && keys.reddit.pass.ToString() == "") {
                Console.WriteLine("  > reddit info not provided, disabling reddit");
            }
            else {
                botf_reddit = true;
                webAgent = new BotWebAgent(keys.reddit.user.ToString(), keys.reddit.pass.ToString(), keys.reddit.key1.ToString(), keys.reddit.key2.ToString(), "https://www.reddit.com/api/v1/authorize?client_id=CLIENT_ID&response_type=TYPE&state=RANDOM_STRING&redirect_uri=URI&duration=DURATION&scope=SCOPE_STRING");
                Console.WriteLine("* setting up reddit client");
                reddit = new Reddit(webAgent, true);

                Console.WriteLine("* setting up default subreddit of /r/xubot_subreddit");
                subreddit = await reddit.GetSubredditAsync("/r/xubot_subreddit");
            }
            Console.WriteLine("* setting up discord connection: login");

#if (dev)
            Console.WriteLine("  > this version of xubot was compiled as dev");
            prefix = "d>";
#endif
            if (prefix != "d>")
            {
                await xuClient.LoginAsync(TokenType.Bot, keys.discord.ToString());
            } else
            {
                await xuClient.LoginAsync(TokenType.Bot, keys.discord_dev.ToString());
            }

            Console.WriteLine("* setting up discord connection: starting client");
            await BeginStart();

            xuClient.Ready += ClientReady;
            xuClient.UserJoined += XuClient_UserJoined;
            Console.WriteLine();

            await Task.Delay(-1);
        }

        public static Task XuClient_UserJoined(SocketGuildUser arg)
        {
            return Task.CompletedTask;
        }

        private static async Task BeginStart()
        {
            await xuClient.StartAsync();
        }

        public static Task ClientReady()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("                                                                                ");
            Console.Write("                                    ready!!!                                    ");
            Console.Write("                                                                                ");

            //await xuClient.SetGameAsync("xubot is alive!");
            Console.Beep();

            RefreshPerServ();

            return Task.CompletedTask;
        }

        public static Task readMessages()
        {
            xuClient.MessageReceived += (message) =>
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"[{message.Timestamp}] {{{message.Source}}} {message.Author}: {message.Content}");
                
                return Task.CompletedTask;
            };

            //console logs hhheeerrrrrrrr
            xuClient.Connected += XuClient_Connected;
            xuClient.Disconnected += XuClient_Disconnected;

            xuClient.LoggedIn += XuClient_LoggedIn;
            xuClient.LoggedOut += XuClient_LoggedOut;

            xuClient.JoinedGuild += XuClient_JoinedGuild;
            xuClient.LeftGuild += XuClient_LeftGuild;
            xuClient.GuildAvailable += XuClient_GuildAvailableAsync;

            return Task.CompletedTask;
        }

        private static Task XuClient_GuildAvailable(SocketGuild arg)
        {
            Console.WriteLine("guild list: " + arg.Name);
            return Task.CompletedTask;
        }

        private static Task XuClient_JoinedGuild(SocketGuild arg)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.Write("                                                                                ");
            Console.Write("                               added to a guild!!                               ");
            Console.Write("                                                                                ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("the guild name: " + arg.Name);
            Console.WriteLine();
            return Task.CompletedTask;
        }

        private static Task XuClient_LeftGuild(SocketGuild arg)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.Write("                                                                                ");
            Console.Write("                               left a guild... :<                               ");
            Console.Write("                   probably got kicked/banned which is rude.                    ");
            Console.Write("                                                                                ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("the guild name: " + arg.Name);
            Console.WriteLine();
            return Task.CompletedTask;
        }

        private static Task XuClient_LoggedIn()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.Write("                                                                                ");
            Console.Write("                                   logged in!                                   ");
            Console.Write("                                                                                ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            return Task.CompletedTask;
        }

        private static Task XuClient_LoggedOut()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                                                                                ");
            Console.Write("                                  logged out.                                   ");
            Console.Write("                                                                                ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            return Task.CompletedTask;
        }

        private static Task XuClient_Connected()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                                                                                ");
            Console.Write("                              connection successful!                            ");
            Console.Write("                                                                                ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            
            return Task.CompletedTask;
        }

        private static Task XuClient_Disconnected(Exception arg)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("                                                                                ");
            Console.Write("                                connection lost...                              ");
            Console.Write("                               probably got dropped                             ");
            Console.Write("                                                                                ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Exception logged at: " + Environment.CurrentDirectory + "\\Exceptions\\latest.txt");
            Console.Beep();
            Console.Beep();
            Console.Beep();

            Directory.CreateDirectory(Environment.CurrentDirectory + "\\Exceptions\\");
            File.WriteAllText(Environment.CurrentDirectory + "\\Exceptions\\latest.txt", arg.ToString());

            Thread.Sleep(2500);

            //Console.Read();

            //await BeginStart();

            return Task.CompletedTask;
        }

        public static async Task XuClient_GuildAvailableAsync(SocketGuild arg)
        {
            if (first)
            {
                Console.WriteLine("guild list: " + arg.Name);

                var xdoc = XDocument.Load("PerServTrigg.xml");

                var items = from i in xdoc.Descendants("server")
                            select new
                            {
                                guildid = i.Attribute("id"),
                                onwake = i.Attribute("onwake")
                            };

                foreach (var item in items)
                {
                    if (item.guildid.Value == arg.Id.ToString())
                    {
                        if (item.onwake.Value != "")
                        {
                            await arg.DefaultChannel.SendMessageAsync(item.onwake.Value);
                        }
                    }
                }

                first = false;
            }

        }

        public static async Task commandInitiation()
        {
            xuClient.MessageReceived += handleCommands;
            await xuCommand.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public static async Task handleCommands(SocketMessage messageParameters)
        {
            var message = messageParameters as SocketUserMessage;
            if (message == null) return;

            int argumentPosition = 0;

            if (!(message.HasStringPrefix(prefix, ref argumentPosition) || message.HasMentionPrefix(xuClient.CurrentUser, ref argumentPosition)))
                return;

            var context = new CommandContext(xuClient, message);

            var result = await xuCommand.ExecuteAsync(context, argumentPosition);
            if (!result.IsSuccess)
            {
                await GeneralTools.CommHandler.BuildError(result, context);
            }
        }

        public static void RefreshPerServ()
        {
            string text;
            using (var sr = new StreamReader("PerServerTrigg.json"))
            {
                text = sr.ReadToEnd();
            }

            perserv = JObject.Parse(text);
            perserv_parsed = JObject.Parse(text);
            text = null;
        }

        public static bool ServerTrigger_Detect(ICommandContext Context)
        {
            RefreshPerServ();
            try
            {
                string guild = Context.Guild.Id.ToString() + "_onwake";
                return (perserv.Value<String>(guild).ToString() ?? "") == "";
            }
            catch (Exception fuck)
            {
                Context.Channel.SendMessageAsync(fuck.ToString());
                return false;
            }
        }
    }
}