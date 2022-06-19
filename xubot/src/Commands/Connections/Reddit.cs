// using static xubot.src.RedditTools.ParseSorting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RedditSharp;
using RedditSharp.Things;
using xubot.Attributes;
using static xubot.Util;
// ReSharper disable StringLiteralTypo

namespace xubot.Commands.Connections
{
    public class Reddit : ModuleBase
    {
        private static string _previousSub = src.BotSettings.Global.Default.StartingSubreddit;
        private static string _previousQuery = src.BotSettings.Global.Default.StartingRedditQuery;
        private static int _previousSorting = src.BotSettings.Global.Default.StartingRedditSorting;
        private static bool _previousHide = src.BotSettings.Global.Default.StartingRedditHideOutput;

        private static readonly string[] RandomSubs = { "adviceanimals", "askreddit", "aww", "bestof", "books", "earthporn", "explainlikeimfive", "funny", "gaming", "gifs", "iama", "movies", "music",
                                               "news", "pics", "science", "technology", "television", "todayilearned", "videos", "worldnews", "wtf" };

        [Command("reddit?last", RunMode = RunMode.Async), Alias("reddit?l"), Summary("Gets a post from the last subreddit entered."), NsfwPossibility("Anything probably")]
        public async Task DoLastSubreddit()
        {
            await Operate(Context, _previousSub, _previousQuery, _previousSorting, _previousHide);
        }

        [NsfwPossibility("Has links to NSFW subreddits")]
        [Command("reddit?nsfwmap"), Summary("Returns a URL to a visual map of many NSFW subreddits and how they link.")]
        public async Task GetNsfwMap()
        {
            if (await IsChannelNsfw(Context)) await ReplyAsync("Alright... then... " + "http://electronsoup.net/nsfw_subreddits/#");
            else await ReplyAsync("Move to a NSFW channel.");
        }

        [Command("reddit?random", RunMode = RunMode.Async), Alias("reddit?r"), Summary("Gets a random post from a random subreddit in a predetermined list.")]
        public async Task GetFromRandomSubreddit()
        {
            Random rnd = Globals.Rng;

            _previousSub = RandomSubs[rnd.Next(RandomSubs.Length)];

            await Operate(Context, _previousSub, _previousQuery, _previousSorting, _previousHide);
        }

        [Command("reddit?sub", RunMode = RunMode.Async), Summary("Returns some details about a subreddit.")]
        public async Task GetDetailsFromSubreddit(string input)
        {
            // is the reddit fuck off?
            if (!Program.RedditEnabled)
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

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Subreddit: {input}", "Details of a subreddit", Color.Orange);
            embed.ThumbnailUrl = image;
            embed.Fields = new List<EmbedFieldBuilder>
            {
                new()
                {
                    Name = "First line of Description",
                    Value = desc,
                    IsInline = false
                },
                new()
                {
                    Name = "Subscriber Count",
                    Value = sub,
                    IsInline = true
                },
                new()
                {
                    Name = "Display Name",
                    Value = display,
                    IsInline = true
                },
                new()
                {
                    Name = "Internal Name",
                    Value = fullname,
                    IsInline = true
                },
                new()
                {
                    Name = "NSFW?",
                    Value = nsfw,
                    IsInline = true
                },
                new()
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
            if (!Program.RedditEnabled)
            {
                Context.Channel.SendMessageAsync("Reddit is disabled. Try again when it's back on.");
                return;
            }

            Program.Subreddit = await Program.Reddit.GetSubredditAsync(input);

            string image = Program.Subreddit.HeaderImage;

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Subreddit: {input}", "Subreddit Wiki Pages", Color.Orange);
            embed.ThumbnailUrl = image;
            embed.Fields = new List<EmbedFieldBuilder>
            {
                new()
                {
                    Name = "Wiki",
                    Value = Program.Subreddit.GetWiki.GetPageNamesAsync(),
                    IsInline = false
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        [NsfwPossibility("Anything probably"), Example("aww dog")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, search query, sorting method, and can prevent from showing previews.")]
        public async Task GetRedditPost(string subreddit, string query = "", int sorting = 0, bool hide = false)
        {
            await Operate(Context, subreddit, query, sorting, hide);
        }

        [NsfwPossibility("Anything probably"), Example("aww 0")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and sorting method.")]
        public async Task GetRedditPost(string subreddit, int sorting)
        {
            await Operate(Context, subreddit, "", sorting, false);
        }

        [NsfwPossibility("Anything probably"), Example("aww false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, bool hide)
        {
            await Operate(Context, subreddit, "", 0, hide);
        }

        [NsfwPossibility("Anything probably"), Example("aww cat false")]
        [Command("reddit", RunMode = RunMode.Async), Summary("Returns a random post given the subreddit and search query, but can prevent previews from showing.")]
        public async Task GetRedditPost(string subreddit, string query, bool hide)
        {
            await Operate(Context, subreddit, query, 0, hide);
        }

        [NsfwPossibility("Anything probably"), Example("aww 0 false")]
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
            if (!Program.RedditEnabled)
            {
                context.Channel.SendMessageAsync("Reddit is disabled. Try again when it's back on.");
                return;
            }

            using WorkingBlock wb = new WorkingBlock(context);
            try
            {
                Program.Subreddit = await Program.Reddit.GetSubredditAsync(subreddit);
                Random rnd = Globals.Rng;

                Listing<Post> contents = Program.Subreddit.GetPosts(RedditTools.ParseSorting.FromIntSort(sorting));
                int contentsCount = await contents.CountAsync();

                if (contentsCount < 10) contents = Program.Subreddit.GetPosts();
                //Console.WriteLine(contents.Count);
                var post = await contents.ElementAtAsync(rnd.Next(contentsCount));
                //EmbedBuilder embedd;

                bool isNsfw = await IsChannelNsfw(context);

                string postMessage = $"**{post.Title}**\nPosted on *{post.CreatedUTC.ToShortDateString()}* by **{post.AuthorName}**\n\n{ReturnCharOnTrue(hide, "<")}{post.Url.AbsoluteUri}{ReturnCharOnTrue(hide, ">")}\n<https://www.reddit.com{post.Permalink}>";

                if ((post.NSFW || post.Title.Contains("NSFW") || post.Title.Contains("NSFL")) && !isNsfw)
                    postMessage = "The random post that was selected is NSFW or the subreddit is NSFW.Try again for another random post, with another subreddit, or move to a NSFW channel(needs nsfw in the name).";

                await ReplyAsync(postMessage);
            }
            catch (Exception e)
            {
                await Error.BuildError(e, context);
            }
        }

        public string ReturnCharOnTrue(bool hide, string input)
        {
            return hide ? input : null;
        }
    }
}
