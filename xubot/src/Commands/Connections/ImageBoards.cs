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
        private partial class Entry
        {
            public ulong UserId { get; private set; }
            public ulong GuildId { get; private set; }
            public string Key { get; private set; }

            public Entry(ulong userId, ulong guildId, string key)
            {
                this.UserId = userId;
                this.GuildId = guildId;
                this.Key = key;
            }
        }

        public static readonly BooruSharp.Booru.ABooru Danbooru =       new BooruSharp.Booru.DanbooruDonmai();
        public static readonly BooruSharp.Booru.ABooru E621 =           new BooruSharp.Booru.E621();
        public static readonly BooruSharp.Booru.ABooru Rule34 =         new BooruSharp.Booru.Rule34();
        public static readonly BooruSharp.Booru.ABooru Gelbooru =       new BooruSharp.Booru.Gelbooru();
        public static readonly BooruSharp.Booru.ABooru Yandere =        new BooruSharp.Booru.Yandere();
        public static readonly BooruSharp.Booru.ABooru E926 =           new BooruSharp.Booru.E926();
        public static readonly BooruSharp.Booru.ABooru Safebooru =      new BooruSharp.Booru.Safebooru();
        public static readonly BooruSharp.Booru.ABooru Konachan =       new BooruSharp.Booru.Konachan();
        public static readonly BooruSharp.Booru.ABooru Allthefallen =   new BooruSharp.Booru.Atfbooru();
        public static readonly BooruSharp.Booru.ABooru Sankakucomplex = new BooruSharp.Booru.SankakuComplex();
        public static readonly BooruSharp.Booru.ABooru Sakugabooru =    new BooruSharp.Booru.Sakugabooru();

        private static readonly Dictionary<Entry, string> CaughtFromBeingSent = new Dictionary<Entry, string>();

        private async Task GetRandomPostFrom(ICommandContext context, BooruSharp.Booru.ABooru booru, params string[] inputs)
        {
            try
            {
                string lastInput = inputs.Last();
                bool hide = false;

                if (bool.TryParse(lastInput, out hide))
                {
                    inputs = inputs.Where(x => x != inputs.Last()).ToArray<string>();
                }
                else
                {
                    hide = false;
                }

                BooruSharp.Search.Post.SearchResult post = await booru.GetRandomPostAsync(inputs);

                await PostNsfwMessage(context, $"{(hide ? "|| " : "")}{post.FileUrl.AbsoluteUri}{(hide ? " ||" : "")}", post.Rating != BooruSharp.Search.Post.Rating.Safe);
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, context);
            }
        }

        private async Task PostNsfwMessage(ICommandContext context, string message, bool forceFail = false)
        {
            if ((await Util.IsDmChannel(Context)) && !BotSettings.Global.Default.DMsAlwaysNSFW)
            {
                await ReplyAsync("The bot got a post deemed questionable or explicit in a DM. DMs are set to not be NSFW, so move to a server.");
                return;
            }

            if (forceFail && !(await Util.IsChannelNsfw(context)))
            {
                string retrieveKey = Util.String.RandomHexadecimal(8);

                await ReplyAsync($"The bot got a post deemed questionable or explicit. Try again in a NSFW channel.\nYou (the requestor) can retrieve this image *once* appropriate later with the key `{Program.prefix}booru-get {retrieveKey}` on **this server only**.\n" +
                                  "The server limitation is here to keep in line Discord's age gating.");
                CaughtFromBeingSent.Add(new Entry(context.Message.Author.Id, context.Guild.Id, retrieveKey), message);
                return;
            }

            await ReplyAsync(message);
        }

        [Example("night")]
        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("danbooru", RunMode = RunMode.Async), Summary("Retrieves a post from danbooru.donmani.us. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task DanbooruTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Danbooru, inputs);
        }

        [Example("male true")]
        [NsfwPossibilty("Porn, snuff, whatever gets drawn.")]
        [Command("e621", RunMode = RunMode.Async), Summary("Retrieves a post from e621.net. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task E621Task(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, E621, inputs);
        }

        [Example("sex true")]
        [NsfwPossibilty("Porn, snuff, whatever gets drawn.")]
        [Command("rule34", RunMode = RunMode.Async), Summary("Retrieves a post from rule34.xxx, to the bot's dismay. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task R34Task(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Rule34, inputs);
        }

        [Example("solo true")]
        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("gelbooru", RunMode = RunMode.Async), Summary("Retrieves a post from gelbooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task GelbooruTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Gelbooru, inputs);
        }

        [Example("thighhighs true")]
        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("yandere", RunMode = RunMode.Async), Summary("Retrieves a post from yande.re. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task YandereTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Yandere, inputs);
        }

        [Example("male")]
        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("e926", RunMode = RunMode.Async), Summary("Retrieves a post from e926.net. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task E926Task(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, E926, inputs);
        }

        [Example("sky false")]
        [Command("safebooru", RunMode = RunMode.Async), Summary("Retrieves a post from safebooru.org. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task SafebooruTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Safebooru, inputs);
        }

        [Example("thighhighs true")]
        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("konachan", RunMode = RunMode.Async), Summary("Retrieves a post from konachan.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task KonachanTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Konachan, inputs);
        }

        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("atfbooru", RunMode = RunMode.Async), Summary("Retrieves a post from booru.allthefallen.moe. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task AtfBooruTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Allthefallen, inputs);
        }

        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("sankakucomplex", RunMode = RunMode.Async), Summary("Retrieves a post from beta.sankakucomplex.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task SankakuComplexTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Sankakucomplex, inputs);
        }

        [NsfwPossibilty("Is a possibility (although not guaranteed).")]
        [Command("sakugabooru", RunMode = RunMode.Async), Summary("Retrieves a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task SakugabooruTask(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, Sakugabooru, inputs);
        }

        [Example("00000000")]
        [Command("booru-get", RunMode = RunMode.Async), Summary("Gets an image you requested that wasn't appropriate for the orginial context with a given key.")]
        public async Task GetStoredImageUri(string key, bool hide = false)
        {
            if (await Util.IsDmChannel(Context))
            {
                await ReplyAsync("You can only use this command with your retrieve key on the server you made the request from, not from a DM with the bot.");
                return;
            }

            try
            {
                Entry exists = CaughtFromBeingSent.First(x => (x.Key.UserId == Context.Message.Author.Id) && (x.Key.GuildId == Context.Guild.Id) && (x.Key.Key == key)).Key;

                if (exists != null)
                {
                    string value;
                    CaughtFromBeingSent.Remove(exists, out value);

                    await PostNsfwMessage(Context, $"{(hide ? "|| " : "")}{value}{(hide ? " ||" : "")}");
                }
                else
                {
                    await ReplyAsync("Your ID is not associated with that retrieve key on this server.");
                }
            }
            catch (InvalidOperationException ioe)
            {
                await ReplyAsync("Your ID is not associated with that retrieve key on this server.");
            }
        }
    }
}
