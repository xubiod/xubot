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
            switch (searchterm)
            {
                case "checker":
                    {
                        if (patternID == 1) { return "10101"; }
                        if (patternID == 2) { return "01010"; }
                        if (patternID == 3) { return "10101"; }
                        if (patternID == 4) { return "01010"; }
                        if (patternID == 5) { return "10101"; }
                        return "FAILURE";
                    }
                case "carrot-r":
                    {
                        if (patternID == 1) { return "01100"; }
                        if (patternID == 2) { return "00110"; }
                        if (patternID == 3) { return "00011"; }
                        if (patternID == 4) { return "00110"; }
                        if (patternID == 5) { return "01100"; }
                        return "FAILURE";
                    }
                case "carrot-l":
                    {
                        if (patternID == 1) { return "00011"; }
                        if (patternID == 2) { return "00110"; }
                        if (patternID == 3) { return "01100"; }
                        if (patternID == 4) { return "00110"; }
                        if (patternID == 5) { return "00011"; }
                        return "FAILURE";
                    }
                case "power":
                    {
                        if (patternID == 1) { return "00100"; }
                        if (patternID == 2) { return "10101"; }
                        if (patternID == 3) { return "10101"; }
                        if (patternID == 4) { return "10001"; }
                        if (patternID == 5) { return "01110"; }
                        return "FAILURE";
                    }
                case "pico":
                    {
                        if (patternID == 1) { return "00100"; }
                        if (patternID == 2) { return "01010"; }
                        if (patternID == 3) { return "10001"; }
                        if (patternID == 4) { return "01010"; }
                        if (patternID == 5) { return "00100"; }
                        return "FAILURE";
                    }
                case "cross-hair":
                    {
                        if (patternID == 1) { return "01010"; }
                        if (patternID == 2) { return "10001"; }
                        if (patternID == 3) { return "00100"; }
                        if (patternID == 4) { return "10001"; }
                        if (patternID == 5) { return "01010"; }
                        return "FAILURE";
                    }
                case "ship":
                    {
                        if (patternID == 1) { return "00100"; }
                        if (patternID == 2) { return "01010"; }
                        if (patternID == 3) { return "11011"; }
                        if (patternID == 4) { return "11111"; }
                        if (patternID == 5) { return "10101"; }
                        return "FAILURE";
                    }
                case "smile-c":
                    {
                        if (patternID == 1) { return "00000"; }
                        if (patternID == 2) { return "01010"; }
                        if (patternID == 3) { return "00000"; }
                        if (patternID == 4) { return "10001"; }
                        if (patternID == 5) { return "01110"; }
                        return "FAILURE";
                    }
                case "smile-o":
                    {
                        if (patternID == 1) { return "00000"; }
                        if (patternID == 2) { return "01010"; }
                        if (patternID == 3) { return "00000"; }
                        if (patternID == 4) { return "11111"; }
                        if (patternID == 5) { return "01110"; }
                        return "FAILURE";
                    }
                case "sad-c":
                    {
                        if (patternID == 1) { return "00000"; }
                        if (patternID == 2) { return "01010"; }
                        if (patternID == 3) { return "00000"; }
                        if (patternID == 4) { return "01110"; }
                        if (patternID == 5) { return "10001"; }
                        return "FAILURE";
                    }
                case "sad-o":
                    {
                        if (patternID == 1) { return "00000"; }
                        if (patternID == 2) { return "01010"; }
                        if (patternID == 3) { return "00000"; }
                        if (patternID == 4) { return "01110"; }
                        if (patternID == 5) { return "11111"; }
                        return "FAILURE";
                    }
                case "spade":
                    {
                        if (patternID == 1) { return "00100"; }
                        if (patternID == 2) { return "01110"; }
                        if (patternID == 3) { return "11111"; }
                        if (patternID == 4) { return "11111"; }
                        if (patternID == 5) { return "00100"; }
                        return "FAILURE";
                    }
                case "club":
                    {
                        if (patternID == 1) { return "11011"; }
                        if (patternID == 2) { return "11011"; }
                        if (patternID == 3) { return "00100"; }
                        if (patternID == 4) { return "01011"; }
                        if (patternID == 5) { return "10011"; }
                        return "FAILURE";
                    }
                case "heart":
                    {
                        if (patternID == 1) { return "01010"; }
                        if (patternID == 2) { return "11111"; }
                        if (patternID == 3) { return "11111"; }
                        if (patternID == 4) { return "01110"; }
                        if (patternID == 5) { return "00100"; }
                        return "FAILURE";
                    }
                case "diamond":
                    {
                        if (patternID == 1) { return "00100"; }
                        if (patternID == 2) { return "01110"; }
                        if (patternID == 3) { return "11111"; }
                        if (patternID == 4) { return "01110"; }
                        if (patternID == 5) { return "00100"; }
                        return "FAILURE";
                    }
                case "eighth-note":
                    {
                        if (patternID == 1) { return "00110"; }
                        if (patternID == 2) { return "00100"; }
                        if (patternID == 3) { return "00100"; }
                        if (patternID == 4) { return "01100"; }
                        if (patternID == 5) { return "01100"; }
                        return "FAILURE";
                    }
                case "sixteenth-note":
                    {
                        if (patternID == 1) { return "00110"; }
                        if (patternID == 2) { return "00110"; }
                        if (patternID == 3) { return "00100"; }
                        if (patternID == 4) { return "01100"; }
                        if (patternID == 5) { return "01100"; }
                        return "FAILURE";
                    }
                case "double-eighth-note":
                    {
                        if (patternID == 1) { return "01111"; }
                        if (patternID == 2) { return "01001"; }
                        if (patternID == 3) { return "01001"; }
                        if (patternID == 4) { return "11011"; }
                        if (patternID == 5) { return "11011"; }
                        return "FAILURE";
                    }
                case "double-sixteenth-note":
                    {
                        if (patternID == 1) { return "01111"; }
                        if (patternID == 2) { return "01111"; }
                        if (patternID == 3) { return "01001"; }
                        if (patternID == 4) { return "11011"; }
                        if (patternID == 5) { return "11011"; }
                        return "FAILURE";
                    }
                case "hammer":
                    {
                        if (patternID == 1) { return "01101"; }
                        if (patternID == 2) { return "11111"; }
                        if (patternID == 3) { return "10101"; }
                        if (patternID == 4) { return "00100"; }
                        if (patternID == 5) { return "00100"; }
                        return "FAILURE";
                    }
                case "paper":
                    {
                        if (patternID == 1) { return "11111"; }
                        if (patternID == 2) { return "10001"; }
                        if (patternID == 3) { return "11111"; }
                        if (patternID == 4) { return "10001"; }
                        if (patternID == 5) { return "11111"; }
                        return "FAILURE";
                    }
                default: return "FAILURE";
            }
        }
    }
}
