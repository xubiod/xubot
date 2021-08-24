using Discord;
using Newtonsoft.Json.Linq;

namespace xubot.Commands.Global
{
    public class MoodTools
    {
        public static void AddOrRefreshMood(IUser arg)
        {
            if (!(Program.JsonKeys["mood"].Contents as JObject).ContainsKey(arg.Id.ToString()))
            {
                (Program.JsonKeys["mood"].Contents as JObject).Add(arg.Id.ToString(), 0);
                Util.Json.SaveKeyAsJson("mood");
            }
        }

        public static double ReadMood(IUser arg)
        {
            return (Program.JsonKeys["mood"].Contents as JObject).Value<double>(arg.Id.ToString());
        }

        public static void AdjustMood(IUser arg, double adjust)
        {
            (Program.JsonKeys["mood"].Contents as JObject)[arg.Id.ToString()] = ReadMood(arg) + adjust;
            Util.Json.SaveKeyAsJson("mood");
        }

        public static string RandomResponse(params string[] any)
        {
            return any[Util.Globals.Rng.Next(any.Length)];
        }
    }
}
