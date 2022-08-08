using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using xubot.Attributes;
using Color = Discord.Color;
using IUser = Discord.IUser;

namespace xubot.Commands
{
    public class GenericCommands : ModuleBase
    {
        private static readonly string[] insultVictim = new string[128];
        private static int insultVictimIndex;

        private static readonly string[] insultAdjective = new string[128];
        private static int insultAdjectiveIndex;

        private static readonly string[] insultNoun = new string[128];
        private static int insultNounIndex;

        private static readonly string[] pattern = { "01110", "11011", "10001", "11011", "01110" };

        private static readonly TwitterClient Twitter = new(
            new TwitterCredentials(
                Program.JsonKeys["keys"].Contents.twitter.consumer_key.ToString(), Program.JsonKeys["keys"].Contents.twitter.consumer_secret.ToString(),
                Program.JsonKeys["keys"].Contents.twitter.access_key.ToString(),   Program.JsonKeys["keys"].Contents.twitter.access_secret.ToString()
            )
        );

        [Group("echo"), Alias("m"), Summary("Repeats after you.")]
        public class Echo : ModuleBase
        {
            [Example("\"polly want a cracker\"")]
            [Command, Summary("Repeats a string given once.")]
            public async Task RepeatOnce(string blegh)
            {
                await ReplyAsync(blegh);
            }

            [Example("\"polly want a cracker\" 5 \" \"")]
            [Command("repeat"), Alias("r"), Summary("Repeats a string a given amount of times."), RequireUserPermission(ChannelPermission.ManageMessages)]
            public async Task Repeat(string message, int loop, string sep)
            {
                string echoRes = "";

                for (int i = 0; i <= loop; i++)
                {
                    echoRes += message + sep;
                }

                await ReplyAsync(echoRes);
                //await ReplyAsync("Currently not usable due to spam reasons.");
            }
        }

        [Group("insult"), Summary("Get insulted by software.")]
        public class Insult : ModuleBase
        {
            [Command("init"), Summary("Initializes the insult choices.")]
            public async Task Init()
            {
                Array.Clear(insultVictim, 0, 128);
                insultVictim[0] = "You are a ";
                insultVictim[1] = "Your face is a ";
                insultVictim[2] = "Your body is a ";
                insultVictim[3] = "Your code is a ";
                insultVictimIndex = 3;

                Array.Clear(insultAdjective, 0, 128);
                insultAdjective[0] = "steaming piece of ";
                insultAdjective[1] = "smelly ";
                insultAdjective[2] = "small ";
                insultAdjective[3] = "fat ";
                insultAdjectiveIndex = 3;

                Array.Clear(insultNoun, 0, 128);
                insultNoun[0] = "rotting poo.";
                insultNoun[1] = "dick. ";
                insultNoun[2] = "spoiled oatmeal.";
                insultNoun[3] = "arsehole.";
                insultNounIndex = 3;

                await ReplyAsync("Reset the insult arrays.");
            }

            [Command("list"), Summary("Displays the insult arrays' contents.")]
            public async Task List()
            {
                string v = "**V**(itim): `[";
                for (int i = 0; i < insultVictimIndex; i++)
                {
                    v += insultVictim[i];

                    if (i != 128)
                    {
                        v += "] [";
                    }
                }
                v += "]`";

                // ReSharper disable once StringLiteralTypo
                string a = "**A**(djective): `[";
                for (int i = 0; i < insultAdjectiveIndex; i++)
                {
                    a += insultAdjective[i];

                    if (i != 128)
                    {
                        a += "] [";
                    }
                }
                a += "]`";

                string n = "**N**(oun): `[";
                for (int i = 0; i < insultNounIndex; i++)
                {
                    n += insultNoun[i];

                    if (i != 128)
                    {
                        n += "] [";
                    }
                }
                n += "]`";

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "List of Insults", "To add something to any list, use `[>insult add [LIST LETTER in BOLD] [STRING]`.", Color.Orange);
                embed.Fields = new List<EmbedFieldBuilder>
                {
                    new()
                    {
                        Name = "Lists",
                        Value = $"{v}\n\n{a}\n\n{n}",
                        IsInline = false
                    }
                };

