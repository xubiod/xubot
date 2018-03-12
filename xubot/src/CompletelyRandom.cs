using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
