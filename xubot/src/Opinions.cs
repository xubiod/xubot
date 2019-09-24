using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Discord;
using System.Xml.Linq;

namespace xubot
{
    public class Opinions : ModuleBase
    {
        [Command("opinion")]
        public async Task opinion(string input)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;
            Random _reply_decide = new Random();
            
            string reply = "";
            
            switch (_reply_decide.Next(3)) {
                case 0: reply = "I don't have an opinion on that yet."; break;
                case 1: reply = "I got no opinion on that yet."; break;
                case 2: reply = "I either don't know what that is, or I just don't have an opinion."; break;
                case 3: reply = "No opinion for this yet."; break;
            }
            
            var xdoc = XDocument.Load("Opinions.xml");

            var items = from i in xdoc.Descendants("opinion")
                        select new
                        {
                            Attribute = (string)i.Attribute("on"),
                            i.Value
                        };

            foreach (var item in items)
            {
                if (item.Attribute.ToLower() == input.ToLower())
                {
                    reply = item.Value;
                }
            }

            await ReplyAsync(reply);
        }
    }
}