                await ReplyAsync("", false, embed.Build());
            }

            [Group("add")]
            public class Add : ModuleBase
            {
                [Example("Noun")]
                [Command("v"), Summary("Adds a string to the 'victim' list.")]
                public async Task Vit(String input)
                {
                    insultVictimIndex++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insultVictim[insultVictimIndex] = input;

                    await ReplyAsync($"Added \"{input}\".");
                }

                [Example("\"looking like\"")]
                [Command("a"), Summary("Adds a string to the 'adjective' list.")]
                public async Task Adj(String input)
                {
                    insultAdjectiveIndex++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insultAdjective[insultAdjectiveIndex] = input;

                    await ReplyAsync($"Added \"{input}\".");
                }

                [Example("noun.")]
                [Command("n"), Summary("Adds a string to the 'noun' list.")]
                public async Task Nou(String input)
                {
                    insultNounIndex++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insultNoun[insultNounIndex] = input;

                    await ReplyAsync($"Added \"{input}\".");
                }
            }

            [Command("generate"), Summary("Generates an insult.")]
            public async Task Gen()
            {
                Random rnd = Util.Globals.Rng;
                int insultVUse = rnd.Next(insultVictimIndex);
                int insultAUse = rnd.Next(insultAdjectiveIndex);
                int insultNUse = rnd.Next(insultNounIndex);

                await ReplyAsync(insultVictim[insultVUse] + insultAdjective[insultAUse] + insultNoun[insultNUse]);
            }
        }

        [Group("pattern"), Alias("pat"), Summary("Makes a pattern with given characters.")]
        public class Pattern : ModuleBase
        {
            [Example("club X O")]
            [Command("generate"), Summary("Generates a premade pattern using a search term.")]
            public async Task GeneratePreset(string searchqueue, string emo1, string emo2)
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    pattern[i] = PatternPresets.ReturnQuery(searchqueue, 1).Replace("0", emo1).Replace("1", emo2);
                }

