using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    class PatternPresets
    {
        private static readonly Dictionary<string, string[]> Patterns = new Dictionary<string, string[]>() {
                { "checker",                new string[]{ "10101", "01010", "01010", "01010", "10101" } },
                { "carrot-r",               new string[]{ "01100", "00110", "00011", "00110", "01100" } },
                { "carrot-l",               new string[]{ "00011", "00110", "01100", "00110", "00011" } },
                { "power",                  new string[]{ "00100", "10101", "10101", "10001", "01110" } },
                { "pico",                   new string[]{ "00100", "01010", "10001", "01010", "00100" } },
                { "cross-hair",             new string[]{ "01010", "10001", "00100", "10001", "01010" } },
                { "ship",                   new string[]{ "00100", "01010", "11011", "11111", "10101" } },
                { "smile-c",                new string[]{ "00000", "01010", "00000", "10001", "01110" } },
                { "smile-o",                new string[]{ "00000", "01010", "00000", "11111", "01110" } },
                { "sad-c",                  new string[]{ "00000", "01010", "00000", "01110", "10001" } },
                { "sad-o",                  new string[]{ "00000", "01010", "00000", "01110", "11111" } },
                { "spade",                  new string[]{ "00100", "01110", "11111", "11111", "00100" } },
                { "club",                   new string[]{ "11011", "11011", "00100", "01011", "10011" } },
                { "heart",                  new string[]{ "01010", "11111", "11111", "01110", "00100" } },
                { "diamond",                new string[]{ "00100", "01110", "11111", "01110", "00100" } },
                { "eighth-note",            new string[]{ "00110", "00100", "00100", "01100", "01100" } },
                { "sixteenth-note",         new string[]{ "00110", "00110", "00100", "01100", "01100" } },
                { "double-eighth-note",     new string[]{ "01111", "01001", "01001", "11011", "11011" } },
                { "double-sixteenth-note",  new string[]{ "01111", "01111", "01001", "11011", "11011" } },
                { "hammer",                 new string[]{ "01101", "11111", "10101", "00100", "00100" } },
                { "paper",                  new string[]{ "11111", "10001", "11111", "10001", "11111" } } };

        public static string ReturnQuery(string searchterm, int patternId)
        {
            int index = (patternId - 1) % 5;
            return Patterns.ContainsKey(searchterm) ? (Patterns[searchterm])[index] : "FAILURE";
        }
    }
}
