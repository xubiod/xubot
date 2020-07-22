using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot_core.src
{
    public class Miscellanous : ModuleBase
    {
        [Command("dog-unrolling-from-bubble-wrap"), Summary("I honestly forgot why this is here.")]
        public async Task DUFBW()
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
        public async Task NNTBU()
        {
            await ReplyAsync("https://youtu.be/GJDNkVDGM_s");
        }

        [Command("gay-frogs"), Summary("I DONT LIKE PUTTIN CHEMICALS IN THE WATER THAT TURN THE FRICKIN FROGS GAY")]
        public async Task GayFrogs()
        {
            await ReplyAsync("https://youtu.be/9JRLCBb7qK8");
        }

        [Command("make-this-middle-finger"), Summary("Takes text and makes it rude.")]
        public async Task MTMF(string face)
        {
            await ReplyAsync("(凸 " + face + ")凸");
        }

        [Command("rm -rf"), Alias("rm -rf --no-preserve-root /"), Summary("Deletes xubot and the rest of the computer it's running on.")]
        public async Task RMRF()
        {
            await ReplyAsync("no u");
        }

        [Command("anon", RunMode = RunMode.Async), RequireContext(ContextType.DM), Summary("Sends someone an anonymous message. They must have a DM of the bot open to work.")]
        public async Task AnonMsg(ulong id, string msg)
        {
            if (Globals.Economy.EconomyTools.ReadAmount(Context.Message.Author) > 10)
            {
                Globals.Economy.EconomyTools.Adjust(Context.Message.Author, -10);
            }
            else
            {
                await ReplyAsync("You need at least 10# from the economy to use this command.");
                return;
            }
            IUser sendTo = Program.xuClient.GetUser(id);

            IDMChannel dm = await sendTo.GetOrCreateDMChannelAsync();
            await dm.SendMessageAsync(msg);
        }

        [Command("anon"), RequireContext(ContextType.DM), Summary("Sends someone an anonymous message. They must have a DM of the bot open to work.")]
        public async Task AnonMsg(string user, string discrm, string msg)
        {
            if (Globals.Economy.EconomyTools.ReadAmount(Context.Message.Author) > 10)
            {
                Globals.Economy.EconomyTools.Adjust(Context.Message.Author, -10);
            }
            else
            {
                await ReplyAsync("You need at least 10# from the economy to use this command.");
                return;
            }
            IUser sendTo = Program.xuClient.GetUser(user, discrm);

            IDMChannel dm = await sendTo.GetOrCreateDMChannelAsync();
            await dm.SendMessageAsync(msg);
        }

        [Command("yritwh"), Alias("you-reposted-in-the-wrong-neighborhood"), Summary("Use as a reaction to a meme that has been reposted inappropriately.")]
        public async Task YRITWH()
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
        public async Task WhatIsntACountryIveHeardOf()
        {
            await ReplyAsync("https://youtu.be/a0x6vIAtFcI");
        }

        [Command("what-does-a-cat-in-zero-g-look-like"), Alias("wdacizgll")]
        public async Task WDACIZGLL()
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
    }
}