                await ReplyAsync(pattern[0] + '\n' + pattern[1] + '\n' + pattern[2] + '\n' + pattern[3] + '\n' + pattern[4]);
            }
        }

        [Group("post"), Alias("p"), Summary("Post to ...")]
        public class Post2Media : ModuleBase
        {
            [Command("")]
            public async Task Default_()
            {
                await ReplyAsync("You didn't give me a service.");
            }

            //[Command("reddit"), Alias("r", "reddit"), Summary("Attempts to post a text post to Reddit.")]
            //public async Task RedditPost(string title, string content)
            //{
            //    if (Program.redditEnabled == false)
            //    {
            //        await ReplyAsync("Reddit token not provided by bot runner.");
            //    }
            //    else
            //    {
            //        Program.subreddit = await Program.reddit.GetSubredditAsync("/r/xubot_subreddit");

            //        string result_ = content;
            //        var redditPost = await Program.subreddit.SubmitTextPostAsync(title, Context.Message.Author.Username + "#" + Context.Message.Author.Discriminator + " on the Discord server " + Context.Guild.Name + " posted:\n\n" + result_);
            //        await ReplyAsync("<" + redditPost.Url.AbsoluteUri}>");
            //    }
            //}

            [Example("\"I am creatively starved and pasted in the example usage command to see what it did\"")]
            [Command("twitter"), Alias("t", "twit"), Summary("Attempts to post a thing to Twitter. Substitute `@` and `#` with [A] and [H] prospectively.")]
            public async Task TweetPost(string content)
            {
                string result = content.Replace("[A]", "@").Replace("[H]", "#");

                if ((Context.Message.Author.Username + "#" + Context.Message.Author.Discriminator + ": " + result).Length < 280)
                {
                    ITweet twt = await Twitter.Tweets.PublishTweetAsync($"{Context.Message.Author.Username}#{Context.Message.Author.Discriminator}: {result}");

                    await ReplyAsync(twt.Url);
                }
                else
                {
                    await ReplyAsync("Did you know that tweets have a limit of 280 characters?");
                }
            }
        }

        [Example("316712338042650624 7")]
        [Command("discord-api-link-gen"), Alias("discord-bot", "db"), Summary("Generates a bot adding link with a given permission number.")]
        public async Task GenerateDiscordBotLink(string id, long permission = 0)
        {
            await ReplyAsync($"https://discordapp.com/api/oauth2/authorize?client_id={id}&scope=bot&permissions={permission}");
        }

        public class SettingsComm : ModuleBase
        {
            [Group("settings-old"), Alias("set", "~"), Summary("Modify some bot stuff. Most of it is restricted.")]
            public class Settings : ModuleBase
            {
                [Command("!"), Alias("kill"), Summary("Kills the bot."), RequireOwner]
                public async Task End()
                {
                    await ReplyAsync("Ending...");
                    Environment.Exit(0);
                }

                [Group("nsfw-commands"), Summary("The toggle for the defunct NSFW restriction (Bot now uses channel's NSFW flag instead.)")]
                public class NsfwSet : ModuleBase
                {
                    [Command]
                    public async Task Get()
                    {
                        await ReplyAsync($"Currently NSFW execution is: **{Program.EnableNsfw.ToString().ToLower()}** *(true means NSFW commands are executable)*");
                    }

                    [Command("set"), RequireOwner]
                    public async Task Set(bool newValue)
                    {
                        Program.EnableNsfw = newValue;
                        await ReplyAsync($"NSFW execution is now: **{newValue.ToString().ToLower()}**");
                    }
                }

                [Example("\"with C#\"")]
                [Command("playing"), Alias("play", "game"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Playing(string newPlay)
                {
                    await Program.XuClient.SetGameAsync(newPlay);
                    await ReplyAsync($"*Activity* has been set to: **{newPlay}**");
                }

                [Example("\"my crash logs directory getting full\"")]
                [Command("watching"), Alias("watch"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Watching(string newPlay)
                {
                    await Program.XuClient.SetGameAsync(newPlay, null, ActivityType.Watching);
                    await ReplyAsync($"*Activity* has been set to: **{newPlay}**");
                }

                [Example("\"to computer fans being overworked\"")]
                [Command("listening"), Alias("listen"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Listening(string newPlay)
                {
                    await Program.XuClient.SetGameAsync(newPlay, null, ActivityType.Listening);
                    await ReplyAsync($"*Activity* has been set to: **{newPlay}**");
                }

                [Example("\"bits across tubes\"")]
                [Command("streaming"), Alias("stream"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Streaming(string newPlay)
                {
                    await Program.XuClient.SetGameAsync(newPlay, "https://www.twitch.tv/xubiod_chat_bot", ActivityType.Streaming);
                    await ReplyAsync($"*Activity* has been set to: **{newPlay}**" +
                                    "\n*Status* has been set to: **streaming**");
                }

                [Example("online")]
                [Command("status"), Alias("stat"), Summary("Sets the bot's status."), RequireOwner]
                public async Task Status(string newPlay)
                {
                    switch (newPlay.ToLower())
                    {
                        case "online":
                        case "on":
                            {
                                await Program.XuClient.SetStatusAsync(UserStatus.Online);
                                break;
                            }

                        case "invisible":
                        case "offline":
                            {
                                await Program.XuClient.SetStatusAsync(UserStatus.Invisible);
                                break;
                            }

                        case "afk":
                        case "away":
                            {
                                await Program.XuClient.SetStatusAsync(UserStatus.AFK);
                                break;
                            }

                        case "idle":
                            {
                                await Program.XuClient.SetStatusAsync(UserStatus.Idle);
                                break;
                            }

                        case "dnd":
                        case "silence":
                            {
                                await Program.XuClient.SetStatusAsync(UserStatus.DoNotDisturb);
                                break;
                            }

                        default:
                            {
                                await ReplyAsync($"*Status* hasn't been set to: **{newPlay}**, it's invalid.");
                                return;
                            }
                    }

                    await ReplyAsync($"*Status* has been set to: **{newPlay}**");
                }

                [Example("[>")]
                [Command("temp_prefix"), Alias("prefix"), Summary("Sets the prefix for current session."), RequireOwner]
                public async Task Prefix(string newPrefix)
                {
                    Program.Prefix = newPrefix;
                    await ReplyAsync($"*Prefix* has been set for this session to: **{newPrefix}**");
                }

                [Command("ping"), Alias("#", "latency"), Summary("Gets the latency from message recieved to reply.")]
                public async Task Ping()
                {
                    await ReplyAsync($"*Ping latency* is currently at: **{Program.XuClient.Latency} milliseconds.**");
                }

                [Command("connection_state"), Alias("cs", "connect"), Summary("Gets the bot's connection state.")]
                public async Task Cs()
                {
                    await ReplyAsync($"*Connection state* is currently at: **{Program.XuClient.ConnectionState}.**");
                }
            }
        }

        public class RandomComm : ModuleBase
        {
            /// <summary>
            /// this is just a split
            ///
            /// jokes below
            /// </summary>
            ///
            [Example("10")]
            [Command("gen"), Summary("Makes a random integer with the number given as maximum.")]
            public async Task RndDefault(int max)
            {
                Random rnd = Util.Globals.Rng;

                await ReplyAsync($"Random number generated: **{rnd.Next(max)}**");
            }

            [Example("\"This is an example\"")]
            [Command("leet-speak"), Alias("1337"), Summary("Takes input and returns leet-speak.")]
            public async Task LeetSpeak(string input)
            {
                input = input.Replace('i', '!');
                input = input.Replace('I', '1');

                input = input.Replace('o', '0');
                input = input.Replace('O', '0');

                input = input.Replace('l', '1');
                input = input.Replace('L', '7');

                input = input.Replace('e', '3');
                input = input.Replace('E', '3');

                input = input.Replace('h', '4');
                input = input.Replace('H', '#');

                input = input.Replace('s', '5');
                input = input.Replace('S', '$');

                input = input.Replace('a', '@');
                input = input.Replace('A', '@');

                input = input.Replace('g', '9');
                input = input.Replace('G', '6');

                input = input.Replace('t', '7');
                input = input.Replace('T', '7');

                input = input.Replace('B', '8');
                input = input.Replace('b', '6');

                input = input.Replace('q', '9');

                await ReplyAsync(input);
            }

            [Example("\"This is an example\"")]
            // ReSharper disable once StringLiteralTypo
            [Command("moar-leet-speak"), Alias("moar1337"), Summary("Takes input and returns leet-speak. (more character substitution)")]
            public async Task LeetSpeakAdv(string input)
            {
                input = input.Replace('a', '@');
                input = input.Replace("A", "/-\\\\\\");

                input = input.Replace('b', '6');
                input = input.Replace('B', '8');

                //c
                //C

                //d
                //D

                input = input.Replace('e', '3');
                input = input.Replace('E', '3');

                //f
                //F

                input = input.Replace('g', '9');
                input = input.Replace('G', '6');

                input = input.Replace('h', '4');
                input = input.Replace('H', '#');

                input = input.Replace('i', '!');
                input = input.Replace('I', '1');

                //j
                //J

                input = input.Replace("k", "|<");
                input = input.Replace("K", "|<");

                input = input.Replace('l', '1');
                input = input.Replace('L', '7');

                input = input.Replace("m", '|' + "\\\\" + "\\" + '/' + "|");
                input = input.Replace("M", '|' + "\\\\" + "\\" + '/' + "|");

                //n
                //N

                input = input.Replace('o', '0');
                input = input.Replace('O', '0');

                input = input.Replace("p", "|>");
                input = input.Replace("P", "|>");

                input = input.Replace('q', '9');
                //Q

                //r
                //R

                input = input.Replace('s', '5');
                input = input.Replace('S', '$');

                input = input.Replace('t', '7');
                input = input.Replace('T', '7');

                input = input.Replace("u", "L|");
                input = input.Replace("U", "|_|");

                input = input.Replace("v", "\\/");
                input = input.Replace("V", "\\/");

                input = input.Replace("w", '|' + "/" + '\\' + '\\' + "|");
                input = input.Replace("W", '|' + "/" + '\\' + '\\' + "|");

                input = input.Replace("x", "><");
                //X

                input = input.Replace("y", "'-/");
                input = input.Replace("Y", "'-/");

                //z
                //Z

                await ReplyAsync(input);
            }
        }
    }
}
