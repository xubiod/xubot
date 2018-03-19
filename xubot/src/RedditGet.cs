using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static xubot.RedditGetFunct.ParseSorting;


namespace xubot
{
    public class RedditGet : ModuleBase
    {
        static string previous_sub = "notinteresting";
        static string previous_query = "";
        static int previous_sorting = 0;
        static bool previous_hide = false;

        [Command("reddit?last"), Alias("reddit?l")]
        public async Task last()
        {
            Task.Run(async () => {
                await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
            });
        }

        [Command("reddit?random"), Alias("reddit?r")]
        public async Task rnd()
        {
            Random rnd = new Random();
            switch (rnd.Next(0, 22))
            {
                case 0: previous_sub = "adviceanimals"; break;
                case 1: previous_sub = "askreddit"; break;
                case 2: previous_sub = "aww"; break;
                case 3: previous_sub = "bestof"; break;
                case 4: previous_sub = "books"; break;
                case 5: previous_sub = "earthporn"; break;
                case 6: previous_sub = "explainlikeimfive"; break;
                case 7: previous_sub = "funny"; break;
                case 8: previous_sub = "gaming"; break;
                case 9: previous_sub = "gifs"; break;
                case 10: previous_sub = "iama"; break;
                case 11: previous_sub = "movies"; break;
                case 12: previous_sub = "music"; break;
                case 13: previous_sub = "news"; break;
                case 14: previous_sub = "pics"; break;
                case 15: previous_sub = "science"; break;
                case 16: previous_sub = "technology"; break;
                case 17: previous_sub = "television"; break;
                case 18: previous_sub = "todayilearned"; break;
                case 19: previous_sub = "videos"; break;
                case 20: previous_sub = "worldnews"; break;
                case 21: previous_sub = "wtf"; break;
            }

            previous_query = "";
            previous_sorting = 0;
            previous_hide = false;

            Task.Run(async () => {
                await Operate(Context, previous_sub, previous_query, previous_sorting, previous_hide);
            });
        }

        [Command("reddit?sub")]
        public async Task subreddit(string input)
        {
            Program.subreddit = Program.reddit.GetSubreddit(input);

            string display = Program.subreddit.DisplayName;
            string fullname = Program.subreddit.FullName;
            string NSFW = Program.subreddit.NSFW.ToString();
            string sub = string.Format("{0:#,###0}", Program.subreddit.Subscribers);
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
                                Name = "Subscriber Count",
                                Value = sub,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Full Name",
                                Value = fullname,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "NSFW?",
                                Value = NSFW,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Link",
                                Value = "https://reddit.com/r/"+input,
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd);
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = "";
            previous_hide = false;

            Task.Run(async () => {
                await Operate(Context, subreddit, "", 0, false);
            });
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit, string query)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = query;
            previous_hide = false;

            Task.Run(async () => {
                await Operate(Context, subreddit, query, 0, false);
            });
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit, string query, int sorting)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = query;
            previous_hide = false;

            Task.Run(async () => {
                await Operate(Context, subreddit, query, sorting, false);
            });
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit, int sorting)
        {

            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = "";
            previous_hide = false;

            Task.Run(async () => {
                await Operate(Context, subreddit, "", sorting, false);
            });
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit, bool hide)
        {

            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = "";
            previous_hide = hide;

            Task.Run(async () => {
                await Operate(Context, subreddit, "", 0, hide);
            });
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit, string query, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = 0;
            previous_query = query;
            previous_hide = false;

            Task.Run(async () => {
                await Operate(Context, subreddit, query, 0, hide);
            });
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit, string query, int sorting, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = query;
            previous_hide = hide;

            Task.Run(async () => {
                await Operate(Context, subreddit, query, sorting, hide);
            });
        }

        [Command("reddit")]
        public async Task reddit_pic(string subreddit, int sorting, bool hide)
        {
            previous_sub = subreddit;
            previous_sorting = sorting;
            previous_query = "";
            previous_hide = hide;

            Task.Run(async () => {
                await Operate(Context, subreddit, "", sorting, hide);
            });
        }

        /* operation functions */

        List<RedditSharp.Things.Post> contents_list = new List<RedditSharp.Things.Post> { };

        public async Task Operate(ICommandContext Context, string subreddit, string query, int sorting, bool hide)
        {
            try
            {
                Program.subreddit = Program.reddit.GetSubreddit(subreddit);
                var msg = await ReplyAsync("Subreddit: **" + subreddit + "**\nPlease wait, this takes a while with broad terms and popular subreddits!");

                await Context.Channel.TriggerTypingAsync();
                Random rnd = new Random();

                RedditSharp.Listing<RedditSharp.Things.Post> contents = Program.subreddit.Search(query, FromInt(sorting));

                int contents_count = contents.Count();

                contents_list = contents.ToList();

                var post = contents_list[rnd.Next(contents_count)];

                if (post.NSFW)
                {
                    if (!Context.Channel.IsNsfw)
                    {
                        await msg.DeleteAsync();
                        await ReplyAsync("The random post that was selected is NSFW or the subreddit is NSFW. Try again for another random post, with another subreddit, or move to a NSFW channel (needs nsfw in the name).");
                        return;
                    }
                    else
                    {
                        await msg.DeleteAsync();
                        await ReplyAsync("**" + post.Title + "**\n\nImage/Posted Link: " + ReturnCharOnTrue(hide, "<") + post.Url.AbsoluteUri + ReturnCharOnTrue(hide, ">") + " Reddit Post: <https://www.reddit.com" + post.Permalink.ToString() + ">");

                    }
                }

                await msg.DeleteAsync();
                //await ReplyAsync("https://reddit.com" + post.Permalink.ToString());

                await ReplyAsync("**" + post.Title + "**\n\nImage/Posted Link: " + ReturnCharOnTrue(hide, "<") + post.Url.AbsoluteUri + ReturnCharOnTrue(hide, ">") + " Reddit Post: <https://www.reddit.com" + post.Permalink.ToString() + ">");

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
