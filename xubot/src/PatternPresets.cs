using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src
{
    class PatternPresets
    {
        public static string ReturnQuery(string searchterm, int patternID)
        {
            int index = (patternID - 1) % 5;
            switch (searchterm)
            {
                case "checker":
                    return new string[] { "10101", "01010", "01010", "01010", "10101" }[index];
                case "carrot-r":
                    return new string[] { "01100", "00110", "00011", "00110", "01100" }[index];
                case "carrot-l":
                    return new string[] { "00011", "00110", "01100", "00110", "00011" }[index];
                case "power":
                    return new string[] { "00100", "10101", "10101", "10001", "01110" }[index];
                case "pico":
                    return new string[] { "00100", "01010", "10001", "01010", "00100" }[index];
                case "cross-hair":
                    return new string[] { "01010", "10001", "00100", "10001", "01010" }[index];
                case "ship":
                    return new string[] { "00100", "01010", "11011", "11111", "10101" }[index];
                case "smile-c":
                    return new string[] { "00000", "01010", "00000", "10001", "01110" }[index];
                case "smile-o":
                    return new string[] { "00000", "01010", "00000", "11111", "01110" }[index];
                case "sad-c":
                    return new string[] { "00000", "01010", "00000", "01110", "10001" }[index];
                case "sad-o":
                    return new string[] { "00000", "01010", "00000", "01110", "11111" }[index];
                case "spade":
                    return new string[] { "00100", "01110", "11111", "11111", "00100" }[index];
                case "club":
                    return new string[] { "11011", "11011", "00100", "01011", "10011" }[index];
                case "heart":
                    return new string[] { "01010", "11111", "11111", "01110", "00100" }[index];
                case "diamond":
                    return new string[] { "00100", "01110", "11111", "01110", "00100" }[index];
                case "eighth-note":
                    return new string[] { "00110", "00100", "00100", "01100", "01100" }[index];
                case "sixteenth-note":
                    return new string[] { "00110", "00110", "00100", "01100", "01100" }[index];
                case "double-eighth-note":
                    return new string[] { "01111", "01001", "01001", "11011", "11011" }[index];
                case "double-sixteenth-note":
                    return new string[] { "01111", "01111", "01001", "11011", "11011" }[index];
                case "hammer":
                    return new string[] { "01101", "11111", "10101", "00100", "00100" }[index];
                case "paper":
                    return new string[] { "11111", "10001", "11111", "10001", "11111" }[index];
                default: return "FAILURE";
            }
        }
    }
}
