using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Commands.Global
{
    public class MoodTools
    {
        public static void AddOrRefreshMood(IUser arg)
        {
            if (!(Program.JSONKeys["mood"].Contents as JObject).ContainsKey(arg.Id.ToString()))
            {
                (Program.JSONKeys["mood"].Contents as JObject).Add(arg.Id.ToString(), 0);
                Util.JSON.SaveKeyAsJSON("mood");
            }
        }

        public static double ReadMood(IUser arg)
        {
            return (Program.JSONKeys["mood"].Contents as JObject).Value<double>(arg.Id.ToString());
        }

        public static void AdjustMood(IUser arg, double adjust)
        {
            (Program.JSONKeys["mood"].Contents as JObject)[arg.Id.ToString()] = ReadMood(arg) + adjust;
            Util.JSON.SaveKeyAsJSON("mood");
        }

        public static string RandomResponse(params string[] any)
        {
            return any[Util.Globals.RNG.Next(any.Length)];
        }
    }
}
