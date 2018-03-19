using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Xml.Linq;

namespace xubot
{
    public class NSFW : ModuleBase
    {
        //public JObject jsonInt = new JObject();

        public static XDocument xdoc = new XDocument();

        [Command("danbooru", RunMode = RunMode.Async)]
        public async Task danbooru(string tags = "")
        {
            if (!Context.Channel.IsNsfw)
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else if (Program.enableNSFW || CheckTrigger())
            {
                try
                {
                    var webClient = new WebClient();
                    string link = "http://danbooru.donmai.us/posts.json?limit=1&random=true&tags=" + tags;

                    webClient.Headers.Add("user-agent", "xubotNSFW/2.0");

                    string text = webClient.DownloadString(link);
                    text = text.Substring(1, text.Length - 2);
                    //await ReplyAsync(text);
                    dynamic keys = JObject.Parse(text);

                    if (keys.large_file_url.ToString().Contains("http"))
                    {
                        await ReplyAsync(keys.large_file_url.ToString());
                    }
                    else
                    {
                        await ReplyAsync("http://danbooru.donmai.us" + keys.large_file_url.ToString());
                    }
                }
                catch (Exception exp)
                {
                    await GeneralTools.CommHandler.BuildError(exp, Context);
                }
            }
            else
            {
                if (Program.enableNSFW) await ReplyAsync("NSFW commands are disabled at the moment.");
                else { await ReplyAsync("Umm.. something happened that I don't have a clue about. Whoopies."); }
            }
        }

        [Command("e621", RunMode = RunMode.Async)]
        public async Task e621(string tags = "")
        {
            if (!Context.Channel.IsNsfw)
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else if (Program.enableNSFW || CheckTrigger())
            {
                try
                {
                    Random rnd = new Random();
                    var webClient = new WebClient();
                    var webClient2 = new WebClient();
                    string link = "https://e621.net/post/index.xml?limit=1&page=&tags=" + tags;
                    string text_j = "";

                    webClient.Headers.Add("user-agent", "xubotNSFW/2.0");
                    string linkJson = "https://e621.net/post/index.json?limit=1&page=" + rnd.Next(751).ToString() + "&tags=" + tags + "";

                    text_j = webClient.DownloadString(linkJson);
                    text_j = text_j.Substring(1, text_j.Length - 2);

                    //await ReplyAsync(text);
                    dynamic keys = JObject.Parse(text_j);

                    await ReplyAsync(keys.file_url.ToString());
                    //string text = webClient.DownloadString(link);
                    //text = text.Substring(1, text.Length - 2);
                    //await ReplyAsync(text);
                    //dynamic keys = JObject.Parse(text);

                    //await ReplyAsync(keys.file_url.ToString());
                }
                catch (Exception exp)
                {
                    await GeneralTools.CommHandler.BuildError(exp, Context);
                }
            }
            else
            {
                if (Program.enableNSFW) await ReplyAsync("NSFW commands are disabled at the moment.");
                else { await ReplyAsync("Umm.. something happened that I don't have a clue about. Whoopies."); }
            }
        }

        [Command("rule34", RunMode = RunMode.Async)]
        public async Task r34(string tags = "")
        {
            if (!Context.Channel.IsNsfw)
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else if ((Program.enableNSFW || CheckTrigger()) && tags != "")
            {
                try
                {
                    var webClient = new WebClient();
                    string link = "https://rule34.xxx/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags;

                    webClient.Headers.Add("user-agent", "xubotNSFW/2.0");

                    string imgUrl = "";
                    int count = 0;
                    Random rnd = new Random();

                    var xdoc = XDocument.Load(link);
                    var items = from i in xdoc.Descendants("posts")
                                select new
                                {
                                    Attribute = (string)i.Attribute("count")
                                };

                    foreach (var item in items)
                    {
                        count = Convert.ToInt32(item.Attribute) / 2;
                    }

                    link = "https://rule34.xxx/index.php?page=dapi&s=post&q=index&limit=1&pid=" + rnd.Next(count) + "&tags=" + tags;
                    xdoc = XDocument.Load(link);

                    items = from q in xdoc.Descendants("post")
                            select new
                            {
                                Attribute = (string)q.Attribute("file_url")
                            };

                    foreach (var item in items)
                    {
                        imgUrl = item.Attribute;
                    }

                    await ReplyAsync(imgUrl);
                }
                catch (Exception exp)
                {
                    await GeneralTools.CommHandler.BuildError(exp, Context);
                }
            }
            else
            {
                if (tags == "" && Program.enableNSFW)
                {
                    await ReplyAsync("rule34 requires a tag.");
                }
                else
                {
                    if (Program.enableNSFW) await ReplyAsync("NSFW commands are disabled at the moment.");
                    else { await ReplyAsync("Umm.. something happened that I don't have a clue about. Whoopies."); }
                }
            }
        }

