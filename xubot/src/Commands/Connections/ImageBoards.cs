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
            public ulong UserID { get; private set; }
            public ulong GuildID { get; private set; }
            public string Key { get; private set; }

            public Entry(ulong userId, ulong guildId, string key)
            {
                this.UserID = userId;
                this.GuildID = guildId;
                this.Key = key;
            }
        }

        public readonly static BooruSharp.Booru.ABooru danbooru =       new BooruSharp.Booru.DanbooruDonmai();
        public readonly static BooruSharp.Booru.ABooru e621 =           new BooruSharp.Booru.E621();
        public readonly static BooruSharp.Booru.ABooru rule34 =         new BooruSharp.Booru.Rule34();
        public readonly static BooruSharp.Booru.ABooru gelbooru =       new BooruSharp.Booru.Gelbooru();
        public readonly static BooruSharp.Booru.ABooru yandere =        new BooruSharp.Booru.Yandere();
        public readonly static BooruSharp.Booru.ABooru e926 =           new BooruSharp.Booru.E926();
        public readonly static BooruSharp.Booru.ABooru safebooru =      new BooruSharp.Booru.Safebooru();
        public readonly static BooruSharp.Booru.ABooru konachan =       new BooruSharp.Booru.Konachan();
        public readonly static BooruSharp.Booru.ABooru allthefallen =   new BooruSharp.Booru.Atfbooru();
        public readonly static BooruSharp.Booru.ABooru furrybooru =     new BooruSharp.Booru.Furrybooru();
        public readonly static BooruSharp.Booru.ABooru sankakucomplex = new BooruSharp.Booru.SankakuComplex();
        public readonly static BooruSharp.Booru.ABooru sakugabooru =    new BooruSharp.Booru.Sakugabooru();

        private readonly static Dictionary<Entry, string> caughtFromBeingSent = new Dictionary<Entry, string>();

        private async Task GetRandomPostFrom(ICommandContext context, BooruSharp.Booru.ABooru booru, params string[] inputs)
        {
            try
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

                await PostNSFWMessage(context, $"{(hide ? "|| " : "")}{post.FileUrl.AbsoluteUri}{(hide ? " ||" : "")}", post.Rating != BooruSharp.Search.Post.Rating.Safe);
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, context);
            }
        }

        private async Task PostNSFWMessage(ICommandContext context, string message, bool forceFail = false)
        {
            if ((await Util.IsDMChannel(Context)) && !BotSettings.Global.Default.DMsAlwaysNSFW)
            {
                await ReplyAsync("The bot got a post deemed questionable or explicit in a DM. DMs are set to not be NSFW, so move to a server.");
                return;
            }

            if (forceFail && !(await Util.IsChannelNSFW(context)))
            {
                string retrieve_key = Util.String.RandomHexadecimal(8);

                await ReplyAsync($"The bot got a post deemed questionable or explicit. Try again in a NSFW channel.\nYou (the requestor) can retrieve this image *once* appropriate later with the key `{Program.prefix}booru-get {retrieve_key}` on **this server only**.\n" +
                                  "The server limitation is here to keep in line Discord's age gating.");
                caughtFromBeingSent.Add(new Entry(context.Message.Author.Id, context.Guild.Id, retrieve_key), message);
                return;
            }

            await ReplyAsync(message);
        }

        [Example("night")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("danbooru", RunMode = RunMode.Async), Summary("Retrives a post from danbooru.donmani.us. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Danbooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, danbooru, inputs);
        }

        [Example("male true")]
        [NSFWPossibilty("Porn, snuff, whatever gets drawn.")]
        [Command("e621", RunMode = RunMode.Async), Summary("Retrives a post from e621.net. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task E621(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, e621, inputs);
        }

        [Example("sex true")]
        [NSFWPossibilty("Porn, snuff, whatever gets drawn.")]
        [Command("rule34", RunMode = RunMode.Async), Summary("Retrives a post from rule34.xxx, to the bot's dismay. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task R34(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, rule34, inputs);
        }

        [Example("solo true")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("gelbooru", RunMode = RunMode.Async), Summary("Retrives a post from gelbooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Gelbooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, gelbooru, inputs);
        }

        [Example("thighhighs true")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("yandere", RunMode = RunMode.Async), Summary("Retrives a post from yande.re. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Yandere(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, yandere, inputs);
        }

        [Example("male")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("e926", RunMode = RunMode.Async), Summary("Retrives a post from e926.net. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task E926(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, e926, inputs);
        }

        [Example("sky false")]
        [Command("safebooru", RunMode = RunMode.Async), Summary("Retrives a post from safebooru.org. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Safebooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, safebooru, inputs);
        }

        [Example("thighhighs true")]
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("konachan", RunMode = RunMode.Async), Summary("Retrives a post from konachan.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Konachan(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, konachan, inputs);
        }

        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("atfbooru", RunMode = RunMode.Async), Summary("Retrives a post from booru.allthefallen.moe. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task ATFBooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, allthefallen, inputs);
        }

        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("furrybooru", RunMode = RunMode.Async), Summary("Retrives a post from furry.booru.org. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Furrybooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, furrybooru, inputs);
        }

        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("sankakucomplex", RunMode = RunMode.Async), Summary("Retrives a post from beta.sankakucomplex.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task SankakuComplex(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, sankakucomplex, inputs);
        }

        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("sakugabooru", RunMode = RunMode.Async), Summary("Retrives a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
        public async Task Sakugabooru(params string[] inputs)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                GetRandomPostFrom(Context, sakugabooru, inputs);
        }

        [Example("00000000")]
        [Command("booru-get", RunMode = RunMode.Async), Summary("Gets an image you requested that wasn't appropriate for the orginial context with a given key.")]
        public async Task GetStoredImageURI(string key, bool hide = false)
        {
            if (await Util.IsDMChannel(Context))
            {
                await ReplyAsync("You can only use this command with your retrieve key on the server you made the request from, not from a DM with the bot.");
                return;
            }

            try
            {
                Entry exists = caughtFromBeingSent.First(x => (x.Key.UserID == Context.Message.Author.Id) && (x.Key.GuildID == Context.Guild.Id) && (x.Key.Key == key)).Key;

                if (exists != null)
                {
                    string value;
                    caughtFromBeingSent.Remove(exists, out value);

                    await PostNSFWMessage(Context, $"{(hide ? "|| " : "")}{value}{(hide ? " ||" : "")}");
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
