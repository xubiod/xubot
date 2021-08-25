using RedditSharp;
using RedditSharp.Things;

namespace xubot
{
    internal class RedditTools
    {
        public class ParseSorting
        {
            public static Sorting FromString(string sorting)
            {
                return sorting switch
                {
                    "relevance" => Sorting.Relevance,
                    "relevant" => Sorting.Relevance,
                    "new" => Sorting.New,
                    "top" => Sorting.Top,
                    "comments" => Sorting.Comments,
                    _ => Sorting.New
                };
            }

            public static Sorting FromInt(int sorting)
            {
                return sorting switch
                {
                    0 => Sorting.Relevance,
                    1 => Sorting.New,
                    2 => Sorting.Top,
                    3 => Sorting.Comments,
                    _ => Sorting.New
                };
            }

            public static Subreddit.Sort FromIntSort(int sorting)
            {
                return sorting switch
                {
                    0 => Subreddit.Sort.Rising,
                    1 => Subreddit.Sort.New,
                    2 => Subreddit.Sort.Top,
                    3 => Subreddit.Sort.Controversial,
                    _ => Subreddit.Sort.New
                };
            }

            public static int StringToInt(string sorting)
            {
                return sorting switch
                {
                    "relevance" => 0,
                    "relevant" => 0,
                    "new" => 1,
                    "top" => 2,
                    "comments" => 3,
                    _ => 1
                };
            }
        }
    }
}
