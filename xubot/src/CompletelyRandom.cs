using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xubot.src;

namespace xubot
{
    public class CompletelyRandom : ModuleBase
    {
        [Command("dog-unrolling-from-bubble-wrap")]
        public async Task dufbw()
        {
            await ReplyAsync("https://68.media.tumblr.com/6dd362a8aafe8bbdacf8fb32a5c6b528/tumblr_ncmv90I1gM1qj26eao1_400.gif");
        }

        [Command("( ͡° ͜ʖ ͡°)"), Alias("lenny")]
        public async Task lenny()
        {
            await ReplyAsync("( ͡° ͜ʖ ͡°)");
        }

        [Command("🥚"), Alias("egg")] 
        public async Task egg()
        {
            await ReplyAsync("⬛⬛🥚🥚⬛⬛\n" +
                             "⬛🥚🥚🥚🥚⬛\n" +
                             "🥚🥚🥚🥚🥚🥚\n" +
                             "🥚🥚🥚🥚🥚🥚\n" +
                             "🥚🥚🥚🥚🥚🥚\n" +
                             "⬛🥚🥚🥚🥚⬛");
        }

        [Command("no-need-to-be-upset")]
        public async Task nntbu()
        {
            await ReplyAsync("https://youtu.be/GJDNkVDGM_s");
        }

        [Command("gay-frogs")]
        public async Task gf()
        {
            await ReplyAsync("https://youtu.be/9JRLCBb7qK8");
        }

        [Command("make-this-middle-finger")]
        public async Task mtmf(string face)
        {
            await ReplyAsync("(凸 " + face + ")凸");
        }

        [Command("rm -rf"), Alias("rm -rf --no-preserve-root /")]
        public async Task rmrf()
        {
            await ReplyAsync("no u");
        }

        [Command("anon", RunMode = RunMode.Async), RequireContext(ContextType.DM), Summary("Sends someone an anonymous message. They must have a DM of the bot open to work.")]
        public async Task anonmsg(ulong id, string msg)
        {
            if (Economy.EconomyTools.ReadAmount(Context.Message.Author) > 10)
            {
                Economy.EconomyTools.Adjust(Context.Message.Author, -10);
            } else
            {
                await ReplyAsync("You need at least 10# from the economy to use this command.");
                return;
            }
            IUser sendTo = Program.xuClient.GetUser(id);

            IDMChannel dm = await sendTo.GetOrCreateDMChannelAsync();
            await dm.SendMessageAsync(msg);
        }

        [Command("anon"), RequireContext(ContextType.DM), Summary("Sends someone an anonymous message. They must have a DM of the bot open to work.")]
        public async Task anonmsg(string user, string discrm, string msg)
        {
            if (Economy.EconomyTools.ReadAmount(Context.Message.Author) > 10)
            {
                Economy.EconomyTools.Adjust(Context.Message.Author, -10);
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

        [Command("yritwh"), Alias("you-reposted-in-the-wrong-neighborhood")]
        public async Task yritwh()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=0cOAUSVBGX8");
        }

        [Command("what-is-the-best-feeling")]
        public async Task ohGoodGod()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=0tdyU_gW6WE");
        }
    }
}
