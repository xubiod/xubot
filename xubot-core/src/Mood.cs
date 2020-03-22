using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.Xml.Linq;

namespace xubot_core.src
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
                switch (new Random().Next(5))
                {
                    case 0: await ReplyAsync($"Ok..."); break;
                    case 1: await ReplyAsync($"Thanks... I guess..."); break;
                    case 2: await ReplyAsync($"!"); break;
                    case 3: await ReplyAsync($"*flinches a bit, but not a lot*"); break;
                    case 4: await ReplyAsync($"Okay then..."); break;
                }
            }
            else if (-16 >= mood)
            {
                switch (new Random().Next(5))
                {
                    case 0: await ReplyAsync($"*hiss*"); break;
                    case 1: await ReplyAsync($"Can you not?"); break;
                    case 2: await ReplyAsync($"!"); break;
                    case 3: await ReplyAsync($"What do you want? Go away."); break;
                    case 4: await ReplyAsync($"..."); break;
                }
            }
            else if (mood >= 16)
            {
                switch (new Random().Next(3))
                {
                    case 0: await ReplyAsync($"Thanks!"); break;
                    case 1: await ReplyAsync($"*(it's quiet, but there is some sound coming out)*"); break;
                    case 2: await ReplyAsync($"*quiet, happy chuckle*"); break;
                }
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
                switch (new Random().Next(3))
                {
                    case 0: await ReplyAsync($"*stunned*"); break;
                    case 1: await ReplyAsync($"!"); break;
                    case 2: await ReplyAsync($"I... wasn't expecting that."); break;
                }
            }
            else if (-16 >= mood)
            {
                switch (new Random().Next(5))
                {
                    case 0: await ReplyAsync($"I don't want a hug."); break;
                    case 1: await ReplyAsync($"*awkward silence*"); break;
                    case 2: await ReplyAsync($"Uhhh... OK?"); break;
                    case 3: await ReplyAsync($"You are going to make me explode."); break;
                    case 4: await ReplyAsync($"..."); break;
                }
            }
            else if (mood >= 16)
            {
                switch (new Random().Next(3))
                {
                    case 0: await ReplyAsync($"*accepts the embrace*"); break;
                    case 1: await ReplyAsync($"*it's faint, but there is some undifferential sound*"); break;
                    case 2: await ReplyAsync($"*hugs back*"); break;
                }
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
                switch (new Random().Next(3))
                {
                    case 0: await ReplyAsync($"Oh... um... alright..."); break;
                    case 1: await ReplyAsync($"*surprised*"); break;
                    case 2: await ReplyAsync($"Uh..."); break;
                }
            }
            else if (-16 >= mood)
            {
                switch (new Random().Next(5))
                {
                    case 0: await ReplyAsync($"I don't want to cuddle with you."); break;
                    case 1: await ReplyAsync($"Isn't this techincally *bot abuse?*"); break;
                    case 2: await ReplyAsync($"We shall not coexist on the same bed."); break;
                    case 3: await ReplyAsync($"This isn't fine."); break;
                    case 4: await ReplyAsync($"*makes a strange noise*"); break;
                }
            }
            else if (mood >= 16)
            {
                switch (new Random().Next(3))
                {
                    case 0: await ReplyAsync($"uwu"); break;
                    case 1: await ReplyAsync($"*accepts cuddles*"); break;
                    case 2: await ReplyAsync($"*you hear a barely audible noise*"); break;
                }
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.5);
        }

        [Command("poke"), Alias("tap"), Summary("attempts to poke the bot")]
        public async Task Poke()
        {
            switch (new Random().Next(5))
            {
                case 0: await ReplyAsync($"Hmm..?"); break;
                case 1: await ReplyAsync($"Stop poking me"); break;
                case 2: await ReplyAsync($"I *will* not friend you on Facebook."); break;
                case 3: await ReplyAsync($"I swear if you poke me one more time..."); break;
                case 4: await ReplyAsync($"..."); break;
            }

            MoodTools.AdjustMood(Context.Message.Author, -0.1);
        }

        [Command("highfive"), Alias("high5"), Summary("attempts to highfive the bot")]
        public async Task HighFive()
        {
            switch (new Random().Next(5))
            {
                case 0: await ReplyAsync($"Get your hand out of my face."); break;
                case 1: await ReplyAsync($"I will not slap your hand."); break;
                case 2: await ReplyAsync($"I shall not."); break;
                case 3: await ReplyAsync($"I will not talk to the hand."); break;
                case 4: await ReplyAsync($"..."); break;
            }

            MoodTools.AdjustMood(Context.Message.Author, -0.01);
        }
    }

    public class MoodTools : ModuleBase
    {
        public static void AddOrRefreshMood(IUser arg)
        {
            bool exists = false;

            Mood.xdoc = XDocument.Load("Moods.xml");

            var items = from i in Mood.xdoc.Descendants("mood")
                        select new
                        {
                            user = i.Attribute("user")
                        };

            foreach (var item in items)
            {
                if (item.user.Value == arg.Id.ToString())
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                Console.WriteLine("new user found to add to mood, doing that now");

                XElement xelm = new XElement("mood");
                XAttribute user = new XAttribute("user", arg.Id.ToString());
                XAttribute moodval = new XAttribute("moodvalue", "0");

                xelm.Add(user);
                xelm.Add(moodval);

                Mood.xdoc.Root.Add(xelm);
                Mood.xdoc.Save("Moods.xml");
            }
        }

        public static double ReadMood(IUser arg)
        {
            Mood.xdoc = XDocument.Load("Moods.xml");

            var items = from i in Mood.xdoc.Descendants("mood")
                        select new
                        {
                            user = i.Attribute("user"),
                            moodval = i.Attribute("moodvalue")
                        };

            foreach (var item in items)
            {
                if (item.user.Value == arg.Id.ToString())
                {
                    return Convert.ToDouble(item.moodval.Value);
                }
            }

            return 0;
        }

        public static void AdjustMood(IUser arg, double adjust)
        {
            Mood.xdoc = XDocument.Load("Moods.xml");

            var items = from i in Mood.xdoc.Descendants("mood")
                        select new
                        {
                            user = i.Attribute("user"),
                            moodval = i.Attribute("moodvalue")
                        };

            foreach (var item in items)
            {
                if (item.user.Value == arg.Id.ToString())
                {
                    item.moodval.Value = (Convert.ToDouble(item.moodval.Value) + adjust).ToString();
                }
            }

            Mood.xdoc.Save("Moods.xml");
        }
    }
}
