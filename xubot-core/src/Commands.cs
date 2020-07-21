using Discord;
using Discord.Commands;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using RedditSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using xubot_core;
using xubot_core.src;
using static xubot_core.src.SpecialException;
using SLImage = SixLabors.ImageSharp.Image;

namespace xubot_core.src
{
    public class Commands : ModuleBase
    {
        public static string[] insultVictim = new string[128];
        public static int insultVictimIndex = 0;

        public static string[] insultAdjective = new string[128];
        public static int insultAdjectiveIndex = 0;

        public static string[] insultNoun = new string[128];
        public static int insultNounIndex = 0;

        public static string[] pattern = { "01110", "11011", "10001", "11011", "01110" };

        [Group("echo"), Alias("m"), Summary("Repeats after you.")]
        public class Echo : ModuleBase
        {
            [Command(""), Summary("Repeats a string given once.")]
            public async Task RepeatOnce(string blegh)
            {
                await ReplyAsync(blegh);
            }

            [Command("repeat"), Alias("r"), Summary("Repeats a string a given amount of times."), RequireUserPermission(Discord.ChannelPermission.ManageMessages)]
            public async Task Repeat(string blegh, int loop, string sep)
            {
                string echo_res = "";

                for (int i = 0; i <= loop; i++)
                {
                    echo_res += blegh + sep;
                }

                await ReplyAsync(echo_res);
                //await ReplyAsync("Currently not usable due to spam reasons.");
            }
        }

        [Group("insult"), Summary("Get insulted by software.")]
        public class Insult : ModuleBase
        {
            [Command("init"), Summary("Initalizes the insult choices.")]
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
                string _v = "**V**(itim): `[";
                for (int i = 0; i < insultVictimIndex; i++)
                {
                    _v += insultVictim[i];

                    if (i != 128)
                    {
                        _v += "] [";
                    }
                }
                _v += "]`";

                string _a = "**A**(djective): `[";
                for (int i = 0; i < insultAdjectiveIndex; i++)
                {
                    _a += insultAdjective[i];

                    if (i != 128)
                    {
                        _a += "] [";
                    }
                }
                _a += "]`";

                string _n = "**N**(oun): `[";
                for (int i = 0; i < insultNounIndex; i++)
                {
                    _n += insultNoun[i];

                    if (i != 128)
                    {
                        _n += "] [";
                    }
                }
                _n += "]`";

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Insults",
                    Color = Discord.Color.Orange,
                    Description = "To add something to any list, use `[>insult add [LIST LETTER in BOLD] [STRING]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Lists",
                                Value = _v + '\n' + '\n' + _a +  '\n' + '\n' + _n,
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd.Build());
            }

            [Group("add")]
            public class Add : ModuleBase
            {
                [Command("v"), Summary("Adds a string to the 'victim' list.")]
                public async Task Vit(String input)
                {
                    insultVictimIndex++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insultVictim[insultVictimIndex] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }

                [Command("a"), Summary("Adds a string to the 'adjective' list.")]
                public async Task Adj(String input)
                {
                    insultAdjectiveIndex++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insultAdjective[insultAdjectiveIndex] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }

                [Command("n"), Summary("Adds a string to the 'noun' list.")]
                public async Task Nou(String input)
                {
                    insultNounIndex++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insultNoun[insultNounIndex] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }
            }

