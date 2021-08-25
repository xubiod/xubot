using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Discord;
using Discord.Commands;
using xubot.Attributes;

namespace xubot.Commands.Global
{
    public class ServerTriggers : ModuleBase
    {
        public static XDocument xDocument = new();

        [Group("servertriggers"), Summary("Server specific triggers? Wow that sounds hella lame"), Deprecated]
        public class Base : ModuleBase
        {
            [Command("add"), RequireUserPermission(GuildPermission.ManageGuild)]
            public async Task AddTrigger(string onWake = "", bool nsfwOverride = false, bool useMarkov = false)
            {
                //[>servertriggers add "onwake msg" true
                bool exist = false;
                xDocument = XDocument.Load("PerServTrigg.xml");

                var items = from i in xDocument.Descendants("server")
                            select new
                            {
                                guildid = i.Attribute("id"),
                                onwake = i.Attribute("onwake"),
                                nsfwoverride = i.Attribute("nsfwoverride"),
                                useMarkov = i.Attribute("useMarkov")
                            };

                foreach (var item in items)
                {
                    if (item.guildid.Value == Context.Guild.Id.ToString())
                    {
                        exist = true;
                    }
                }

                if (!exist)
                {
                    XElement element = new XElement("server");

                    XAttribute idAtt = new XAttribute("id", Context.Guild.Id.ToString());
                    XAttribute onwakeAtt = new XAttribute("onwake", onWake);
                    XAttribute nsfwAtt = new XAttribute("nsfwoverride", nsfwOverride);
                    XAttribute useMarkovAtt = new XAttribute("useMarkov", useMarkov);

                    element.Add(idAtt);
                    element.Add(onwakeAtt);
                    element.Add(nsfwAtt);
                    element.Add(useMarkovAtt);

                    xDocument.Root.Add(element);
                    xDocument.Save("PerServTrigg.xml");

                    await ReplyAsync("Added server and inputs in per-server triggers.");
                }
                else
                {
                    await ReplyAsync("Per-server triggers have already been added to this server.");
                }
            }

            [Command("edit")]
            public async Task EditTrigger(string edit, string setTo)
            {
                xDocument = XDocument.Load("PerServTrigg.xml");

                var items = from i in xDocument.Descendants("server")
                            select new
                            {
                                guildid = i.Attribute("id"),
                                onwake = i.Attribute("onwake"),
                                nsfwoverride = i.Attribute("nsfwoverride"),
                                useMarkov = i.Attribute("useMarkov")
                            };

                foreach (var item in items)
                {
                    if (item.guildid.Value == Context.Guild.Id.ToString())
                    {
                        switch (edit)
                        {
                            case "onwake":
                                item.onwake.Value = setTo;
                                break;
                            case "nsfwoverride":
                                item.nsfwoverride.Value = setTo;
                                break;
                            case "useMarkov":
                                item.useMarkov.Value = setTo;
                                break;
                        }
                    }
                }

                xDocument.Save("PerServTrigg.xml");

                await ReplyAsync("Server trigger **" + edit + "** should now be: **" + setTo + "**.");
            }
        }
    }
}
