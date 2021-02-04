using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using xubot.src.Commands.Global;

namespace xubot.src.Commands.Global
{
    public class Mood : ModuleBase
    {
        public static XDocument xdoc = new XDocument();

        [Command("mood"), Summary("Get's your mood/emotion value from xubot.")]
        public async Task GetMoodCMD()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);
            double mood = MoodTools.ReadMood(Context.Message.Author);
            string moodAsStr = "invalid";

            if (-16 <= mood && mood <= 16) { moodAsStr = "neutral"; }
            else if (-16 >= mood) { moodAsStr = "negative"; }
            else if (mood >= 16) { moodAsStr = "positive"; }

            await ReplyAsync("Your mood value is " + mood.ToString() + " (aka " + moodAsStr + ")", false);
        }

        [Command("pet"), Alias("stroke"), Summary("why.")]
        public async Task Pet()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            double mood = MoodTools.ReadMood(Context.Message.Author);

            if (-16 <= mood && mood <= 16)
            {
                await ReplyAsync(MoodTools.RandomResponse("Ok...", "Thanks... I guess...", "!", "*flinches a bit, but not a lot*", "Okay then..."));
            }
            else if (-16 >= mood)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "*hiss*",
                    "Can you not?",
                    "!",
                    "What do you want? Go away.",
                    "..."
                ));
            }
            else if (mood >= 16)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "Thanks!",
                    "*(it's quiet, but there is some sound coming out)*",
                    "*quiet, happy chuckle*"
                ));
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.2);
        }

        [Command("hug"), Alias("squeeze"), Summary("attempts to hug the bot")]
        public async Task Hug()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            double mood = MoodTools.ReadMood(Context.Message.Author);

            if (-16 <= mood && mood <= 16)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "*stunned*",
                    "!",
                    "I... wasn't expecting that."
                ));
            }
            else if (-16 >= mood)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "I don't want a hug.",
                    "*awkward silence*",
                    "Uhhh... OK?",
                    "You are going to make me explode.",
                    "..."
                ));
            }
            else if (mood >= 16)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "*accepts the embrace*",
                    "*it's faint, but there is some undifferential sound*",
                    "*hugs back*"
                ));
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.4);
        }

        [Command("sex"), Alias("fuck"), Summary("attempts to sex the bot")]
        public async Task Sex()
        {
            await ReplyAsync($"Don't even think about it.");
            MoodTools.AdjustMood(Context.Message.Author, -2);
        }

        [Command("cuddle"), Alias("cud"), Summary("attempts to cuddle the bot")]
        public async Task Cuddle()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            double mood = MoodTools.ReadMood(Context.Message.Author);

            if (-16 <= mood && mood <= 16)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "Oh... um... alright...",
                    "*surprised*",
                    "We shall not coexist on the same bed.")
                );
            }
            else if (-16 >= mood)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "I don't want to cuddle with you.",
                    "Isn't this techincally *bot abuse?*",
                    "We shall not coexist on the same bed.",
                    "This isn't fine.",
                    "*makes a strange noise*")
                );
            }
            else if (mood >= 16)
            {
                await ReplyAsync(MoodTools.RandomResponse(
                    "uwu",
                    "*accepts cuddles*",
                    "*you hear a barely audible noise*")
                );
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.5);
        }

        [Command("poke"), Alias("tap"), Summary("attempts to poke the bot")]
        public async Task Poke()
        {
            await ReplyAsync(MoodTools.RandomResponse(
                "Hmm..?",
                "Stop poking me",
                "I *will* not friend you on Facebook.",
                "I swear if you poke me one more time...",
                "..."
            ));

            MoodTools.AdjustMood(Context.Message.Author, -0.1);
        }

        [Command("highfive"), Alias("high5"), Summary("attempts to highfive the bot")]
        public async Task HighFive()
        {
            await ReplyAsync(MoodTools.RandomResponse(
                "Get your hand out of my face.",
                "I will not slap your hand.",
                "I shall not.",
                "I will not talk to the hand.",
                "..."
            ));

            MoodTools.AdjustMood(Context.Message.Author, -0.01);
        }
    }
}