            [Command("generate"), Summary("Generates an insult.")]
            public async Task Gen()
            {
                Random rnd = new Random();
                int insult_v_use = rnd.Next(insultVictimIndex);
                int insult_a_use = rnd.Next(insultAdjectiveIndex);
                int insult_n_use = rnd.Next(insultNounIndex);

                await ReplyAsync(insultVictim[insult_v_use] + insultAdjective[insult_a_use] + insultNoun[insult_n_use]);
            }
        }

        [Group("pattern"), Alias("pat"), Summary("Makes a pattern with given characters.")]
        public class Pattern : ModuleBase
        {
            [Command("generate"), Summary("Generates a premade pattern using a search term.")]
            public async Task GeneratePreset(string searchqueue, string emo1, string emo2)
            {
                for (int i = 0; i < pattern.Length; i++)
                {
                    pattern[i] = Pattern_Presets.Return_Query(searchqueue, 1).Replace("0", emo1).Replace("1", emo2);
                }

                await ReplyAsync(pattern[0] + '\n' + pattern[1] + '\n' + pattern[2] + '\n' + pattern[3] + '\n' + pattern[4]);
            }
        }

        [Group("post"), Alias("p"), Summary("post to ...")]
        public class Post2Media : ModuleBase
        {
            [Command("")]
            public async Task Default_()
            {
                await ReplyAsync("You didn't give me a service... ;<");
            }

            [Command("reddit"), Alias("r", "redd"), Summary("Attempts to post a text post to Reddit.")]
            public async Task RedditPost(string title, string content)
            {
                if (Program.redditEnabled == false)
                {
                    await ReplyAsync("Reddit token not provided by bot runner.");
                }
                else
                {
                    Program.subreddit = await Program.reddit.GetSubredditAsync("/r/xubot_subreddit");

                    string result_ = content;
                    var redditPost = await Program.subreddit.SubmitTextPostAsync(title, Context.Message.Author.Username + "#" + Context.Message.Author.Discriminator + " on the Discord server " + Context.Guild.Name + " posted:\n\n" + result_);
                    await ReplyAsync("<" + redditPost.Url.AbsoluteUri.ToString() + ">");
                }
            }

            [Command("twitter"), Alias("t", "twit"), Summary("Attempts to post a thing to Twitter. Substitute `@` and `#` with [A] and [H] prospectively.")]
            public async Task TweetPost(string content)
            {
                string result_ = content.Replace("[A]", "@").Replace("[H]", "#");

                Auth.SetUserCredentials(Program.keys.twitter.key1.ToString(), Program.keys.twitter.key2.ToString(), Program.keys.twitter.key3.ToString(), Program.keys.twitter.key4.ToString());

                if ((Context.Message.Author.Username + "#" + Context.Message.Author.Discriminator + ": " + result_).Length < 280)
                {
                    ITweet twt = Tweet.PublishTweet(Context.Message.Author.Username + "#" + Context.Message.Author.Discriminator + ": " + result_);

                    await ReplyAsync(twt.Url);
                    //await ReplyAsync("Your post has been submitted to twitter. Go to https://twitter.com/xubot_bot to find it!");
                }
                else
                {
                    await ReplyAsync("Did you know that tweets have a limit of 280 characters?");
                }
            }
        }

        [Command("discord-api-link-gen"), Alias("discord-bot", "db"), Summary("Generates a bot adding link (without any permissions.)")]
        public async Task DALG(string id)
        {
            await ReplyAsync("https://discordapp.com/api/oauth2/authorize?client_id=" + id + "&scope=bot&permissions=0");
        }

        [Command("discord-api-link-gen"), Alias("discord-bot", "db"), Summary("Generates a bot adding link with a given permission number.")]
        public async Task DALG(string id, string permission)
        {
            await ReplyAsync("https://discordapp.com/api/oauth2/authorize?client_id=" + id + "&scope=bot&permissions=" + permission);
        }

