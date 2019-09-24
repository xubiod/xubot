using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using xubot_core;
using Tweetinvi;
using System.Xml;

//unused atm
using System.IO;
using System.Net.Sockets;
using System.Drawing;
using System.Net.Http;
using RedditSharp;
using System.Xml.Linq;
using System.Linq;
using Tweetinvi.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using xubot_core.src;
using System.Web;

using System.Threading;
using System.IO.Compression;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

using SLImage = SixLabors.ImageSharp.Image;

using static xubot_core.src.SpecialException;

namespace xubot_core.src
{
    public class Commands : ModuleBase
    {
        public static string[] insult_v = new string[128];
        public static int insult_v_index = 0;
        public static string[] insult_a = new string[128];
        public static int insult_a_index = 0;
        public static string[] insult_n = new string[128];
        public static int insult_n_index = 0;

        public static string pattern1 = "01110";
        public static string pattern2 = "11011";
        public static string pattern3 = "10001";
        public static string pattern4 = "11011";
        public static string pattern5 = "01110";

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
                Array.Clear(insult_v, 0, 128);
                insult_v[0] = "You are a ";
                insult_v[1] = "Your face is a ";
                insult_v[2] = "Your body is a ";
                insult_v[3] = "Your code is a ";
                insult_v_index = 3;

                Array.Clear(insult_a, 0, 128);
                insult_a[0] = "steaming piece of ";
                insult_a[1] = "smelly ";
                insult_a[2] = "small ";
                insult_a[3] = "fat ";
                insult_a_index = 3;

                Array.Clear(insult_n, 0, 128);
                insult_n[0] = "rotting poo.";
                insult_n[1] = "dick. ";
                insult_n[2] = "spoiled oatmeal.";
                insult_n[3] = "arsehole.";
                insult_n_index = 3;

                await ReplyAsync("Reset the insult arrays.");
            }

            [Command("list"), Summary("Displays the insult arrays' contents.")]
            public async Task List()
            {
                string _v = "**V**(itim): `[";
                for (int i = 0; i < insult_v_index; i++)
                {
                    _v += insult_v[i];

                    if (i != 128)
                    {
                        _v += "] [";
                    }
                }
                _v += "]`";

                string _a = "**A**(djective): `[";
                for (int i = 0; i < insult_a_index; i++)
                {
                    _a += insult_a[i];

                    if (i != 128)
                    {
                        _a += "] [";
                    }
                }
                _a += "]`";

                string _n = "**N**(oun): `[";
                for (int i = 0; i < insult_n_index; i++)
                {
                    _n += insult_n[i];

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
                    insult_v_index++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insult_v[insult_v_index] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }

                [Command("a"), Summary("Adds a string to the 'adjective' list.")]
                public async Task Adj(String input)
                {
                    insult_a_index++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insult_a[insult_a_index] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }

                [Command("n"), Summary("Adds a string to the 'noun' list.")]
                public async Task Nou(String input)
                {
                    insult_n_index++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insult_n[insult_n_index] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }
            }

            [Command("generate"), Summary("Generates an insult.")]
            public async Task Gen()
            {
                Random rnd = new Random();
                int insult_v_use = rnd.Next(insult_v_index);
                int insult_a_use = rnd.Next(insult_a_index);
                int insult_n_use = rnd.Next(insult_n_index);

                await ReplyAsync(insult_v[insult_v_use] + insult_a[insult_a_use] + insult_n[insult_n_use]);
            }
        }

        [Group("pattern"), Alias("pat"), Summary("Makes a pattern with given characters.")]
        public class Pattern : ModuleBase
        {
            [Command("generate"), Summary("Generates a premade pattern using a search term.")]
            public async Task GeneratePreset(string searchqueue, string emo1, string emo2)
            {
                string pattern1_ = Pattern_Presets.Return_Query(searchqueue, 1).Replace("0", emo1).Replace("1", emo2);
                string pattern2_ = Pattern_Presets.Return_Query(searchqueue, 2).Replace("0", emo1).Replace("1", emo2);
                string pattern3_ = Pattern_Presets.Return_Query(searchqueue, 3).Replace("0", emo1).Replace("1", emo2);
                string pattern4_ = Pattern_Presets.Return_Query(searchqueue, 4).Replace("0", emo1).Replace("1", emo2);
                string pattern5_ = Pattern_Presets.Return_Query(searchqueue, 5).Replace("0", emo1).Replace("1", emo2);

                await ReplyAsync(pattern1_ + '\n' + pattern2_ + '\n' + pattern3_ + '\n' + pattern4_ + '\n' + pattern5_);
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
                if (Program.botf_reddit == false)
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
                    await GeneralTools.CommHandler.BuildError(exp, Context);
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
                string _all = "c: " + Context.Message.Attachments.Count + "\nl: <" + GeneralTools.ReturnAttachmentURL(Context) + ">\nf:";

                await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "/downloadsuccess.data");

                await Context.Channel.SendFileAsync(Path.GetTempPath() + "/downloadsuccess.data", _all);
            }

            [Command("manipulation")]
            public async Task Test008()
            {
                try
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    await ReplyAsync("past download");
                    string type = Path.GetExtension(GeneralTools.ReturnAttachmentURL(Context));
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
                    await GeneralTools.CommHandler.BuildError(e, Context);
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
                    await GeneralTools.CommHandler.BuildError(e, Context);
                }
            }

            [Command("nsfw")]
            public async Task Test010()
            {
                try
                {
                    await ReplyAsync((await GeneralTools.ChannelNSFW(Context)).ToString());
                }
                catch (Exception e)
                {
                    await GeneralTools.CommHandler.BuildError(e, Context);
                }
            }

            [Command("new error handling")]
            public async Task Test011()
            {
                await GeneralTools.CommHandler.BuildError("you triggered the debug command\ncongratu-fucking-lations bitch", Context);
            }
        }

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
                    if (new_play.ToLower() == "online" || new_play.ToLower() == "on")
                    {
                        await Program.xuClient.SetStatusAsync(UserStatus.Online);
                        await ReplyAsync("*Status* has been set to: **" + new_play + "**");
                    }
                    else if (new_play.ToLower() == "invisible")
                    {
                        await Program.xuClient.SetStatusAsync(UserStatus.Invisible);
                        await ReplyAsync("*Status* has been set to: **" + new_play + "**");
                    }
                    else if (new_play.ToLower() == "afk" || new_play.ToLower() == "away")
                    {
                        await Program.xuClient.SetStatusAsync(UserStatus.AFK);
                        await ReplyAsync("*Status* has been set to: **" + new_play + "**");
                    }
                    else if (new_play.ToLower() == "dnd" || new_play.ToLower() == "no_notif")
                    {
                        await Program.xuClient.SetStatusAsync(UserStatus.DoNotDisturb);
                        await ReplyAsync("*Status* has been set to: **" + new_play + "**");
                    }
                    else
                    {
                        await ReplyAsync("*Status* hasn't been set to: **" + new_play + "**, it's invalid.");
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
            XDocument xdoc;

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
                    await GeneralTools.CommHandler.BuildError(exp, Context);
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
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }
    }
}
