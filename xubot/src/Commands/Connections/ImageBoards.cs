﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooruSharp.Booru;
using BooruSharp.Search.Post;
using Discord.Commands;
using xubot.Attributes;
using SearchResult = BooruSharp.Search.Post.PostSearchResult;

namespace xubot.Commands.Connections;

public class ImageBoards : ModuleBase
{
    private class Entry(ulong userId, ulong guildId, string key)
    {
        public ulong UserId { get; } = userId;
        public ulong GuildId { get; } = guildId;
        public string Key { get; } = key;
    }

    private static readonly ABooru Allthefallen =   new Atfbooru();
    private static readonly ABooru Danbooru =       new DanbooruDonmai();
    private static readonly ABooru Derpibooru =     new Derpibooru();
    private static readonly ABooru E621 =           new E621();
    private static readonly ABooru E926 =           new E926();
    private static readonly ABooru Gelbooru =       new Gelbooru();
    private static readonly ABooru Konachan =       new Konachan();
    private static readonly ABooru Ponybooru =      new Ponybooru();
    private static readonly ABooru Realbooru =      new Realbooru();
    private static readonly ABooru Rule34 =         new Rule34();
    private static readonly ABooru Safebooru =      new Safebooru();
    private static readonly ABooru Sakugabooru =    new Sakugabooru();
    private static readonly ABooru Sankakucomplex = new SankakuComplex();
    private static readonly ABooru Twibooru =       new Twibooru();
    private static readonly ABooru Xbooru =         new Xbooru();
    private static readonly ABooru Yandere =        new Yandere();

    private static readonly Dictionary<Entry, string> CaughtFromBeingSent = new();

    private async Task GetRandomPostFrom(ICommandContext context, ABooru booru, params string[] inputs)
    {
        try
        {
            var lastInput = inputs.Last();

            if (bool.TryParse(lastInput, out var hide))
            {
                inputs = inputs.Where(x => x != inputs.Last()).ToArray();
            }
            else
            {
                hide = false;
            }

            var post = await booru.GetRandomPostAsync(inputs);

            await PostNsfwMessage(context, $"{(hide ? "|| " : "")}{post.FileUrl.AbsoluteUri}{(hide ? " ||" : "")}", post.Rating != Rating.Safe);
        }
        catch (Exception e)
        {
            await Util.Error.BuildErrorAsync(e, context);
        }
    }

    private async Task PostNsfwMessage(ICommandContext context, string message, bool forceFail = false)
    {
        if (await Util.IsDmChannelAsync(Context) && !src.BotSettings.Global.Default.DMsAlwaysNSFW)
        {
            await ReplyAsync("The bot got a post deemed questionable or explicit in a DM. DMs are set to not be NSFW, so move to a server.");
            return;
        }

        if (forceFail && !await Util.IsChannelNsfwAsync(context))
        {
            var retrieveKey = Util.String.RandomHexadecimal(8);

            await ReplyAsync($"The bot got a post deemed questionable or explicit. Try again in a NSFW channel.\nYou (the requester) can retrieve this image *once* appropriate later with the key `{Program.Prefix}booru-get {retrieveKey}` on **this server only**.\n" +
                             "The server limitation is here to keep in line Discord's age gating.");
            CaughtFromBeingSent.Add(new Entry(context.Message.Author.Id, context.Guild.Id, retrieveKey), message);
            return;
        }

        await ReplyAsync(message);
    }

    [Example("night")]
    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("danbooru", RunMode = RunMode.Async), Summary("Retrieves a post from danbooru.donmani.us. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task DanbooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Danbooru, inputs);
    }

    [Example("male true")]
    [NsfwPossibility("Porn, snuff, whatever gets drawn.")]
    [Command("e621", RunMode = RunMode.Async), Summary("Retrieves a post from e621.net. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task E621Task(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, E621, inputs);
    }

    [Example("sex true")]
    [NsfwPossibility("Porn, snuff, whatever gets drawn.")]
    [Command("rule34", RunMode = RunMode.Async), Summary("Retrieves a post from rule34.xxx, to the bot's dismay. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task R34Task(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Rule34, inputs);
    }

    [Example("solo true")]
    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("gelbooru", RunMode = RunMode.Async), Summary("Retrieves a post from gelbooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task GelbooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Gelbooru, inputs);
    }

    [Example("thighhighs true")]
    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("yandere", RunMode = RunMode.Async), Summary("Retrieves a post from yande.re. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task YandereTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Yandere, inputs);
    }

    [Example("male")]
    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("e926", RunMode = RunMode.Async), Summary("Retrieves a post from e926.net. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task E926Task(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, E926, inputs);
    }

    [Example("sky false")]
    [Command("safebooru", RunMode = RunMode.Async), Summary("Retrieves a post from safebooru.org. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task SafebooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Safebooru, inputs);
    }

    [Example("thighhighs true")]
    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("konachan", RunMode = RunMode.Async), Summary("Retrieves a post from konachan.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task KonachanTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Konachan, inputs);
    }

    // TODO: unfuck up these attributes below and give some fucking examples, cannot be arsed to make the website urls right atm

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("atfbooru", RunMode = RunMode.Async), Summary("Retrieves a post from booru.allthefallen.moe. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task AtfBooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Allthefallen, inputs);
    }

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("sankakucomplex", RunMode = RunMode.Async), Summary("Retrieves a post from beta.sankakucomplex.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task SankakuComplexTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Sankakucomplex, inputs);
    }

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("sakugabooru", RunMode = RunMode.Async), Summary("Retrieves a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task SakugabooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Sakugabooru, inputs);
    }

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("realbooru", RunMode = RunMode.Async), Summary("Retrieves a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task RealbooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Realbooru, inputs);
    }

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("derpibooru", RunMode = RunMode.Async), Summary("Retrieves a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task DerpibooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Derpibooru, inputs);
    }

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("ponybooru", RunMode = RunMode.Async), Summary("Retrieves a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task PonybooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Ponybooru, inputs);
    }

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("twibooru", RunMode = RunMode.Async), Summary("Retrieves a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task TwibooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Twibooru, inputs);
    }

    [NsfwPossibility("Is a possibility (although not guaranteed).")]
    [Command("xbooru", RunMode = RunMode.Async), Summary("Retrieves a post from sakugabooru.com. If the last input is a boolean, it counts as a spoiler toggle.")]
    public async Task XbooruTask(params string[] inputs)
    {
        using Util.WorkingBlock wb = new(Context);
        await GetRandomPostFrom(Context, Xbooru, inputs);
    }

    [Example("00000000")]
    [Command("booru-get", RunMode = RunMode.Async), Summary("Gets an image you requested that wasn't appropriate for the original context with a given key.")]
    public async Task GetStoredImageUri(string key, bool hide = false)
    {
        if (await Util.IsDmChannelAsync(Context))
        {
            await ReplyAsync("You can only use this command with your retrieve key on the server you made the request from, not from a DM with the bot.");
            return;
        }

        try
        {
            var exists = CaughtFromBeingSent.First(x => x.Key.UserId == Context.Message.Author.Id && x.Key.GuildId == Context.Guild.Id && x.Key.Key == key).Key;

            if (exists != null)
            {
                CaughtFromBeingSent.Remove(exists, out var value);

                await PostNsfwMessage(Context, $"{(hide ? "|| " : "")}{value}{(hide ? " ||" : "")}");
            }
            else
            {
                await ReplyAsync("Your ID is not associated with that retrieve key on this server.");
            }
        }
        catch
        {
            await ReplyAsync("Your ID is not associated with that retrieve key on this server.");
        }
    }
}