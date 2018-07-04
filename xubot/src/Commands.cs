﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using xubot;
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
using static xubot.src.SpecialException;
using System.Net;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using xubot.src;
using System.Web;

using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Net.Http;
using System.Drawing;
using System.IO;
using IronOcr;
using System.Threading;
using Discord;
using System.IO.Compression;
using SixLabors.ImageSharp;
using System.Web;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

using SLImage = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing.Filters;

namespace xubot
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
        
        [Group("echo"), Alias("m"), Summary("a calculator, but shittier")]
        public class echo : ModuleBase
        {
            [Command]
            public async Task repeat_once(string blegh)
            {
                await ReplyAsync(blegh);
            }

            [Command("repeat"), Alias("r"), Summary("types what you give it."), RequireUserPermission(Discord.ChannelPermission.ManageMessages)]
            public async Task repeat(string blegh, int loop, string sep)
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

        [Group("math"), Alias("m"), Summary("a calculator, but shittier")]
        public class math : ModuleBase
        {
            [Command("add"), Alias("plus"), Summary("attempts to add two floats")]
            public async Task add([Summary("float 1")] float num1, [Summary("float 2")] float num2)
            {
                float result = num1 + num2;
                await ReplyAsync("The result is: " + result.ToString());
            }

            [Command("sub"), Alias("subtract"), Summary("attempts to subtract two floats")]
            public async Task sub([Summary("float 1")] float num1, [Summary("float 2")] float num2)
            {
                float result = num1 - num2;
                await ReplyAsync("The result is: " + result.ToString());
            }

            [Command("multi"), Alias("multiply"), Summary("attempts to multiply two floats")]
            public async Task multi([Summary("float 1")] float num1, [Summary("float 2")] float num2)
            {
                float result = num1 * num2;
                await ReplyAsync("The result is: " + result.ToString());
            }

            [Command("divide"), Alias("division"), Summary("attempts to divide two floats")]
            public async Task divide([Summary("float 1")] float num1, [Summary("float 2")] float num2)
            {
                float result = num1 / num2;
                await ReplyAsync("The result is: " + result.ToString());
            }

            [Command("mod"), Alias("modulo"), Summary("attempts to modulo two floats")]
            public async Task mod([Summary("float 1")] float num1, [Summary("float 2")] float num2)
            {
                float result = num1 % num2;
                await ReplyAsync("The result is: " + result.ToString());
            }

            [Command("pow"), Alias("power"), Summary("attempts to double power double")]
            public async Task pow([Summary("double 1")] double num1, [Summary("double 2")] double num2)
            {
                double result = Math.Pow(num1, num2);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The result is: " + result.ToString());
                }
                else
                {
                    await ReplyAsync("The result was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("sqrt"), Alias("squareroot"), Summary("attempts to sqrt a double")]
            public async Task sqrt([Summary("double 1")] double num1)
            {
                double result = Math.Sqrt(num1);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The result is: " + result.ToString());
                }
                else
                {
                    await ReplyAsync("The result was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("sin"), Alias("sine"), Summary("attempts to sine a double")]
            public async Task sin([Summary("double")] double num)
            {
                double result = Math.Sin(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The sine of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The sine was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("sinh"), Alias("sineh"), Summary("attempts to hyperbolic sine a double")]
            public async Task sinh([Summary("double")] double num)
            {
                double result = Math.Sinh(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The hyperbolic sine of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The hyperbolic sine was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("asin"), Alias("asine"), Summary("attempts to asine a double and returns an angle")]
            public async Task asin([Summary("double")] double num)
            {
                double result = Math.Asin(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The asine of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The asine was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("cos"), Alias("cosine"), Summary("attempts to cosine a double")]
            public async Task cos([Summary("double")] double num)
            {
                double result = Math.Cos(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The cosine of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The cosine was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("cosh"), Alias("cosineh"), Summary("attempts to hyperbolic cosine a double")]
            public async Task cosh([Summary("double")] double num)
            {
                double result = Math.Cosh(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The hyperbolic cosine of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The hyperbolic cosine was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("acos"), Alias("acosine"), Summary("attempts to acosine a double and returns an angle")]
            public async Task acos([Summary("double")] double num)
            {
                double result = Math.Acos(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The acosine of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The acosine was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("tan"), Alias("tangent"), Summary("attempts to tangent a double")]
            public async Task tan([Summary("double")] double num)
            {
                double result = Math.Sin(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The tangent of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The tanget was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("tanh"), Alias("tangenth"), Summary("attempts to hyperbolic tangent a double")]
            public async Task tanh([Summary("double")] double num)
            {
                double result = Math.Tanh(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The hyperbolic tangent of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The hyperbolic tangent was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("atan"), Alias("atangent"), Summary("attempts to atangent a double and returns an angle")]
            public async Task atan([Summary("double")] double num)
            {
                double result = Math.Atan(num);

                if (!Double.IsInfinity(result))
                {
                    await ReplyAsync("The atangent of " + num + " is: " + result.ToString() + ".");
                }
                else
                {
                    await ReplyAsync("The atangent was changed to Infinty or -Infinity. Please use smaller numbers.");
                }
            }

            [Command("quickeval"), Alias("eval", "quickdo", "do"), Summary("attempts to do shit without the other typing (ONLY INTEGERS)")]
            public async Task evalu([Summary("eval input")] string input)
            {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expression", string.Empty.GetType(), input);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                int result = int.Parse((string)row["expression"]);
                await ReplyAsync("The equation was evaluated and returned **" + result.ToString() + "**.");
            }
        }

        [Group("insult"), Summary("yay")]
        public class insult : ModuleBase
        {
            [Command("init"), Summary("inits the insults"), RequireOwner]
            public async Task init()
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

            [Command("list"), Summary("list array contents")]
            public async Task list()
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
            public class add : ModuleBase
            {
                [Command("v"), Summary("adds to the v list")]
                public async Task vit(String input)
                {
                    insult_v_index++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insult_v[insult_v_index] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }

                [Command("a"), Summary("adds to the a list")]
                public async Task adj(String input)
                {
                    insult_a_index++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insult_a[insult_a_index] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }

                [Command("n"), Summary("adds to the n list")]
                public async Task nou(String input)
                {
                    insult_n_index++;
                    if (!input.EndsWith(" ")) { input += " "; }
                    insult_n[insult_n_index] = input;

                    await ReplyAsync("Added " + '"' + input + '"' + ".");
                }
            }

            [Command("generate"), Summary("generate an insult")]
            public async Task gen()
            {
                Random rnd = new Random();
                int insult_v_use = rnd.Next(insult_v_index);
                int insult_a_use = rnd.Next(insult_a_index);
                int insult_n_use = rnd.Next(insult_n_index);

                await ReplyAsync(insult_v[insult_v_use] + insult_a[insult_a_use] + insult_n[insult_n_use]);
            }
        }

        [Group("convert"), Alias("c"), Summary("a calculator, but shittier")]
        public class convert : ModuleBase
        {
            [Command("temperature"), Alias("temp"), Summary("attempts to add two floats")]
            public async Task temp([Summary("double 1")] double num1, string fromto)
            {
                if (fromto == "c2f")
                {
                    await ReplyAsync("*Celsius to Fahrenheit:* " + ((num1 / 9) * 5 + 32).ToString());
                }
                else if (fromto == "f2c")
                {
                    await ReplyAsync("*Fahrenheit to Celsius:* " + ((num1 - 32) * (9 / 5)).ToString());
                }
            }

            [Command("length"), Alias("height"), Summary("attempts to add two floats")]
            public async Task leng([Summary("double 1")] double num1, string fromto)
            {
                if (fromto == "ft2m")
                {
                    await ReplyAsync("*Feet to Meters:* " + (num1 * 0.3048).ToString());
                }
                else if (fromto == "m2ft")
                {
                    await ReplyAsync("*Meters to Feet:* " + (num1 / 0.3048).ToString());
                }
            }
        }

        [Group("pattern"), Alias("pat"), Summary("a calculator, but shittier")]
        public class pat : ModuleBase
        {
            [Command("generate")]
            public async Task generate(string emo1, string emo2)
            {
                string pattern1_;
                string pattern2_;
                string pattern3_;
                string pattern4_;
                string pattern5_;

                pattern1_ = pattern1.Replace("0", emo1).Replace("1", emo2);
                pattern2_ = pattern2.Replace("0", emo1).Replace("1", emo2);
                pattern3_ = pattern3.Replace("0", emo1).Replace("1", emo2);
                pattern4_ = pattern4.Replace("0", emo1).Replace("1", emo2);
                pattern5_ = pattern5.Replace("0", emo1).Replace("1", emo2);

                await ReplyAsync(pattern1_ + '\n' + pattern2_ + '\n' + pattern3_ + '\n' + pattern4_ + '\n' + pattern5_);
            }

            [Command("set")]
            public async Task set(string pat1, string pat2, string pat3, string pat4, string pat5)
            {
                if ((pat1.Contains("0") || pat1.Contains("1") ||
                    pat2.Contains("0") || pat2.Contains("1") ||
                    pat3.Contains("0") || pat3.Contains("1") ||
                    pat4.Contains("0") || pat4.Contains("1") ||
                    pat5.Contains("0") || pat5.Contains("1")) &&
                    pat1.Length == 5 && pat2.Length == 5 &&
                    pat3.Length == 5 && pat4.Length == 5 &&
                    pat5.Length == 5
                    )
                {
                    pattern1 = pat1;
                    pattern2 = pat2;
                    pattern3 = pat3;
                    pattern4 = pat4;
                    pattern5 = pat5;

                    await ReplyAsync("Pattern(s) set.");
                }
                else
                {
                    await ReplyAsync("One of the patterns given has an invalid character, or it's too long, or it's too short. Use **0**s and **1**s and the patterns must be exactly **5** characters long.");
                }
            }

            [Command("generate-preset")]
            public async Task generate_preset(string searchqueue, string emo1, string emo2)
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
        public class post2media : ModuleBase
        {
            [Command]
            public async Task default_()
            {
                await ReplyAsync("You didn't give me a service... ;<");
            }

            [Command("reddit"), Alias("r", "redd"), Summary("attempts to post a thing to reddit")]
            public async Task redditfun(string title, string content){
                if (Program.botf_reddit == false) {
                    await ReplyAsync("Reddit token not provided by bot runner.");
                }
                else {
                    Program.subreddit = await Program.reddit.GetSubredditAsync("/r/xubot_subreddit");

                    string result_ = content;
                    var redditPost = await Program.subreddit.SubmitTextPostAsync(title, Context.Message.Author.Username + "#" + Context.Message.Author.Discriminator + " on the Discord server " + Context.Guild.Name + " posted:\n\n" + result_);
                    await ReplyAsync("<" + redditPost.Url.AbsoluteUri.ToString() + ">");
                    }

            }

            [Command("twitter"), Alias("t", "twit"), Summary("attempts to post a thing to twitter")]
            public async Task tweet(string content)
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

        [Command("discord-api-link-gen"), Alias("discord-bot", "db"), Summary("attempts to post a thing to twitter")]
        public async Task tweet(string id)
        {
            await ReplyAsync("https://discordapp.com/api/oauth2/authorize?client_id=" + id + "&scope=bot&permissions=0");
        }

        [Group("debug"), RequireOwner]
        public class debug : ModuleBase
        {
            [Command("return_attachs")]
            public async Task returnAttachs()
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
            public async Task test001()
            {
                var stuff = Context.Message.Source;

                await ReplyAsync(stuff.ToString(), false);
            }

            [Command("return_type")]
            public async Task test002()
            {
                var stuff = Context.Message.Type;

                await ReplyAsync(stuff.ToString(), false);
            }

            [Command("get_mood")]
            public async Task test003(ulong id)
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
            public async Task test004(int id)
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
                        default: await ReplyAsync("invaild id"); break;
                    }
                } catch (Exception exp)
                {
                    await GeneralTools.CommHandler.BuildError(exp, Context);
                }
            }

            [Command("ct")]
            public async Task test005()
            {
                await ReplyAsync(Discord.Color.LightOrange.ToString());
            }

            [Command("li")]
            public async Task test006()
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
            public async Task test007()
            {
                string _all = "c: " + Context.Message.Attachments.Count + "\nl: <" + GeneralTools.ReturnAttachmentURL(Context) + ">\nf:";

                await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "/downloadsuccess.data");

                await Context.Channel.SendFileAsync(Path.GetTempPath() + "/downloadsuccess.data", _all);
            }

            [Command("manipulation")]
            public async Task test008()
            {
                try
                {
                    await GeneralTools.DownloadAttachmentAsync(Context, Path.GetTempPath() + "manip", true);
                    await ReplyAsync("past download");
                    string type = VirtualPathUtility.GetExtension(GeneralTools.ReturnAttachmentURL(Context));
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
                catch (Exception e) { await GeneralTools.CommHandler.BuildError(e, Context); }
            }
        }

        [Group("info")]
        public class info : ModuleBase
        {
            [Command("server"), Alias("server-info", "si"), Summary("attempts to post a thing to twitter")]
            public async Task serverinfo()
            {
                string verifyLvl = Context.Guild.VerificationLevel.ToString();
                string afkchannelid = Context.Guild.AFKChannelId.ToString();

                if (afkchannelid == "") { afkchannelid = "No AFK Channel"; }

                if (verifyLvl == "None") { verifyLvl = "None ._."; }
                else if (verifyLvl == "Low") { verifyLvl = "Low >_>"; }
                else if (verifyLvl == "Medium") { verifyLvl = "Medium o_o"; }
                else if (verifyLvl == "High") { verifyLvl = "(╯°□°）╯︵ ┻━┻ (High)"; }
                else if (verifyLvl == "Very High") { verifyLvl = "┻━┻ミヽ(ಠ益ಠ)ﾉ彡 ┻━┻ (Very High)"; }
                else { verifyLvl = "Unconclusive"; }

                IGuildChannel welcomeChannel = await Context.Guild.GetChannelAsync(Context.Guild.SystemChannelId ?? 0);

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Information about: " + Context.Guild.Name,
                    Color = Discord.Color.Red,
                    Description = "Server information details",
                    ThumbnailUrl = Context.Guild.IconUrl,

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "ID",
                                Value = Context.Guild.Id,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Name",
                                Value = Context.Guild.Name,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Owner",
                                Value = (await Context.Guild.GetUserAsync(Context.Guild.OwnerId)).Username + "#" + (await Context.Guild.GetUserAsync(Context.Guild.OwnerId)).Discriminator,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Verification Level",
                                Value = verifyLvl,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "AFK Timeout",
                                Value = Context.Guild.AFKTimeout.ToString() + " seconds",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "AFK Channel ID",
                                Value = afkchannelid,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Welcomes go to this channel:",
                                Value = welcomeChannel.Name,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Created",
                                Value = Context.Guild.CreatedAt,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Default MSG Notifications",
                                Value = Context.Guild.DefaultMessageNotifications,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Amount of Roles",
                                Value = Context.Guild.Roles.Count,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Amount of Custom Emoji",
                                Value = Context.Guild.Emotes.Count,
                                IsInline = true
                            }
                        }
                };
                await ReplyAsync("", false, embedd.Build());
            }

            [Command("channel"), Alias("channel-info", "ci"), Summary("attempts to post a thing to twitter")]
            public async Task channelinfo()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Information about: " + Context.Channel.Name,
                    Color = Discord.Color.Red,
                    Description = "Channel information details",
                    ThumbnailUrl = Context.Guild.IconUrl,

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "ID",
                                Value = Context.Channel.Id,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Name",
                                Value = Context.Channel.Name,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Created on",
                                Value = Context.Channel.CreatedAt,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Amount of Pinned Messages",
                                Value = (await Context.Channel.GetPinnedMessagesAsync()).Count.ToString() + "/50",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "NSFW?",
                                Value = GeneralTools.ChannelNSFW(Context),
                                IsInline = true
                            }
                        }
                };
                await ReplyAsync("", false, embedd.Build());
            }
            
            [Command("user", RunMode = RunMode.Async), Alias("user-info", "ui"), Summary("attempts to post a thing to twitter")]
            public async Task user(ulong id = 0)
            {
                try
                {
                    //throw new SpecialException.IHaveNoFuckingIdeaException();

                    Discord.IUser _user0 = Context.Message.Author;
                    IGuildUser _user1 = await Context.Guild.GetUserAsync(_user0.Id);

                    if (id == 0)
                    {
                        _user0 = Context.Message.Author;
                        _user1 = await Context.Guild.GetUserAsync(_user0.Id);
                    }
                    else
                    {
                        _user0 = Program.xuClient.GetUser(id);
                        _user1 = await Context.Guild.GetUserAsync(_user0.Id);
                    }

                    string _role_list = "";

                    foreach (var role in _user1.RoleIds)
                    {
                        var _role = Context.Guild.GetRole(role);

                        _role_list += _role.Mention + " ";
                    }

                    string act = "";

                    if (_user0.Activity == null)
                    {
                        act = "Nothing.";
                    } else
                    {
                        act = _user0.Activity.Type + " " + _user0.Activity.Name;
                    }

                    EmbedBuilder embedd = new EmbedBuilder
                    {
                        Title = "Information about: " + _user0,
                        Color = Discord.Color.Red,
                        Description = "User information details",
                        ThumbnailUrl = _user0.GetAvatarUrl(),

                        Footer = new EmbedFooterBuilder
                        {
                            Text = "xubot :p"
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "ID",
                                Value = _user0.Id,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Status",
                                Value = _user0.Status,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Bot?",
                                Value = _user0.IsBot,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Webhook?",
                                Value = _user0.IsWebhook,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Current Activity",
                                Value = act,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Deafened?",
                                Value = _user1.IsDeafened,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Self Deafened?",
                                Value = _user1.IsSelfDeafened,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Muted?",
                                Value = _user1.IsMuted,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Self Muted?",
                                Value = _user1.IsSelfMuted,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Joined server on",
                                Value = _user1.JoinedAt,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Nickname",
                                Value = _user1.Nickname,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Has " + (_user1.RoleIds.Count) + "roles:",
                                Value = _role_list,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Created on",
                                Value = _user0.CreatedAt,
                                IsInline = true
                            }
                        }
                    };
                    await ReplyAsync("", false, embedd.Build());
                } catch (Exception e)
                {
                    await GeneralTools.CommHandler.BuildError(e, Context);
                }
            }

        }

        public class settings_comm : ModuleBase
        {
            [Group("settings"), Alias("set"), Summary("bot setting tweaks")]
            public class settings : ModuleBase
            {
                [Command("!"), Alias("kill"), Summary("attempts to kill the bot"), RequireOwner]
                public async Task end()
                {
                    await ReplyAsync("Ending...");
                    Environment.Exit(0);
                }

                [Group("nsfw-commands")]
                public class nsfwSet : ModuleBase
                {
                    [Command]
                    public async Task get()
                    {
                        await ReplyAsync("Currently NSFW execution is: **" + Program.enableNSFW.ToString().ToLower() + "** *(true means NSFW commands are executable)*");
                    }

                    [Command("set"), RequireOwner]
                    public async Task set(bool newval)
                    {
                        Program.enableNSFW = newval;
                        await ReplyAsync("NSFW execution is now: **" + newval.ToString().ToLower() + "**");
                    }
                }

                [Command("playing"), Alias("play", "game"), Summary("attempts to set playing"), RequireOwner]
                public async Task play(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, null, ActivityType.Playing);
                    await ReplyAsync("*Game* has been set to: **" + new_play + "**");
                }

                [Command("watching"), Alias("watch"), Summary("attempts to set playing"), RequireOwner]
                public async Task watching(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, null, ActivityType.Watching);
                    await ReplyAsync("*Game* has been set to: **" + new_play + "**");
                }

                [Command("listening"), Alias("listen"), Summary("attempts to set playing"), RequireOwner]
                public async Task listening(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, null, ActivityType.Listening);
                    await ReplyAsync("*Game* has been set to: **" + new_play + "**");
                }

                [Command("streaming"), Alias("stream"), Summary("attempts to set streaming"), RequireOwner]
                public async Task stream(string new_play)
                {
                    await Program.xuClient.SetGameAsync(new_play, "https://www.twitch.tv/xubiod_chat_bot", ActivityType.Streaming);
                    await ReplyAsync("*Game* has been set to: **" + new_play + "**" +
                                    "\n*Status* has been set to: **streaming**");
                }

                [Command("status"), Alias("stat"), Summary("attempts to set playing"), RequireOwner]
                public async Task stat(string new_play)
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

                [Command("temp_prefix"), Alias("prefix"), Summary("attempts to set prefix for current session"), RequireOwner]
                public async Task prefix(string new_prefix)
                {
                    Program.prefix = new_prefix;
                    await ReplyAsync("*Prefix* has been set for this session to: **" + new_prefix + "**");
                }

                [Command("ping"), Alias("#", "latency"), Summary("attempts to set playing")]
                public async Task ping()
                {
                    await ReplyAsync("*Ping latency* is currently at: **" + Program.xuClient.Latency + " milliseconds.**");
                }

                [Command("connection_state"), Alias("cs", "connect"), Summary("attempts to set playing")]
                public async Task cs()
                {
                    await ReplyAsync("*Connection state* is currently at: **" + Program.xuClient.ConnectionState + ".**");
                }
            }
        }

        public class random : ModuleBase
        {
            /// <summary>
            /// this is just a split
            /// 
            /// jokes below
            /// </summary>
            /// 

            [Command("gen")]
            public async Task rnd_default(int max)
            {
                Random rnd = new Random();

                await ReplyAsync("Random number generated: **" + rnd.Next(max) + "**");
            }

            //thx dickcord

            [Command("existental_crisis"), Alias("ext_crisis", "ext_cri"), Summary("gets a existential crisis")]
            public async Task crisis()
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"Do we exist?"); }
                else if (o == 1) { await ReplyAsync($"WHO AM I?!?!?!??!?!?!??!"); }
                else if (o == 2) { await ReplyAsync($"Does life matter?"); }
                else if (o == 3) { await ReplyAsync($"Is this real?"); }
                else if (o == 4) { await ReplyAsync($"... *sob*"); }
            }

            [Command("bake_cake"), Alias("make_cake"), Summary("attempts to make a cake")]
            public async Task bakecake()
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"Red velvet or green velvet?"); }
                else if (o == 1) { await ReplyAsync($"The last time it was burnt and raw, so hehhehehe!"); }
                else if (o == 2) { await ReplyAsync($"Not feelin' it right now."); }
                else if (o == 3) { await ReplyAsync($"nom nom nom nom nom"); }
                else if (o == 4) { await ReplyAsync($"Do you smell something burning?"); }
            }

            [Command("read_me_a_story"), Alias("rmas"), Summary("attempts to read a story")]
            public async Task story()
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"Jack and Jill went up a hill. Then they died."); }
                else if (o == 1) { await ReplyAsync($"A prince turned into a frog. Then an eagle eat him."); }
                else if (o == 2) { await ReplyAsync($"A planet was existing. Then it blew up."); }
                else if (o == 3) { await ReplyAsync($"No."); }
                else if (o == 4) { await ReplyAsync($"Read one yourself."); }
            }

            [Command("microbrew_some_local_kombucha"), Summary("attempts to do... that.")]
            public async Task mslk()
            {
                await ReplyAsync($"WTF is kombucha anyway?");
            }

            [Command("record_a_mixtape"), Summary("attempts to do... that.")]
            public async Task ram()
            {
                await ReplyAsync($"Last time it blew up a star. So, no.");
            }

            [Command("paint_a_happy_little_tree"), Summary("attempts to do... that.")]
            public async Task pahlt()
            {
                await ReplyAsync($"***You*** are a ***un***happy little accident.");
            }

            [Command("leetspeak"), Alias("1337"), Summary("why.")]
            public async Task leet(string input)
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

            [Command("moarleetspeak"), Alias("moar1337"), Summary("why.")]
            public async Task more(string input)
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
        
        [Group("base65536")]
        public class base65536_comm : ModuleBase
        {
            [Command("encode")]
            public async Task encode(string input)
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] bytes = encoding.GetBytes(input);

                await ReplyAsync(Base65536.Encode(bytes));
            }

            [Command("decode")]
            public async Task decode(string input)
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                await ReplyAsync(encoding.GetString(Base65536.Decode(input)));
            }
        }
        
        [Command("about")]
        public async Task about()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "About Xubot",
                Color = Discord.Color.Orange,
                Description = "Version " + ThisAssembly.Git.BaseTag,
                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "APIs",
                                Value = String.Join("", Program.apiJson.apis),
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Command("credits")]
        public async Task credits()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Xubot Development Credits",
                Color = Discord.Color.Orange,
                Description = "Version " + ThisAssembly.Git.BaseTag,

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Credits",
                                Value = String.Join("", Program.apiJson.credits),
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Command("version")]
        public async Task versionCMD()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Xubot Version",
                Color = Discord.Color.Orange,
                Description = "The specifics.",

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Version",
                                Value = ThisAssembly.Git.BaseTag,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Specific Build",
                                Value = ThisAssembly.Git.Tag,
                                IsInline = true
                            },
                            //https://github.com/xubot-team/xubot/commit/2064085bc0fd33a591036f67b686d0366d1591c5
                            new EmbedFieldBuilder
                            {
                                Name = "Build Commit",
                                Value = ThisAssembly.Git.Commit,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Link to Latest Change",
                                Value = "https://github.com/xubot-team/xubot/commit/" + ThisAssembly.Git.Sha,
                                IsInline = true
                            }

                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Group("trust"), RequireOwner]
        public class Trust : ModuleBase
        {
            XDocument xdoc;

            [Command("add")]
            public async Task add(string user, string discrim)
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

            [Command("remove")]
            public async Task remove(string user, string discrim)
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

            [Command("add")]
            public async Task add(ulong id)
            {
                try
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
                }
            }

            [Command("remove")]
            public async Task remove(ulong id)
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

        [Command("donate")]
        public async Task donate()
        {
            await ReplyAsync("To donate to the creator of this bot, please visit:\n" + Program.keys.donate_link);
        }

        [Command("timezone", RunMode = RunMode.Async)]
        public async Task timezone(string loc)
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

        [Command("privacy-policy")]
        public async Task pp()
        {
            File.WriteAllText(Path.GetTempPath() + "pripol.txt", Properties.Resources.PrivacyPolicy);
            await Context.Channel.SendFileAsync(Path.GetTempPath() + "pripol.txt");
        }
    }
}