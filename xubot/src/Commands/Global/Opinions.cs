using System;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using xubot.Attributes;

namespace xubot.Commands.Global
{
    [Group("opinion")]
    public class Opinions : ModuleBase
    {
        [Example("xubot")]
        [Command(""), Alias("get"), Summary("Gets the bot's opinion on something. Was funny but now has been forgotten.")]
        public async Task OpinionGet(string input)
        {
            Random replyDecide = Util.Globals.Rng;

            string reply = "";

            switch (replyDecide.Next(3))
            {
                case 0: reply = "I don't have an opinion on that yet."; break;
                case 1: reply = "I got no opinion on that yet."; break;
                case 2: reply = "I either don't know what that is, or I just don't have an opinion."; break;
                case 3: reply = "No opinion for this yet."; break;
            }

            reply = (Program.JsonKeys["opinion"].Contents as JObject).Value<string>(input) ?? reply;

            await ReplyAsync(reply);
        }

        [Command("set"), Summary("Sets the bot's opinion on something. Owner only."), RequireOwner]
        public async Task OpinionSet(string input, string output)
        {
            if ((Program.JsonKeys["opinion"].Contents as JObject).ContainsKey(input)) (Program.JsonKeys["opinion"].Contents as JObject)[input] = output;
            else (Program.JsonKeys["opinion"].Contents as JObject).Add(input, output);

            Util.Json.SaveKeyAsJson("opinion");

            await ReplyAsync("Opinion set.");
        }
    }
}
