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

            string reply = "I don't have an opinion on that yet.";

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