        [Command("gelbooru", RunMode = RunMode.Async)]
        public async Task gelbooru(string tags = "")
        {
            if (!Context.Channel.IsNsfw)
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else if ((Program.enableNSFW || CheckTrigger()) && tags != "")
            {
                try
                {
                    var webClient = new WebClient();
                    string link = "https://www.gelbooru.com/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags;

                    webClient.Headers.Add("user-agent", "xubotNSFW/2.0");

                    string imgUrl = "";
                    int count = 0;
                    Random rnd = new Random();

                    var xdoc = XDocument.Load(link);
                    var items = from i in xdoc.Descendants("posts")
                                select new
                                {
                                    Attribute = (string)i.Attribute("count")
                                };

                    foreach (var item in items)
                    {
                        count = Convert.ToInt32(item.Attribute) / 2;
                    }

                    link = "https://www.gelbooru.com/index.php?page=dapi&s=post&q=index&limit=1&pid=" + rnd.Next(count) + "&tags=" + tags;
                    xdoc = XDocument.Load(link);

                    items = from q in xdoc.Descendants("post")
                            select new
                            {
                                Attribute = (string)q.Attribute("file_url")
                            };

                    foreach (var item in items)
                    {
                        imgUrl = item.Attribute;
                    }

                    await ReplyAsync(imgUrl);
                }
                catch (Exception exp)
                {
                    await GeneralTools.CommHandler.BuildError(exp, Context);
                }
            }
            else
            {
                if (tags == "" && Program.enableNSFW)
                {
                    await ReplyAsync("Gelbooru requires a tag.");
                }
                else
                {
                    if (Program.enableNSFW) await ReplyAsync("NSFW commands are disabled at the moment.");
                    else { await ReplyAsync("Umm.. something happened that I don't have a clue about. Whoopies."); }
                }
            }
        }

        [Command("yandere", RunMode = RunMode.Async)]
        public async Task yandere(string tags = "")
        {
            if (!Context.Channel.IsNsfw)
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else if (Program.enableNSFW || CheckTrigger())
            {
                try
                {
                    var webClient = new WebClient();
                    string link = "https://yande.re/post.xml?limit=1&tags=" + tags;

                    webClient.Headers.Add("user-agent", "xubotNSFW/2.0");

                    string imgUrl = "";
                    int count = 0;
                    Random rnd = new Random();

                    var xdoc = XDocument.Load(link);
                    var items = from i in xdoc.Descendants("posts")
                                select new
                                {
                                    Attribute = (string)i.Attribute("count")
                                };

                    foreach (var item in items)
                    {
                        count = Convert.ToInt32(item.Attribute) / 2;
                    }

                    link = "https://yande.re/post.xml?limit=1&page=" + rnd.Next(count) + "&tags=" + tags;
                    xdoc = XDocument.Load(link);

                    items = from q in xdoc.Descendants("post")
                            select new
                            {
                                Attribute = (string)q.Attribute("file_url")
                            };

                    foreach (var item in items)
                    {
                        imgUrl = item.Attribute;
                    }

                    await ReplyAsync(imgUrl);

                }
                catch (Exception exp)
                {
                    await GeneralTools.CommHandler.BuildError(exp, Context);
                }
            }
            else
            {
                if (Program.enableNSFW) await ReplyAsync("NSFW commands are disabled at the moment.");
                else { await ReplyAsync("Umm.. something happened that I don't have a clue about. Whoopies."); }
            }
        }

        //shorthands
        [Command("catgirl")]
        public async Task catG()
        {
            await danbooru("cat_girl");
        }

        [Command("what")]
        public async Task wat()
        {
            await r34("what");
        }

        [Command("dragon")]
        public async Task dragon()
        {
            await r34("dragon");
        }

        [Command("dragon_f")]
        public async Task dragonE()
        {
            await e621("dragon");
        }

        [Command("gaydragon")]
        public async Task gaydragon()
        {
            await r34("gay_dragon");
        }

        [Command("inflation")]
        public async Task inflationKink()
        {
            await r34("inflation");
        }

        public bool CheckTrigger()
        {
            bool ret = false;
            xdoc = XDocument.Load("PerServTrigg.xml");
            var items = from i in xdoc.Descendants("server")
                        select new
                        {
                            guildid = i.Attribute("id"),
                            nsfwoverride = i.Attribute("nsfwoverride")
                        };

            foreach (var item in items)
            {
                if (item.guildid.Value == Context.Guild.Id.ToString())
                {
                    ret = true;
                }
            }

            return ret;
        }
    }
}