#if (DEBUG)

        [Group("debug"), Summary("A group of debug commands for quick debug work. Cannot be used by anyone except owner."), RequireOwner]
        public class Debug : ModuleBase
        {
            [Command("return_attachs")]
            public async Task ReturnAttachs()
            {
                var attach = Context.Message.Attachments;
                IAttachment attached = null;

                foreach (var tempAttachment in attach)
                {
                    attached = tempAttachment;
                }

                await ReplyAsync(attach.ToString() + "\nURL:" + attached.Url, false);
            }

            [Command("return_source")]
            public async Task Test001()
            {
                var stuff = Context.Message.Source;

                await ReplyAsync(stuff.ToString(), false);
            }

            [Command("return_type")]
            public async Task Test002()
            {
                var stuff = Context.Message.Type;

                await ReplyAsync(stuff.ToString(), false);
            }

            [Command("get_mood")]
            public async Task Test003(ulong id)
            {
                MoodTools.AddOrRefreshMood(Program.xuClient.GetUser(id));
                double mood = MoodTools.ReadMood(Program.xuClient.GetUser(id));
                string moodAsStr = "invalid";

                if (-16 <= mood && mood <= 16) { moodAsStr = "neutral"; }
                else if (-16 >= mood) { moodAsStr = "negative"; }
                else if (mood >= 16) { moodAsStr = "positive"; }

                await ReplyAsync(mood.ToString() + " / " + moodAsStr, false);
            }

            [Command("throw_new")]
            public async Task Test004(int id)
            {
                try
                {
                    switch (id)
                    {
                        case 0: throw new ItsFuckingBrokenException();
                        case 1: throw new IHaveNoFuckingIdeaException();
                        case 2: throw new PleaseKillMeException();
                        case 3: throw new ShitCodeException();
                        case 4: throw new StopDoingThisMethodException();
                        case 5: throw new ExceptionException();
                        case 6: throw new InsertBetterExceptionNameException();
                        default:
                            {
                                await ReplyAsync("invaild id");
                                break;
                            }
                    }
                }
                catch (Exception exp)
                {
                    await Util.Error.BuildError(exp, Context);
                }
            }

            [Command("ct")]
            public async Task Test005()
            {
                await ReplyAsync(Discord.Color.LightOrange.ToString());
            }

            [Command("li")]
            public async Task Test006()
            {
                List<Discord.WebSocket.SocketGuild> guild_list = Program.xuClient.Guilds.ToList();
                string _all = "";

                foreach (var item in guild_list)
                {
                    _all += item.Name + " (" + item.Id + ")\n";
                }

                await ReplyAsync(_all);
            }

            [Command("attachment data")]
            public async Task Test007()
            {
                string _all = "c: " + Context.Message.Attachments.Count + "\nl: <" + Util.File.ReturnLastAttachmentURL(Context) + ">\nf:";

                await Util.File.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "/downloadsuccess.data");

                await Context.Channel.SendFileAsync(Path.GetTempPath() + "/downloadsuccess.data", _all);
            }

            [Command("manipulation")]
            public async Task Test008()
            {
                try
                {
                    await Util.File.DownloadLastAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    await ReplyAsync("past download");
                    string type = Path.GetExtension(Util.File.ReturnLastAttachmentURL(Context));
                    await ReplyAsync("type retrieved");

                    await ReplyAsync("going into the `using (var img = SLImage.Load(Path.GetTempPath() + \"manip\" + type))` block");
                    using (var img = SLImage.Load(Path.GetTempPath() + "manip" + type))
                    {
                        img.Mutate(mut => mut.Invert());
                        await ReplyAsync("img manipulated");
                        img.Save(Path.GetTempPath() + "manip_new" + type);
                        await ReplyAsync("img save");
                    }

                    await ReplyAsync("begin send");
                    await Context.Channel.SendFileAsync(Path.GetTempPath() + "manip_new" + type);
                    await ReplyAsync("end send");
                }
                catch (Exception e)
                {
                    await Util.Error.BuildError(e, Context);
                }
            }

            [Command("channels")]
            public async Task Test009()
            {
                try
                {
                    IDMChannel ifDM = await Context.Message.Author.GetOrCreateDMChannelAsync();
                    ITextChannel DMtoTXT = ifDM as ITextChannel;
                    ITextChannel STtoTXT = Context.Channel as ITextChannel;

                    await ReplyAsync(ifDM.Id.ToString());
                    await ReplyAsync(Context.Channel.Id.ToString());
                }
                catch (Exception e)
                {
                    await Util.Error.BuildError(e, Context);
                }
            }

            [Command("nsfw")]
            public async Task Test010()
            {
                try
                {
                    await ReplyAsync((await Util.IsChannelNSFW(Context)).ToString());
                }
                catch (Exception e)
                {
                    await Util.Error.BuildError(e, Context);
                }
            }

            [Command("new error handling")]
            public async Task Test011()
            {
                await Util.Error.BuildError("you triggered the debug command\ncongratu-fucking-lations bitch", Context);
            }
        }

