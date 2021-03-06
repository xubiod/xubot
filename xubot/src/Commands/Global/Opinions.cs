using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Discord;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace xubot.src.Commands.Global
{
    [Group("opinion")]
    public class Opinions : ModuleBase
    {
        [Command(""), Alias("get"), Summary("Gets xubot's opinion on something. Was funny but now has been forgotten.")]
        public async Task OpinionGet(string input)
        {
            Random _reply_decide = Util.Globals.RNG;

            string reply = "";

            switch (_reply_decide.Next(3))
            {
                case 0: reply = "I don't have an opinion on that yet."; break;
                case 1: reply = "I got no opinion on that yet."; break;
                case 2: reply = "I either don't know what that is, or I just don't have an opinion."; break;
                case 3: reply = "No opinion for this yet."; break;
            }

            reply = (Program.JSONKeys["opinion"].Contents as JObject).Value<string>(input) ?? reply;

            await ReplyAsync(reply);
        }

        [Command("set"), Summary("Sets xubot's opinion on something. Owner only."), RequireOwner]
        public async Task OpinionSet(string input, string output)
        {
            if ((Program.JSONKeys["opinion"].Contents as JObject).ContainsKey(input)) (Program.JSONKeys["opinion"].Contents as JObject)[input] = output;
            else (Program.JSONKeys["opinion"].Contents as JObject).Add(input, output);

            Util.JSON.SaveKeyAsJSON("opinion");

            await ReplyAsync("Opinion set.");
        }
    }
}
