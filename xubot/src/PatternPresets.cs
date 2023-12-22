using System.Collections.Generic;

namespace xubot
{
    internal class PatternPresets
    {
        private static readonly Dictionary<string, string[]> Patterns = new() {
                { "checker",                new[]{ "10101", "01010", "01010", "01010", "10101" } },
                { "carrot-r",               new[]{ "01100", "00110", "00011", "00110", "01100" } },
                { "carrot-l",               new[]{ "00011", "00110", "01100", "00110", "00011" } },
                { "power",                  new[]{ "00100", "10101", "10101", "10001", "01110" } },
                { "pico",                   new[]{ "00100", "01010", "10001", "01010", "00100" } },
                { "cross-hair",             new[]{ "01010", "10001", "00100", "10001", "01010" } },
                { "ship",                   new[]{ "00100", "01010", "11011", "11111", "10101" } },
                { "smile-c",                new[]{ "00000", "01010", "00000", "10001", "01110" } },
                { "smile-o",                new[]{ "00000", "01010", "00000", "11111", "01110" } },
                { "sad-c",                  new[]{ "00000", "01010", "00000", "01110", "10001" } },
                { "sad-o",                  new[]{ "00000", "01010", "00000", "01110", "11111" } },
                { "spade",                  new[]{ "00100", "01110", "11111", "11111", "00100" } },
                { "club",                   new[]{ "11011", "11011", "00100", "01011", "10011" } },
                { "heart",                  new[]{ "01010", "11111", "11111", "01110", "00100" } },
                { "diamond",                new[]{ "00100", "01110", "11111", "01110", "00100" } },
                { "eighth-note",            new[]{ "00110", "00100", "00100", "01100", "01100" } },
                { "sixteenth-note",         new[]{ "00110", "00110", "00100", "01100", "01100" } },
                { "double-eighth-note",     new[]{ "01111", "01001", "01001", "11011", "11011" } },
                { "double-sixteenth-note",  new[]{ "01111", "01111", "01001", "11011", "11011" } },
                { "hammer",                 new[]{ "01101", "11111", "10101", "00100", "00100" } },
                { "paper",                  new[]{ "11111", "10001", "11111", "10001", "11111" } } };

        public static string ReturnQuery(string searchTerm, int patternId)
        {
            var index = (patternId - 1) % 5;
            return Patterns.ContainsKey(searchTerm) ? Patterns[searchTerm][index] : "FAILURE";
        }
    }
}
