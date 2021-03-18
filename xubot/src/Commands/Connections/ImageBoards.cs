using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Xml.Linq;
using xubot.src;
using Discord;
using System.Net.Http;
using xubot.src.Attributes;
using System.Web;
using BooruSharp;

namespace xubot.src.Commands.Connections
{
    using static xubot.src.SpecialException;

    public class ImageBoards : ModuleBase
    {
        //public JObject jsonInt = new JObject();
        public readonly static BooruSharp.Booru.DanbooruDonmai danbooru = new BooruSharp.Booru.DanbooruDonmai();
        public readonly static BooruSharp.Booru.E621 e621 = new BooruSharp.Booru.E621();
        public readonly static BooruSharp.Booru.Rule34 rule34 = new BooruSharp.Booru.Rule34();
        public readonly static BooruSharp.Booru.Gelbooru gelbooru = new BooruSharp.Booru.Gelbooru();
        public readonly static BooruSharp.Booru.Yandere yandere = new BooruSharp.Booru.Yandere();
        public readonly static BooruSharp.Booru.E926 e926 = new BooruSharp.Booru.E926();
        public readonly static BooruSharp.Booru.Safebooru safebooru = new BooruSharp.Booru.Safebooru();
        public readonly static BooruSharp.Booru.Konachan konachan = new BooruSharp.Booru.Konachan();

        private async Task GetRandomPostFrom(ICommandContext context, dynamic booru, params string[] inputs)
        {
            string last_input = inputs.Last();
            bool hide = false;

            if (bool.TryParse(last_input, out hide))
            {
                inputs = inputs.Where(x => x != inputs.Last()).ToArray<string>();
            }
            else
            {
                hide = false;
            }

            BooruSharp.Search.Post.SearchResult post = await booru.GetRandomPostAsync(inputs);

            if (post.Rating != BooruSharp.Search.Post.Rating.Safe && !(await Util.IsChannelNSFW(context)))
            {
                await ReplyAsync("The bot got a post deemed questionable or explicit. Try again in a NSFW channel.");
                return;
            }

            await ReplyAsync($"{(hide ? "|| " : "")}{post.FileUrl.AbsoluteUri}{(hide ? " ||" : "")}");
        }

        [Example("night")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("danbooru", RunMode = RunMode.Async), Summary("Retrives a post from danbooru. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Danbooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, danbooru, inputs);
        }

        [NSFWPossibilty("male true")]
        [Command("e621", RunMode = RunMode.Async), Summary("Retrives a post from e621. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task E621(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, e621, inputs);
        }

        [Example("sex true")]
        [NSFWPossibilty("Porn, snuff, whatever gets drawn.")]
        [Command("rule34", RunMode = RunMode.Async), Summary("Retrives a post from rule34, to the bot's dismay. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task R34(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, rule34, inputs);
        }

        [Example("solo true")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("gelbooru", RunMode = RunMode.Async), Summary("Retrives a post from gelbooru. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Gelbooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, gelbooru, inputs);
        }

        [Example("thighhighs true")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("yandere", RunMode = RunMode.Async), Summary("Retrives a post from yandere. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Yandere(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, yandere, inputs);
        }

        [Example("male")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("e926", RunMode = RunMode.Async), Summary("Retrives a post from e926. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task E926(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, e926, inputs);
        }

        [Example("sky false")]
        [Command("safebooru", RunMode = RunMode.Async), Summary("Retrives a post from safebooru. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Safebooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, safebooru, inputs);
        }

        [Example("thighhighs true")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("konachan", RunMode = RunMode.Async), Summary("Retrives a post from konachan. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Konachan(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, konachan, inputs);
        }
    }
}