#endif

        public class SettingsComm : ModuleBase
        {
            [Group("settings"), Alias("set"), Summary("Modify some bot stuff. Most of it is restricted.")]
            public class Settings : ModuleBase
            {
                [Command("!"), Alias("kill"), Summary("Kills the bot."), RequireOwner]
                public async Task End()
                {
                    await ReplyAsync("Ending...");
                    Environment.Exit(0);
                }

                [Group("nsfw-commands"), Summary("The toggle for the defunct NSFW restrictor (Bot now uses channel's NSFW flag instead.)")]
                public class NSFWSet : ModuleBase
                {
                    [Command]
                    public async Task Get()
                    {
                        await ReplyAsync("Currently NSFW execution is: **" + Program.enableNSFW.ToString().ToLower() + "** *(true means NSFW commands are executable)*");
                    }

                    [Command("set"), RequireOwner]
                    public async Task Set(bool newval)
                    {
                        Program.enableNSFW = newval;
                        await ReplyAsync("NSFW execution is now: **" + newval.ToString().ToLower() + "**");
                    }
                }

                [Command("playing"), Alias("play", "game"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Playing(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, null, ActivityType.Playing);
                    await ReplyAsync("*Activity* has been set to: **" + new_play + "**");
                }

                [Command("watching"), Alias("watch"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Watching(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, null, ActivityType.Watching);
                    await ReplyAsync("*Activity* has been set to: **" + new_play + "**");
                }

                [Command("listening"), Alias("listen"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Listening(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, null, ActivityType.Listening);
                    await ReplyAsync("*Activity* has been set to: **" + new_play + "**");
                }

                [Command("streaming"), Alias("stream"), Summary("Sets the bot's activity."), RequireOwner]
                public async Task Streaming(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, "https://www.twitch.tv/xubiod_chat_bot", ActivityType.Streaming);
                    await ReplyAsync("*Activity* has been set to: **" + new_play + "**" +
                                    "\n*Status* has been set to: **streaming**");
                }

                [Command("status"), Alias("stat"), Summary("Sets the bot's status."), RequireOwner]
                public async Task Status(string new_play)
                {
                    switch (new_play.ToLower())
                    {
                        case "online":
                        case "on":
                            {
                                await Program.xuClient.SetStatusAsync(UserStatus.Online);
                                break;
                            }

                        case "invisible":
                        case "offline":
                            {
                                await Program.xuClient.SetStatusAsync(UserStatus.Invisible);
                                break;
                            }

                        case "afk":
                        case "away":
                            {
                                await Program.xuClient.SetStatusAsync(UserStatus.AFK);
                                break;
                            }

                        case "dnd":
                        case "silence":
                            {
                                await Program.xuClient.SetStatusAsync(UserStatus.DoNotDisturb);
                                break;
                            }

                        default:
                            {
                                await ReplyAsync("*Status* hasn't been set to: **" + new_play + "**, it's invalid.");
                                return;
                            }

                            await ReplyAsync("*Status* has been set to: **" + new_play + "**");
                    }
                }

                [Command("temp_prefix"), Alias("prefix"), Summary("Sets the prefix for current session."), RequireOwner]
                public async Task Prefix(string new_prefix)
                {
                    Program.prefix = new_prefix;
                    await ReplyAsync("*Prefix* has been set for this session to: **" + new_prefix + "**");
                }

                [Command("ping"), Alias("#", "latency"), Summary("Gets the latency from message recieved to reply.")]
                public async Task Ping()
                {
                    await ReplyAsync("*Ping latency* is currently at: **" + Program.xuClient.Latency + " milliseconds.**");
                }

                [Command("connection_state"), Alias("cs", "connect"), Summary("Gets the bot's connection state.")]
                public async Task CS()
                {
                    await ReplyAsync("*Connection state* is currently at: **" + Program.xuClient.ConnectionState + ".**");
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
            [Command("gen"), Summary("Makes a random integer with the number given as maximum.")]
            public async Task RndDefault(int max)
            {
                Random rnd = new Random();

                await ReplyAsync("Random number generated: **" + rnd.Next(max) + "**");
            }

            //thx dickcord

            [Command("existental_crisis"), Alias("ext_crisis", "ext_cri"), Summary("Give the bot an existential crisis.")]
            public async Task Crisis()
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"Do we exist?"); }
                else if (o == 1) { await ReplyAsync($"WHO AM I?!?!?!??!?!?!??!"); }
                else if (o == 2) { await ReplyAsync($"Does life matter?"); }
                else if (o == 3) { await ReplyAsync($"Is this real?"); }
                else if (o == 4) { await ReplyAsync($"... *sob*"); }
            }

            [Command("bake_cake"), Alias("make_cake"), Summary("Makes a cake.")]
            public async Task BakeCake()
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"Red velvet or green velvet?"); }
                else if (o == 1) { await ReplyAsync($"The last time it was burnt and raw, so hehhehehe!"); }
                else if (o == 2) { await ReplyAsync($"Not feelin' it right now."); }
                else if (o == 3) { await ReplyAsync($"nom nom nom nom nom"); }
                else if (o == 4) { await ReplyAsync($"Do you smell something burning?"); }
            }

            [Command("read_me_a_story"), Alias("rmas"), Summary("Reads a story.")]
            public async Task Story()
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"Jack and Jill went up a hill. Then they died."); }
                else if (o == 1) { await ReplyAsync($"A prince turned into a frog. Then an eagle eat him."); }
                else if (o == 2) { await ReplyAsync($"A planet was existing. Then it blew up."); }
                else if (o == 3) { await ReplyAsync($"No."); }
                else if (o == 4) { await ReplyAsync($"Read one yourself."); }
            }

            [Command("microbrew_some_local_kombucha"), Summary("Microbrews some local kombucha.")]
            public async Task MSLK()
            {
                await ReplyAsync($"WTF is kombucha anyway?");
            }

            [Command("record_a_mixtape"), Summary("Makes a mixtape.")]
            public async Task RAM()
            {
                await ReplyAsync($"Last time it blew up a star. So, no.");
            }

            [Command("paint_a_happy_little_tree"), Summary("Paints a happy little tree.")]
            public async Task PAHLT()
            {
                await ReplyAsync($"***You*** are a ***un***happy little accident.");
            }

            [Command("leetspeak"), Alias("1337"), Summary("Takes input and returns leetspeak.")]
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

            [Command("moarleetspeak"), Alias("moar1337"), Summary("Takes input and returns leetspeak. (more character subtitutions)")]
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

        [Group("base65536"), Summary("Encodes or decodes strings into Base65536.")]
        public class Base65536Comm : ModuleBase
        {
            [Command("encode"), Summary("Encodes a string into Base65536.")]
            public async Task Encode(string input)
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] bytes = encoding.GetBytes(input);

                await ReplyAsync(Base65536.Encode(bytes));
            }

            [Command("decode"), Summary("Decodes a string into Base65536.")]
            public async Task Decode(string input)
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                await ReplyAsync(encoding.GetString(Base65536.Decode(input)));
            }
        }

        [Group("trust"), Summary("Management for trust. Cannot be used by anyone except owner."), RequireOwner]
        public class Trust : ModuleBase
        {
            private XDocument xdoc;

            [Command("add"), Summary("Adds a user the the trusted list.")]
            public async Task Add(string user, string discrim)
            {
                try
                {
                    Discord.IUser add = Program.xuClient.GetUser(user, discrim);

                    bool exists = false;

                    xdoc = XDocument.Load("Trusted.xml");

                    var items = from i in xdoc.Descendants("trust")
                                select new
                                {
                                    user = i.Attribute("id")
                                };

                    foreach (var item in items)
                    {
                        if (item.user.Value == add.Id.ToString())
                        {
                            exists = true;
                        }
                    }

                    if (!exists)
                    {
                        Console.WriteLine("new user found to add to trust, doing that now");

                        XElement xelm = new XElement("trust");
                        XAttribute _user = new XAttribute("id", add.Id.ToString());

                        xelm.Add(_user);

                        xdoc.Root.Add(xelm);
                        xdoc.Save("Trusted.xml");

                        var pri = await Context.Message.Author.GetOrCreateDMChannelAsync();

                        await pri.SendMessageAsync("**" + add.Username + "#" + add.Discriminator + "** has been trusted.");
                    }
                    else
                    {
                        var pri = await Context.Message.Author.GetOrCreateDMChannelAsync();

                        await pri.SendMessageAsync("**" + add.Username + "#" + add.Discriminator + "** has already been trusted.");
                    }
                }
                catch (Exception exp)
                {
                    await Util.Error.BuildError(exp, Context);
                }
            }

            [Command("remove"), Summary("Revokes a user from the trusted list.")]
            public async Task Remove(string user, string discrim)
            {
                Discord.IUser remove = Program.xuClient.GetUser(user, discrim);

                XDocument xdoc = XDocument.Load("Trusted.xml");
                xdoc.Descendants("trust")
                    .Where(x => (string)x.Attribute("id") == remove.Id.ToString())
                    .Remove();

                xdoc.Save("Trusted.xml");

                var pri = await Context.Message.Author.GetOrCreateDMChannelAsync();

                await pri.SendMessageAsync("**" + remove.Username + "#" + remove.Discriminator + "** has been untrusted.");
            }

            [Command("add"), Summary("Adds a user to the trusted list.")]
            public async Task Add(ulong id)
            {
                Discord.IUser _add = Program.xuClient.GetUser(id);

                await Add(_add.Username, _add.Discriminator);

                /*try
                {
                    Discord.IUser add = Program.xuClient.GetUser(id);

                    bool exists = false;

                    xdoc = XDocument.Load("Trusted.xml");

                    var items = from i in xdoc.Descendants("trust")
                                select new
                                {
                                    user = i.Attribute("id")
                                };

                    foreach (var item in items)
                    {
                        if (item.user.Value == add.Id.ToString())
                        {
                            exists = true;
                        }
                    }

                    if (!exists)
                    {
                        Console.WriteLine("new user found to add to trust, doing that now");

                        XElement xelm = new XElement("trust");
                        XAttribute _user = new XAttribute("id", add.Id.ToString());

                        xelm.Add(_user);

                        xdoc.Root.Add(xelm);
                        xdoc.Save("Trusted.xml");

                        var pri = await Context.Message.Author.GetOrCreateDMChannelAsync();

                        await pri.SendMessageAsync("**" + add.Username + "#" + add.Discriminator + "** has been trusted.");
                    }
                    else
                    {
                        var pri = await Context.Message.Author.GetOrCreateDMChannelAsync();

                        await pri.SendMessageAsync("**" + add.Username + "#" + add.Discriminator + "** has already been trusted.");
                    }
                }
                catch (Exception exp)
                {
                    await GeneralTools.CommHandler.BuildError(exp, Context);
                }*/
            }

            [Command("remove"), Summary("Revokes a user from the trusted list.")]
            public async Task Remove(ulong id)
            {
                Discord.IUser remove = Program.xuClient.GetUser(id);

                XDocument xdoc = XDocument.Load("Trusted.xml");
                xdoc.Descendants("trust")
                    .Where(x => (string)x.Attribute("id") == remove.Id.ToString())
                    .Remove();

                xdoc.Save("Trusted.xml");

                var pri = await Context.Message.Author.GetOrCreateDMChannelAsync();

                await pri.SendMessageAsync("**" + remove.Username + "#" + remove.Discriminator + "** has been untrusted.");
            }
        }

        [Command("timezone", RunMode = RunMode.Async), Summary("Returns the timezone from a given string.")]
        public async Task Timezone(string loc)
        {
            try
            {
                var webClient = new WebClient();
                var webClient2 = new WebClient();
                string link = "https://www.amdoren.com/api/timezone.php?api_key=" + Program.keys.amdoren + "&loc=" + loc;
                string text_j = "";

                text_j = webClient.DownloadString(link);
                //text_j = text_j.Substring(1, text_j.Length - 2);

                //await ReplyAsync(text);
                dynamic keys = JObject.Parse(text_j);

                if (keys.error_message.ToString() != "-")
                {
                    EmbedBuilder embedd = new EmbedBuilder
                    {
                        Title = "Timezone Location",
                        Color = Discord.Color.Red,
                        Description = "Error!",

                        Footer = new EmbedFooterBuilder
                        {
                            Text = "The API requires free users to link to the API, so here it is:\n https://www.amdoren.com/time-zone-api/ \nxubot :p"
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "The API returned: ",
                                Value = "**" + keys.error_message.ToString() + "**",
                                IsInline = false
                            }
                        }
                    };

                    await ReplyAsync("", false, embedd.Build());
                }
                else
                {
                    EmbedBuilder embedd = new EmbedBuilder
                    {
                        Title = "Timezone Location",
                        Color = Discord.Color.Red,
                        Description = "Timezone and time for " + loc,

                        Footer = new EmbedFooterBuilder
                        {
                            Text = "The API requires free users to link to the API, so here it is:\n https://www.amdoren.com/time-zone-api/ \nxubot :p"
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Timezone: ",
                                Value = "**" + keys.timezone.ToString() + "**",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Current Time: ",
                                Value = "**" + keys.time.ToString() + "**",
                                IsInline = true
                            }
                        }
                    };

                    await ReplyAsync("", false, embedd.Build());
                }
                //string text = webClient.DownloadString(link);
                //text = text.Substring(1, text.Length - 2);
                //await ReplyAsync(text);
                //dynamic keys = JObject.Parse(text);

                //await ReplyAsync(keys.file_url.ToString());
            }
            catch (Exception exp)
            {
                await Util.Error.BuildError(exp, Context);
            }
        }
    }
}