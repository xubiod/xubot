using System.Threading.Tasks;
using Discord.Commands;

namespace xubot.Commands.Global
{
    public class Mood : ModuleBase
    {
/*
        public static XDocument xDocument = new();
*/

        [Command("mood"), Summary("Get's your mood/emotion value from xubot.")]
        public async Task GetMoodCmd()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);
            var mood = MoodTools.ReadMood(Context.Message.Author);

            var moodAsStr = mood switch
            {
                >= -16 and <= 16 => "neutral",
                <= -16 => "negative",
                >= 16 => "positive",
                _ => "invalid"
            };

            await ReplyAsync($"Your mood value is {mood} (aka {moodAsStr})");
        }

        [Command("pet"), Alias("stroke"), Summary("why.")]
        public async Task Pet()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            var mood = MoodTools.ReadMood(Context.Message.Author);

            switch (mood)
            {case <= -16:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "*hiss*",
                        "Can you not?",
                        "!",
                        "What do you want? Go away.",
                        "..."
                    ));
                    break;
                case >= 16:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "Thanks!",
                        "*(it's quiet, but there is some sound coming out)*",
                        "*quiet, happy chuckle*"
                    ));
                    break;
                // case >= -16 and <= 16:
                default:
                    await ReplyAsync(MoodTools.RandomResponse("Ok...", "Thanks... I guess...", "!", "*flinches a bit, but not a lot*", "Okay then..."));
                    break;

            }

            MoodTools.AdjustMood(Context.Message.Author, 0.2);
        }

        [Command("hug"), Alias("squeeze"), Summary("attempts to hug the bot")]
        public async Task Hug()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            var mood = MoodTools.ReadMood(Context.Message.Author);

            switch (mood)
            {
                case <= -16:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "I don't want a hug.",
                        "*awkward silence*",
                        "Uhhh... OK?",
                        "You are going to make me explode.",
                        "..."
                    ));
                    break;
                case >= 16:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "*accepts the embrace*",
                        "*it's faint, but there is some non-differentiable sound*",
                        "*hugs back*"
                    ));
                    break;
                // case >= -16 and <= 16:
                default:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "*stunned*",
                        "!",
                        "I... wasn't expecting that."
                    ));
                    break;
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.4);
        }

        [Command("sex"), Alias("fuck"), Summary("attempts to sex the bot")]
        public async Task Sex()
        {
            await ReplyAsync("Don't even think about it.");
            MoodTools.AdjustMood(Context.Message.Author, -2);
        }

        [Command("cuddle"), Alias("cud"), Summary("attempts to cuddle the bot")]
        public async Task Cuddle()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            var mood = MoodTools.ReadMood(Context.Message.Author);

            switch (mood)
            {
                case <= -16:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "I don't want to cuddle with you.",
                        "Isn't this technically *bot abuse?*",
                        "We shall not coexist on the same bed.",
                        "This isn't fine.",
                        "*makes a strange noise*")
                    );
                    break;
                case >= 16:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "uwu",
                        "*accepts cuddles*",
                        "*you hear a barely audible noise*")
                    );
                    break;
                // case >= -16 and <= 16:
                default:
                    await ReplyAsync(MoodTools.RandomResponse(
                        "Oh... um... alright...",
                        "*surprised*",
                        "We shall not coexist on the same bed.")
                    );
                    break;
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

        [Command("highfive"), Alias("high5"), Summary("attempts to high-five the bot")]
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
