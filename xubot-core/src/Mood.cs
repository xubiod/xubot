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
        public async Task getMoodCMD()
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
        public async Task pet()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            double mood = MoodTools.ReadMood(Context.Message.Author);

            if (-16 <= mood && mood <= 16)
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"Ok..."); }
                else if (o == 1) { await ReplyAsync($"Thanks... I guess..."); }
                else if (o == 2) { await ReplyAsync($"!"); }
                else if (o == 3) { await ReplyAsync($"*flinches a bit, but not a lot*"); }
                else if (o == 4) { await ReplyAsync($"Okay then..."); }
            }
            else if (-16 >= mood)
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"*hiss*"); }
                else if (o == 1) { await ReplyAsync($"Can you not?"); }
                else if (o == 2) { await ReplyAsync($"What do you want? Go away."); }
                else if (o == 3) { await ReplyAsync($"Your *pet* has been ignored."); }
                else if (o == 4) { await ReplyAsync($"..."); }
            }
            else if (mood >= 16)
            {
                Random rand = new Random();
                int o = rand.Next(3);
                if (o == 0) { await ReplyAsync($"Thanks!"); }
                else if (o == 1) { await ReplyAsync($"*(it's quiet, but there is some sound coming out)*"); }
                else if (o == 2) { await ReplyAsync($"*quiet, happy chuckle*"); }
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.2);
        }

        [Command("hug"), Alias("squeeze"), Summary("attempts to hug the bot")]
        public async Task hug()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            double mood = MoodTools.ReadMood(Context.Message.Author);

            if (-16 <= mood && mood <= 16)
            {
                Random rand = new Random();
                int o = rand.Next(3);
                if (o == 0) { await ReplyAsync($"*stunned*"); }
                else if (o == 1) { await ReplyAsync($"!"); }
                else if (o == 2) { await ReplyAsync($"I... wasn't expecting that."); }
            }
            else if (-16 >= mood)
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"I don't want a hug."); }
                else if (o == 1) { await ReplyAsync($"*awkward silence*"); }
                else if (o == 2) { await ReplyAsync($"Uhhh... OK?"); }
                else if (o == 3) { await ReplyAsync($"You are going to make me explode."); }
                else if (o == 4) { await ReplyAsync($"..."); }
            }
            else if (mood >= 16)
            {
                Random rand = new Random();
                int o = rand.Next(3);
                if (o == 0) { await ReplyAsync($"*accepts the embrace*"); }
                else if (o == 1) { await ReplyAsync($"*it's faint, but there is some undifferential sound*"); }
                else if (o == 2) { await ReplyAsync($"*hugs back*"); }
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.4);
        }

        [Command("sex"), Alias("fuck"), Summary("attempts to sex the bot")]
        public async Task sex()
        {
            await ReplyAsync($"Don't even think about it.");
            MoodTools.AdjustMood(Context.Message.Author, -2);
        }

        [Command("cuddle"), Alias("cud"), Summary("attempts to cuddle the bot")]
        public async Task cuddle()
        {
            MoodTools.AddOrRefreshMood(Context.Message.Author);

            double mood = MoodTools.ReadMood(Context.Message.Author);

            if (-16 <= mood && mood <= 16)
            {
                Random rand = new Random();
                int o = rand.Next(3);
                if (o == 0) { await ReplyAsync($"Oh... um... alright..."); }
                else if (o == 1) { await ReplyAsync($"*surprised*"); }
                else if (o == 2) { await ReplyAsync($"Uh..."); }
            }
            else if (-16 >= mood)
            {
                Random rand = new Random();
                int o = rand.Next(5);
                if (o == 0) { await ReplyAsync($"I don't want to cuddle with you."); }
                else if (o == 1) { await ReplyAsync($"Isn't this techincally *bot abuse?*"); }
                else if (o == 2) { await ReplyAsync($"We shall not coexist on the same bed."); }
                else if (o == 3) { await ReplyAsync($"This isn't fine."); }
                else if (o == 4) { await ReplyAsync($"*makes a strange noise*"); }
            }
            else if (mood >= 16)
            {
                Random rand = new Random();
                int o = rand.Next(3);
                if (o == 0) { await ReplyAsync($"OwO"); }
                else if (o == 1) { await ReplyAsync($"*accepts cuddles*"); }
                else if (o == 2) { await ReplyAsync($"*you hear a barely audible noise*"); }
            }

            MoodTools.AdjustMood(Context.Message.Author, 0.5);
        }

        [Command("poke"), Alias("tap"), Summary("attempts to poke the bot")]
        public async Task poke()
        {
            Random rand = new Random();
            int o = rand.Next(5);
            if (o == 0) { await ReplyAsync($"Hmm..?"); }
            else if (o == 1) { await ReplyAsync($"Stop poking me."); }
            else if (o == 2) { await ReplyAsync($"I *will* not friend you on Facebook."); }
            else if (o == 3) { await ReplyAsync($"I swear if you poke me one more time..."); }
            else if (o == 4) { await ReplyAsync($"..."); }

            MoodTools.AdjustMood(Context.Message.Author, -0.1);
        }

        [Command("highfive"), Alias("high5"), Summary("attempts to highfive the bot")]
        public async Task highfive()
        {
            Random rand = new Random();
            int o = rand.Next(5);
            if (o == 0) { await ReplyAsync($"Get your hand out of my face."); }
            else if (o == 1) { await ReplyAsync($"I will not slap your hand."); }
            else if (o == 2) { await ReplyAsync($"I shall not."); }
            else if (o == 3) { await ReplyAsync($"I will not talk to the hand."); }
            else if (o == 4) { await ReplyAsync($"..."); }
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
