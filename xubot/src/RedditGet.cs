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

        static string[] randomSubs = { "adviceanimals", "askreddit", "aww", "bestof", "books", "earthporn", "explainlikeimfive", "funny", "gaming", "gifs", "iama", "movies", "music",
                                       "news", "pics", "science", "technology", "television", "todayilearned", "videos", "worldnews", "wtf" };

        [Command("reddit?last", RunMode = RunMode.Async), Alias("reddit?l"), Summary("Gets a post from the last subreddit entered.")]
        public async Task last()
        {
            await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
        }

        [Command("reddit?nsfwmap"), RequireNsfw, Summary("Returns a URL to a visual map of many NSFW subreddits and how they link.")]
        public async Task map()
        {
            await ReplyAsync("Alright... then... " + "http://electronsoup.net/nsfw_subreddits/#");
        }

        [Command("reddit?random", RunMode = RunMode.Async), Alias("reddit?r"), Summary("Gets a random post from a random subreddit in a predetermined list.")]
        public async Task rnd()
        {
            Random rnd = new Random();

            previous_sub = randomSubs[rnd.Next(randomSubs.Length)];
            
            previous_query = "";
            previous_sorting = 0;
            previous_hide = false;

            await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
        }

        [Command("reddit?sub", RunMode = RunMode.Async), Summary("Returns some details about a subreddit.")]
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
                Description = "Details of a subreddit",
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

        [Command("reddit?wiki", RunMode = RunMode.Async), Summary("Returns the wiki pages for a subreddit.")]
        public async Task wiki(string input)
        {
            Program.subreddit = await Program.reddit.GetSubredditAsync(input);
            
            string image = Program.subreddit.HeaderImage;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Subreddit: " + input,
                Color = Discord.Color.Orange,
                Description = "Subreddit Wiki Pages",
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

        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit.")]
        public async Task reddit_pic(string subreddit)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = "";
            previous_hide = false;

            await Operate(Context, subreddit, "", 0, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and search query.")]
        public async Task reddit_pic(string subreddit, string query)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = query;
            previous_hide = false;

            await Operate(Context, subreddit, query, 0, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, search query, and sorting method.")]
        public async Task reddit_pic(string subreddit, string query, int sorting)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = query;
            previous_hide = false;

            await Operate(Context, subreddit, query, sorting, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and sorting method.")]
        public async Task reddit_pic(string subreddit, int sorting)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = "";
            previous_hide = false;

            await Operate(Context, subreddit, "", sorting, false);
        }
        
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, but can prevent previews from showing.")]
        public async Task reddit_pic(string subreddit, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = "";
            previous_hide = hide;

            await Operate(Context, subreddit, "", 0, hide);
        }
        
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and search query, but can prevent previews from showing.")]
        public async Task reddit_pic(string subreddit, string query, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = query;
            previous_hide = false;

            await Operate(Context, subreddit, query, 0, hide);
        }
        
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, search query, sorting method, and prevention from showing previews.")]
        public async Task reddit_pic(string subreddit, string query, int sorting, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = query;
            previous_hide = hide;

            await Operate(Context, subreddit, query, sorting, hide);
        }
        
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and sorting method, but can prevent previews from showing.")]
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
