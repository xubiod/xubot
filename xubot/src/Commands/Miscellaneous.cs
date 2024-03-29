﻿using System.Threading.Tasks;
using Discord.Commands;
using xubot.Attributes;

namespace xubot.Commands;

public class Miscellaneous : ModuleBase
{
    [Command("dog-unrolling-from-bubble-wrap"), Summary("I honestly forgot why this is here.")]
    public async Task BubbleWrap()
    {
        await ReplyAsync("https://68.media.tumblr.com/6dd362a8aafe8bbdacf8fb32a5c6b528/tumblr_ncmv90I1gM1qj26eao1_400.gif");
    }

    [Command("( ͡° ͜ʖ ͡°)"), Alias("lenny"), Summary("( ͡° ͜ʖ ͡°)")]
    public async Task Lenny()
    {
        await ReplyAsync("( ͡° ͜ʖ ͡°)");
    }

    [Command("🥚"), Alias("egg"), Summary("Egg.")]
    public async Task Egg()
    {
        await ReplyAsync("⬛⬛🥚🥚⬛⬛\n" +
                         "⬛🥚🥚🥚🥚⬛\n" +
                         "🥚🥚🥚🥚🥚🥚\n" +
                         "🥚🥚🥚🥚🥚🥚\n" +
                         "🥚🥚🥚🥚🥚🥚\n" +
                         "⬛🥚🥚🥚🥚⬛");
    }

    [Command("no-need-to-be-upset"), Summary(":)")]
    public async Task NoNeedToBeUpset()
    {
        await ReplyAsync("https://youtu.be/GJDNkVDGM_s");
    }

    [Command("gay-frogs"), Summary("I DONT LIKE PUTTIN CHEMICALS IN THE WATER THAT TURN THE FRICKIN FROGS GAY")]
    public async Task GayFrogs()
    {
        await ReplyAsync("https://youtu.be/9JRLCBb7qK8");
    }

    [Example("uwu")]
    [Command("make-this-middle-finger"), Summary("Takes text and makes it rude.")]
    public async Task MiddleFinger(string face)
    {
        await ReplyAsync("(凸 " + face + ")凸");
    }

    [Command("rm -rf"), Alias("rm -rf --no-preserve-root /"), Summary("Deletes xubot and the rest of the computer it's running on.")]
    public async Task DeleteLinux()
    {
        await ReplyAsync("no u");
    }

    // ReSharper disable once StringLiteralTypo
    [Command("yritwh"), Alias("you-reposted-in-the-wrong-neighborhood"), Summary("Use as a reaction to a meme that has been reposted inappropriately.")]
    public async Task WrongNeighbourhood()
    {
        await ReplyAsync("https://youtu.be/0cOAUSVBGX8");
    }

    [Command("what-is-the-best-feeling"), Summary("A mistake.")]
    public async Task OhGoodGod()
    {
        await ReplyAsync("https://youtu.be/0tdyU_gW6WE");
    }

    [Command("is-there-soap-everywhere"), Summary("It lOoKs lIke a iCe cReAm dIsPeNsEr")]
    public async Task Soap()
    {
        await ReplyAsync("https://youtu.be/fcYRmNx1FBA");
    }

    [Command("english-motherfucker"), Alias("english-mf"), Summary("DO YOU SPEAK IT???")]
    public async Task WhatIsNotACountryIveHeardOf()
    {
        await ReplyAsync("https://youtu.be/a0x6vIAtFcI");
    }

    // ReSharper disable once StringLiteralTypo
    [Command("what-does-a-cat-in-zero-g-look-like"), Alias("wdacizgll")]
    public async Task ZeroGCat()
    {
        await ReplyAsync("A cat in a zero G flight looks like this: https://youtu.be/hb4Yd4mEVsE");
    }

    [Command("there-is-a-steam-sale"), Alias("praise-lord-gaben"), Summary("TONIGHT'S THE NIGHT!")]
    public async Task ItIsTimeToCelebrate()
    {
        await ReplyAsync("https://youtu.be/bUo1PgKksgw");
    }

    [Command("santa-kills-the-kids"), Alias("santa-blows-up-children"), Summary(">:)")]
    public async Task SantaFuckingKillsTheKids()
    {
        await ReplyAsync("https://youtu.be/HG2F3hMcBrs");
    }

    [Command("lil-bitch"), Alias("stop-being-such-a-lil-bitch"), Summary("lil bitch")]
    public async Task LilBitch()
    {
        await ReplyAsync("https://youtu.be/kTzKZDvQhBI");
    }

    [Command("wake-up"), Alias("bro-wake-up", "2006"), Summary("Bro wake up Its 2006!")]
    public async Task Its2006()
    {
        await ReplyAsync("https://youtu.be/NNJ21Gzp79E");
    }
}