using RedditSharp;
using Discord.Commands;
using RedditSharp.Things;

namespace xubot
{
    class RedditGetFunct : ModuleBase
    {
        public class ParseSorting
        {
            public static Sorting FromString(string sorting)
            {
                if (sorting.ToLower() == "relevance" || sorting.ToLower() == "relevant") { return Sorting.Relevance; }
                else if (sorting.ToLower() == "new") { return Sorting.New; }
                else if (sorting.ToLower() == "top") { return Sorting.Top; }
                else if (sorting.ToLower() == "comments") { return Sorting.Comments; }
                else { return Sorting.New; }
            }

            public static Sorting FromInt(int sorting)
            {
                if (sorting == 0) { return Sorting.Relevance; }
                else if (sorting == 1) { return Sorting.New; }
                else if (sorting == 2) { return Sorting.Top; }
                else if (sorting == 3) { return Sorting.Comments; }
                else { return Sorting.New; }
            }

            public static Subreddit.Sort FromIntSort(int sorting)
            {
                if (sorting == 0) { return Subreddit.Sort.Rising; }
                else if (sorting == 1) { return Subreddit.Sort.New; }
                else if (sorting == 2) { return Subreddit.Sort.Top; }
                else if (sorting == 3) { return Subreddit.Sort.Controversial; }
                else { return Subreddit.Sort.New; }
            }

            public static int StringToInt(string sorting)
            {
                if (sorting.ToLower() == "relevance" || sorting.ToLower() == "relevant") { return 0; }
                else if (sorting.ToLower() == "new") { return 1; }
                else if (sorting.ToLower() == "top") { return 2; }
                else if (sorting.ToLower() == "comments") { return 3; }
                else { return 1; }
            }

        }
    }
}
