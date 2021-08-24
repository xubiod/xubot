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
        private static string _previousSub = BotSettings.Global.Default.StartingSubreddit;
        private static string _previousQuery = BotSettings.Global.Default.StartingRedditQuery;
        private static int _previousSorting = BotSettings.Global.Default.StartingRedditSorting;
        private static bool _previousHide = BotSettings.Global.Default.StartingRedditHideOutput;

        private static readonly string[] _randomSubs = { "adviceanimals", "askreddit", "aww", "bestof", "books", "earthporn", "explainlikeimfive", "funny", "gaming", "gifs", "iama", "movies", "music",
                                               "news", "pics", "science", "technology", "television", "todayilearned", "videos", "worldnews", "wtf" };

        [Command("reddit?last", RunMode = RunMode.Async), Alias("reddit?l"), Summary("Gets a post from the last subreddit entered."), NsfwPossibilty("Anything probably")]
        public async Task DoLastSubreddit()
        {
            await Operate(Context, _previousSub, _previousQuery, _previousSorting, _previousHide);
        }

        [NsfwPossibilty("Has links to NSFW subreddits")]
        [Command("reddit?nsfwmap"), Summary("Returns a URL to a visual map of many NSFW subreddits and how they link.")]
        public async Task GetNsfwMap()
        {
            if (await Util.IsChannelNsfw(Context)) await ReplyAsync("Alright... then... " + "http://electronsoup.net/nsfw_subreddits/#");
            else await ReplyAsync("Move to a NSFW channel.");
        }

        [Command("reddit?random", RunMode = RunMode.Async), Alias("reddit?r"), Summary("Gets a random post from a random subreddit in a predetermined list.")]
        public async Task GetFromRandomSubreddit()
        {
            Random rnd = Util.Globals.Rng;

            _previousSub = _randomSubs[rnd.Next(_randomSubs.Length)];

            await Operate(Context, _previousSub, _previousQuery, _previousSorting, _previousHide);
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

            Program.Subreddit = await Program.Reddit.GetSubredditAsync(input);

            string display = Program.Subreddit.DisplayName;
            string fullname = Program.Subreddit.FullName;
            string nsfw = Program.Subreddit.NSFW.ToString();
            string sub = string.Format("{0:#,###0}", Program.Subreddit.Subscribers);
            string image = Program.Subreddit.HeaderImage;
            string desc = Program.Subreddit.Description.Split('\n')[0];

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Subreddit: {input}", "Details of a subreddit", Discord.Color.Orange);
            embed.ThumbnailUrl = image;
            embed.Fields = new List<EmbedFieldBuilder>()
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
                    Value = nsfw,
                    IsInline = true
                },
                new EmbedFieldBuilder
                {
                    Name = "Link",
                    Value = "https://reddit.com/r/"+input,
                    IsInline = true
                }
            };

            await ReplyAsync("", false, embed.Build());
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

            Program.Subreddit = await Program.Reddit.GetSubredditAsync(input);

            string image = Program.Subreddit.HeaderImage;

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Subreddit: {input}", "Subreddit Wiki Pages", Discord.Color.Orange);
            embed.ThumbnailUrl = image;
            embed.Fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder
                {
                    Name = "Wiki",
                    Value = Program.Subreddit.GetWiki.GetPageNamesAsync(),
                    IsInline = false
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        [NsfwPossibilty("Anything probably"), Example("aww dog")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, search query, sorting method, and can prevent from showing previews.")]
        public async Task GetRedditPost(string subreddit, string query = "", int sorting = 0, bool hide = false)
        {
            await Operate(Context, subreddit, query, sorting, hide);
        }

        [NsfwPossibilty("Anything probably"), Example("aww 0")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and sorting method.")]
        public async Task GetRedditPost(string subreddit, int sorting)
        {
            await Operate(Context, subreddit, "", sorting, false);
        }

        [NsfwPossibilty("Anything probably"), Example("aww false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, bool hide)
        {
            await Operate(Context, subreddit, "", 0, hide);
        }

        [NsfwPossibilty("Anything probably"), Example("aww cat false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and search query, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, string query, bool hide)
        {
            await Operate(Context, subreddit, query, 0, hide);
        }

        [NsfwPossibilty("Anything probably"), Example("aww 0 false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and sorting method, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, int sorting, bool hide)
        {
            await Operate(Context, subreddit, "", sorting, hide);
        }

        /* operation functions */
        public async Task Operate(ICommandContext context, string subreddit, string query, int sorting, bool hide)
        {
            _previousSub = subreddit;
            _previousQuery = query;
            _previousSorting = sorting;
            _previousHide = hide;

            // is the reddit fuck off?
            if (!Program.redditEnabled)
            {
                context.Channel.SendMessageAsync("Reddit is disabled. Try again when it's back on.");
                return;
            }

            using (WorkingBlock wb = new WorkingBlock(context))
            {
                try
                {
                    Program.Subreddit = await Program.Reddit.GetSubredditAsync(subreddit);
                    Random rnd = Util.Globals.Rng;

                    Listing<Post> contents = Program.Subreddit.GetPosts(RedditTools.ParseSorting.FromIntSort(sorting), -1);
                    int contentsCount = await contents.CountAsync();

                    if (contentsCount < 10) contents = Program.Subreddit.GetPosts(-1);
                    //Console.WriteLine(contents.Count);
                    var post = await contents.ElementAtAsync(rnd.Next(contentsCount));
                    //EmbedBuilder embedd;

                    bool isNsfw = await Util.IsChannelNsfw(context);

                    string postMessage = $"**{post.Title}**\nPosted on *{post.CreatedUTC.ToShortDateString()}* by **{post.AuthorName}**\n\n{ReturnCharOnTrue(hide, "<")}{post.Url.AbsoluteUri}{ReturnCharOnTrue(hide, ">")}\n<https://www.reddit.com{post.Permalink.ToString()}>";

                    if ((post.NSFW || post.Title.Contains("NSFW") || post.Title.Contains("NSFL")) && !isNsfw)
                        postMessage = "The random post that was selected is NSFW or the subreddit is NSFW.Try again for another random post, with another subreddit, or move to a NSFW channel(needs nsfw in the name).";

                    await ReplyAsync(postMessage);
                }
                catch (Exception e)
                {
                    await Util.Error.BuildError(e, context);
                }
            }
        }

        public string ReturnCharOnTrue(bool hide, string input)
        {
            return hide ? input : null;
        }
    }
}
