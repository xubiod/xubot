using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot_core.src
{
    class Pattern_Presets
    {
        public static string Return_Query(string searchterm, int patternID)
        {
            if (searchterm.Contains("checker"))
            {
                if (patternID == 1) { return "10101"; }
                else if (patternID == 2) { return "01010"; }
                else if (patternID == 3) { return "10101"; }
                else if (patternID == 4) { return "01010"; }
                else if (patternID == 5) { return "10101"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("carrot-r"))
            {
                if (patternID == 1) { return "01100"; }
                else if (patternID == 2) { return "00110"; }
                else if (patternID == 3) { return "00011"; }
                else if (patternID == 4) { return "00110"; }
                else if (patternID == 5) { return "01100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("carrot-l"))
            {
                if (patternID == 1) { return "00011"; }
                else if (patternID == 2) { return "00110"; }
                else if (patternID == 3) { return "01100"; }
                else if (patternID == 4) { return "00110"; }
                else if (patternID == 5) { return "00011"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("power"))
            {
                if (patternID == 1) { return "00100"; }
                else if (patternID == 2) { return "10101"; }
                else if (patternID == 3) { return "10101"; }
                else if (patternID == 4) { return "10001"; }
                else if (patternID == 5) { return "01110"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("pico"))
            {
                if (patternID == 1) { return "00100"; }
                else if (patternID == 2) { return "01010"; }
                else if (patternID == 3) { return "10001"; }
                else if (patternID == 4) { return "01010"; }
                else if (patternID == 5) { return "00100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("power"))
            {
                if (patternID == 1) { return "00100"; }
                else if (patternID == 2) { return "00110"; }
                else if (patternID == 3) { return "01100"; }
                else if (patternID == 4) { return "00110"; }
                else if (patternID == 5) { return "00011"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("cross-hair"))
            {
                if (patternID == 1) { return "01010"; }
                else if (patternID == 2) { return "10001"; }
                else if (patternID == 3) { return "00100"; }
                else if (patternID == 4) { return "10001"; }
                else if (patternID == 5) { return "01010"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("ship"))
            {
                if (patternID == 1) { return "00100"; }
                else if (patternID == 2) { return "01010"; }
                else if (patternID == 3) { return "11011"; }
                else if (patternID == 4) { return "11111"; }
                else if (patternID == 5) { return "10101"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("smile-c"))
            {
                if (patternID == 1) { return "00000"; }
                else if (patternID == 2) { return "01010"; }
                else if (patternID == 3) { return "00000"; }
                else if (patternID == 4) { return "10001"; }
                else if (patternID == 5) { return "01110"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("smile-o"))
            {
                if (patternID == 1) { return "00000"; }
                else if (patternID == 2) { return "01010"; }
                else if (patternID == 3) { return "00000"; }
                else if (patternID == 4) { return "11111"; }
                else if (patternID == 5) { return "01110"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("sad-c"))
            {
                if (patternID == 1) { return "00000"; }
                else if (patternID == 2) { return "01010"; }
                else if (patternID == 3) { return "00000"; }
                else if (patternID == 4) { return "01110"; }
                else if (patternID == 5) { return "10001"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("sad-o"))
            {
                if (patternID == 1) { return "00000"; }
                else if (patternID == 2) { return "01010"; }
                else if (patternID == 3) { return "00000"; }
                else if (patternID == 4) { return "01110"; }
                else if (patternID == 5) { return "11111"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("spade"))
            {
                if (patternID == 1) { return "00100"; }
                else if (patternID == 2) { return "01110"; }
                else if (patternID == 3) { return "11111"; }
                else if (patternID == 4) { return "11111"; }
                else if (patternID == 5) { return "00100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("club"))
            {
                if (patternID == 1) { return "11011"; }
                else if (patternID == 2) { return "11011"; }
                else if (patternID == 3) { return "00100"; }
                else if (patternID == 4) { return "01011"; }
                else if (patternID == 5) { return "10011"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("heart"))
            {
                if (patternID == 1) { return "01010"; }
                else if (patternID == 2) { return "11111"; }
                else if (patternID == 3) { return "11111"; }
                else if (patternID == 4) { return "01110"; }
                else if (patternID == 5) { return "00100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("diamond"))
            {
                if (patternID == 1) { return "00100"; }
                else if (patternID == 2) { return "01110"; }
                else if (patternID == 3) { return "11111"; }
                else if (patternID == 4) { return "01110"; }
                else if (patternID == 5) { return "00100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("eighth-note"))
            {
                if (patternID == 1) { return "00110"; }
                else if (patternID == 2) { return "00100"; }
                else if (patternID == 3) { return "00100"; }
                else if (patternID == 4) { return "01100"; }
                else if (patternID == 5) { return "01100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("sixteenth-note"))
            {
                if (patternID == 1) { return "00110"; }
                else if (patternID == 2) { return "00110"; }
                else if (patternID == 3) { return "00100"; }
                else if (patternID == 4) { return "01100"; }
                else if (patternID == 5) { return "01100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("double-eighth-note"))
            {
                if (patternID == 1) { return "01111"; }
                else if (patternID == 2) { return "01001"; }
                else if (patternID == 3) { return "01001"; }
                else if (patternID == 4) { return "11011"; }
                else if (patternID == 5) { return "11011"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("double-sixteenth-note"))
            {
                if (patternID == 1) { return "01111"; }
                else if (patternID == 2) { return "01111"; }
                else if (patternID == 3) { return "01001"; }
                else if (patternID == 4) { return "11011"; }
                else if (patternID == 5) { return "11011"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("hammer"))
            {
                if (patternID == 1) { return "01101"; }
                else if (patternID == 2) { return "11111"; }
                else if (patternID == 3) { return "10101"; }
                else if (patternID == 4) { return "00100"; }
                else if (patternID == 5) { return "00100"; }
                else { return "FAILURE"; }
            }
            else if (searchterm.Contains("paper"))
            {
                if (patternID == 1) { return "11111"; }
                else if (patternID == 2) { return "10001"; }
                else if (patternID == 3) { return "11111"; }
                else if (patternID == 4) { return "10001"; }
                else if (patternID == 5) { return "11111"; }
                else { return "FAILURE"; }
            }
            else { return "FAILURE"; }
        }
    }
}
