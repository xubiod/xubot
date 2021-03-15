using Discord;
using Discord.Commands;
using RedditSharp;
using RedditSharp.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using xubot.src.Attributes;

// using static xubot.src.RedditTools.ParseSorting;
using static xubot.src.Util;

namespace xubot.src.Commands.Connections
{
    public class Reddit : ModuleBase
    {
        private static string previous_sub = "notinteresting";
        private static string previous_query = "";
        private static int previous_sorting = 0;
        private static bool previous_hide = false;

        private static string[] randomSubs = { "adviceanimals", "askreddit", "aww", "bestof", "books", "earthporn", "explainlikeimfive", "funny", "gaming", "gifs", "iama", "movies", "music",
                                               "news", "pics", "science", "technology", "television", "todayilearned", "videos", "worldnews", "wtf" };

        [Command("reddit?last", RunMode = RunMode.Async), Alias("reddit?l"), Summary("Gets a post from the last subreddit entered."), NSFWPossibilty("Anything probably")]
        public async Task DoLastSubreddit()
        {
            await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
        }

        [NSFWPossibilty("Has links to NSFW subreddits")]
        [Command("reddit?nsfwmap"), Summary("Returns a URL to a visual map of many NSFW subreddits and how they link.")]
        public async Task GetNSFWMap()
        {
            if (await Util.IsChannelNSFW(Context)) await ReplyAsync("Alright... then... " + "http://electronsoup.net/nsfw_subreddits/#");
            else await ReplyAsync("Move to a NSFW channel.");
        }

        [Command("reddit?random", RunMode = RunMode.Async), Alias("reddit?r"), Summary("Gets a random post from a random subreddit in a predetermined list.")]
        public async Task GetFromRandomSubreddit()
        {
            Random rnd = Util.Globals.RNG;

            previous_sub = randomSubs[rnd.Next(randomSubs.Length)];

            await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
        }

        [Command("reddit?sub", RunMode = RunMode.Async), Summary("Returns some details about a subreddit.")]
        public async Task GetDetailsFromSubreddit(string input)
        {
            // is the reddit fuck off?
            if (!Program.redditEnabled)
            {
                Context.Channel.SendMessageAsync("Reddit is disabled. Try again when it's back on.");
                return;
            }

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
                    Text = Util.Globals.EmbedFooter
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
        public async Task GetSubredditWiki(string input)
        {
            // is the reddit fuck off?
            if (!Program.redditEnabled)
            {
                Context.Channel.SendMessageAsync("Reddit is disabled. Try again when it's back on.");
                return;
            }

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
                    Text = Util.Globals.EmbedFooter
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

        [NSFWPossibilty("Anything probably"), Example("aww dog")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, search query, sorting method, and can prevent from showing previews.")]
        public async Task GetRedditPost(string subreddit, string query = "", int sorting = 0, bool hide = false)
        {
            await Operate(Context, subreddit, query, sorting, hide);
        }

        [NSFWPossibilty("Anything probably"), Example("aww 0")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and sorting method.")]
        public async Task GetRedditPost(string subreddit, int sorting)
        {
            await Operate(Context, subreddit, "", sorting, false);
        }

        [NSFWPossibilty("Anything probably"), Example("aww false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, bool hide)
        {
            await Operate(Context, subreddit, "", 0, hide);
        }

        [NSFWPossibilty("Anything probably"), Example("aww cat false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and search query, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, string query, bool hide)
        {
            await Operate(Context, subreddit, query, 0, hide);
        }

        [NSFWPossibilty("Anything probably"), Example("aww 0 false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and sorting method, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, int sorting, bool hide)
        {
            await Operate(Context, subreddit, "", sorting, hide);
        }

        /* operation functions */
        public async Task Operate(ICommandContext Context, string subreddit, string query, int sorting, bool hide)
        {
            previous_sub = subreddit;
            previous_query = query;
            previous_sorting = sorting;
            previous_hide = hide;

            // is the reddit fuck off?
            if (!Program.redditEnabled)
            {
                Context.Channel.SendMessageAsync("Reddit is disabled. Try again when it's back on.");
                return;
            }

            using (WorkingBlock wb = new WorkingBlock(Context))
            {
                try
                {
                    Program.subreddit = await Program.reddit.GetSubredditAsync(subreddit);
                    Random rnd = Util.Globals.RNG;

                    Listing<Post> contents = Program.subreddit.GetPosts(RedditTools.ParseSorting.FromIntSort(sorting), -1);
                    int contents_count = await contents.CountAsync();

                    if (contents_count < 10) contents = Program.subreddit.GetPosts(-1);
                    //Console.WriteLine(contents.Count);
                    var post = await contents.ElementAtAsync(rnd.Next(contents_count));
                    //EmbedBuilder embedd;

                    bool isNSFW = await Util.IsChannelNSFW(Context);

                    string post_message = $"**{post.Title}**\nPosted on *{post.CreatedUTC.ToShortDateString()}* by **{post.AuthorName}**\n\n{ReturnCharOnTrue(hide, "<")}{post.Url.AbsoluteUri}{ReturnCharOnTrue(hide, ">")}\n<https://www.reddit.com{post.Permalink.ToString()}>";

                    if ((post.NSFW || post.Title.Contains("NSFW") || post.Title.Contains("NSFL")) && !isNSFW)
                        post_message = "The random post that was selected is NSFW or the subreddit is NSFW.Try again for another random post, with another subreddit, or move to a NSFW channel(needs nsfw in the name).";

                    await ReplyAsync(post_message);
                }
                catch (Exception e)
                {
                    await Util.Error.BuildError(e, Context);
                }
            }
        }

        public string ReturnCharOnTrue(bool hide, string input)
        {
            if (hide) { return input; }
            else { return null; }
        }
    }
}
