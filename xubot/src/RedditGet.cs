using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Strike;
using Strike.IE;

using static xubot.RedditGetFunct.ParseSorting;
using static xubot.src.SpecialException;

namespace xubot
{
    public class RedditGet : ModuleBase
    {
        static string previous_sub = "notinteresting";
        static string previous_query = "";
        static int previous_sorting = 0;
        static bool previous_hide = false;

        [Command("reddit?last", RunMode = RunMode.Async), Alias("reddit?l")]
        public async Task last()
        {
            await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
        }

        [Command("reddit?nsfwmap"), RequireNsfw]
        public async Task map()
        {
            await ReplyAsync("Alright... then... " + "http://electronsoup.net/nsfw_subreddits/#");
        }

        [Command("reddit?random", RunMode = RunMode.Async), Alias("reddit?r")]
        public async Task rnd()
        {
            Random rnd = new Random();
            switch (rnd.Next(0, 22))
            {
                case 0: { previous_sub = "adviceanimals"; break; }
                case 1: { previous_sub = "askreddit"; break; }
                case 2: { previous_sub = "aww"; break; }
                case 3: { previous_sub = "bestof"; break; }
                case 4: { previous_sub = "books"; break; }
                case 5: { previous_sub = "earthporn"; break; }
                case 6: { previous_sub = "explainlikeimfive"; break; }
                case 7: { previous_sub = "funny"; break; }
                case 8: { previous_sub = "gaming"; break; }
                case 9: { previous_sub = "gifs"; break; }
                case 10: { previous_sub = "iama"; break; }
                case 11: { previous_sub = "movies"; break; }
                case 12: { previous_sub = "music"; break; }
                case 13: { previous_sub = "news"; break; }
                case 14: { previous_sub = "pics"; break; }
                case 15: { previous_sub = "science"; break; }
                case 16: { previous_sub = "technology"; break; }
                case 17: { previous_sub = "television"; break; }
                case 18: { previous_sub = "todayilearned"; break; }
                case 19: { previous_sub = "videos"; break; }
                case 20: { previous_sub = "worldnews"; break; }
                case 21: { previous_sub = "wtf"; break; }
            }

            previous_query = "";
            previous_sorting = 0;
            previous_hide = false;

            await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
        }

        [Command("reddit?sub", RunMode = RunMode.Async)]
        public async Task subreddit(string input)
        {
            Program.subreddit = await Program.reddit.GetSubredditAsync(input);

            string display = Program.subreddit.DisplayName;
            string fullname = Program.subreddit.FullName;
            string NSFW = Program.subreddit.NSFW.ToString();
            string sub = string.Format("{0:#,###0}", Program.subreddit.Subscribers);
            string image = Program.subreddit.HeaderImage;
            string desc = Program.subreddit.Description.Split('\n')[0];

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Subreddit: " + input,
                Color = Discord.Color.Orange,
                Description = "",
                ThumbnailUrl = image,

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "First line of Description",
                                Value = desc,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Subscriber Count",
                                Value = sub,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Internal Name",
                                Value = fullname,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "NSFW?",
                                Value = NSFW,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Link",
                                Value = "https://reddit.com/r/"+input,
                                IsInline = true
                            }
                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Command("reddit?wiki", RunMode = RunMode.Async)]
        public async Task wiki(string input)
        {
            Program.subreddit = await Program.reddit.GetSubredditAsync(input);
            
            string image = Program.subreddit.HeaderImage;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Subreddit: " + input,
                Color = Discord.Color.Orange,
                Description = "",
                ThumbnailUrl = image,

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Wiki",
                                Value = Program.subreddit.GetWiki.GetPageNamesAsync(),
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = "";
            previous_hide = false;

            await Operate(Context, subreddit, "", 0, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit, string query)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = query;
            previous_hide = false;

            await Operate(Context, subreddit, query, 0, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit, string query, int sorting)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = query;
            previous_hide = false;

            await Operate(Context, subreddit, query, sorting, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit, int sorting)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = "";
            previous_hide = false;

            await Operate(Context, subreddit, "", sorting, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = "";
            previous_hide = hide;

            await Operate(Context, subreddit, "", 0, hide);
        }
        
        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit, string query, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = query;
            previous_hide = false;

            await Operate(Context, subreddit, query, 0, hide);
        }
        
        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit, string query, int sorting, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = query;
            previous_hide = hide;

            await Operate(Context, subreddit, query, sorting, hide);
        }
        
        [Command("reddit", RunMode = RunMode.Async)]
        public async Task reddit_pic(string subreddit, int sorting, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = "";
            previous_hide = hide;

            await Operate(Context, subreddit, "", sorting, hide);
        }
        
        /* operation functions */

        //List<RedditSharp.Things.Post> contents_list = new List<RedditSharp.Things.Post> { };

        public async Task Operate(ICommandContext Context, string subreddit, string query, int sorting, bool hide)
        {
            try
            {
                //throw new ItsFuckingBrokenException(message: "Throw 'IHaveNoFuckingIdeaException' because I have no fucking idea.", inner: new IHaveNoFuckingIdeaException());

                Program.subreddit = await Program.reddit.GetSubredditAsync(subreddit);
                var msg = await ReplyAsync("Subreddit: **" + subreddit + "**\nPlease wait, this takes a while with broad terms and popular subreddits!");

                await Context.Channel.TriggerTypingAsync();
                Random rnd = new Random();

                var contents = await Program.subreddit.GetPosts(FromIntSort(sorting), -1).ToList();
                if (contents.Count < 10)
                {
                    contents = await Program.subreddit.GetPosts(-1).ToList();
                }
                //Console.WriteLine(contents.Count);
                var post = contents.ElementAt(rnd.Next(contents.Count));
                //EmbedBuilder embedd;

                bool isNSFW = await GeneralTools.ChannelNSFW(Context);

                //await ReplyAsync((isNSFW && (post.NSFW || post.Title.Contains("NSFW") || post.Title.Contains("NSFL"))).ToString());

                if (post.NSFW || post.Title.Contains("NSFW") || post.Title.Contains("NSFL"))
                {
                    if (!isNSFW)
                    {
                        await msg.DeleteAsync();
                        await ReplyAsync("The random post that was selected is NSFW or the subreddit is NSFW. Try again for another random post, with another subreddit, or move to a NSFW channel (needs nsfw in the name).");
                        return;
                    }
                    else
                    {
                        await msg.DeleteAsync();
                        await ReplyAsync("**" + post.Title + "**\n" + "Posted on *" + post.CreatedUTC.ToShortDateString() + "* by **" + post.AuthorName + "**" + "\n\n" + ReturnCharOnTrue(hide, "<") + post.Url.AbsoluteUri + ReturnCharOnTrue(hide, ">") + "\n<https://www.reddit.com" + post.Permalink.ToString() + ">");
                    }
                }
                else
                {
                    await msg.DeleteAsync();
                    //await ReplyAsync("https://reddit.com" + post.Permalink.ToString());

                    await ReplyAsync("**" + post.Title + "**\n" + "Posted on *" + post.CreatedUTC.ToShortDateString() + "* by **" + post.AuthorName + "**" + "\n\n" + ReturnCharOnTrue(hide, "<") + post.Url.AbsoluteUri + ReturnCharOnTrue(hide, ">") + "\n<https://www.reddit.com" + post.Permalink.ToString() + ">");
                }
            }
            catch (Exception e)
            {
                await GeneralTools.CommHandler.BuildError(e, Context);
            }
        }

        public string ReturnCharOnTrue(bool hide, string input)
        {
            if (hide) { return input; }
            else { return null; }
        }
    }
}
