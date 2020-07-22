using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.Xml.Linq;
using xubot_core.src.Attributes;

namespace xubot_core.src.Globals
{
    public class ServerTriggers : ModuleBase
    {
        public static XDocument xdoc = new XDocument();

        [Group("servertriggers"), Summary("Server specific triggers? Wow that sounds hella lame"), Deprecated]
        public class Base : ModuleBase
        {
            [Command("add"), RequireUserPermission(GuildPermission.ManageGuild)]
            public async Task AddTrigger(string onwake = "", bool nsfwOverride = false, bool useMarkov = false)
            {
                //[>servertriggers add "onwake msg" true
                bool exist = false;
                xdoc = XDocument.Load("PerServTrigg.xml");

                var items = from i in xdoc.Descendants("server")
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

                    XAttribute id_att = new XAttribute("id", Context.Guild.Id.ToString());
                    XAttribute onwake_att = new XAttribute("onwake", onwake);
                    XAttribute nsfw_att = new XAttribute("nsfwoverride", nsfwOverride);
                    XAttribute useMarkov_att = new XAttribute("useMarkov", useMarkov);

                    element.Add(id_att);
                    element.Add(onwake_att);
                    element.Add(nsfw_att);
                    element.Add(useMarkov_att);

                    xdoc.Root.Add(element);
                    xdoc.Save("PerServTrigg.xml");

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
                xdoc = XDocument.Load("PerServTrigg.xml");

                var items = from i in xdoc.Descendants("server")
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

                xdoc.Save("PerServTrigg.xml");

                await ReplyAsync("Server trigger **" + edit + "** should now be: **" + setTo + "**.");
            }
        }
    }
}
