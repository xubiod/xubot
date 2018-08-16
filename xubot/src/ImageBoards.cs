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
using xubot.src;
using Discord;
using System.Net.Http;

namespace xubot
{
    public class ImageBoards : ModuleBase
    {
        //public JObject jsonInt = new JObject();

        public static XDocument xdoc = new XDocument();
        public HttpClient client = new HttpClient();

        [Command("danbooru", RunMode = RunMode.Async)]
        public async Task danbooru(string tags = "")
        {
            try
            {
                //var client = new HttpClient();
                string link = "http://danbooru.donmai.us/posts.json?limit=1&random=true&tags=" + tags;
                string text = "";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(link),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

                await client.SendAsync(request).ContinueWith(async (res) =>
                {
                    var response = res.Result;
                    text = await response.Content.ReadAsStringAsync();
                });

                text = text.Substring(1, text.Length - 2);
                //await ReplyAsync(text);
                dynamic keys = JObject.Parse(text);

                if (!(await GeneralTools.ChannelNSFW(Context)) && keys.rating == "e")
                {
                    await ReplyAsync("Move to a NSFW channel.");
                }
                else
                {
                    if (keys.large_file_url.ToString().Contains("http"))
                    {
                        await ReplyAsync(keys.large_file_url.ToString());
                    }
                    else
                    {
                        await ReplyAsync("http://danbooru.donmai.us" + keys.large_file_url.ToString());
                    }
                }

                client.Dispose();
                request.Dispose();
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("e621", RunMode = RunMode.Async)]
        public async Task e621(string tags = "")
        {
            try
            {
                Random rnd = new Random();
                //var client = new HttpClient();
                var webClient2 = new HttpClient();
                string link = "https://e621.net/post/index.xml?limit=1&page=&tags=" + tags;
                string text_j = "";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(link),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

                string linkJson = "https://e621.net/post/index.json?limit=1&page=" + rnd.Next(751).ToString() + "&tags=" + tags + "";
                
                await client.SendAsync(request).ContinueWith(async (res) =>
                {
                    var response = res.Result;
                    text_j = await response.Content.ReadAsStringAsync();
                });

                text_j = await client.GetStringAsync(link);
                text_j = text_j.Substring(1, text_j.Length - 2);

                //await ReplyAsync(text);
                dynamic keys = JObject.Parse(text_j);

                if (!(await GeneralTools.ChannelNSFW(Context)) && keys.rating == "e")
                {
                    await ReplyAsync("Move to a NSFW channel.");
                }
                else
                {
                    await ReplyAsync(keys.file_url.ToString());
                    //string text = client.DownloadString(link);
                    //text = text.Substring(1, text.Length - 2);
                    //await ReplyAsync(text);
                    //dynamic keys = JObject.Parse(text);

                    //await ReplyAsync(keys.file_url.ToString());
                }
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
            
        }

        [Command("rule34", RunMode = RunMode.Async)]
        public async Task r34(string tags = "")
        {
            if (!(await GeneralTools.ChannelNSFW(Context)))
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else
            if ((Program.enableNSFW || CheckTrigger()) && tags != "")
            {
                try
                {
                    //var client = new HttpClient();
                    string link = "https://rule34.xxx/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags;

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(link),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

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
            if (!(await GeneralTools.ChannelNSFW(Context)))
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else
            if ((Program.enableNSFW || CheckTrigger()) && tags != "")
            {
                try
                {
                   // var client = new HttpClient();
                    string link = "https://www.gelbooru.com/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags;

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(link),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

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
            if (!(await GeneralTools.ChannelNSFW(Context)))
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else
            if (Program.enableNSFW || CheckTrigger())
            {
                try
                {
                    //var client = new HttpClient();
                    string link = "https://yande.re/post.xml?limit=1&tags=" + tags;

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(link),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

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

        [Command("e926", RunMode = RunMode.Async)]
        public async Task e926(string tags = "")
        {
            //ITextChannel c = Context.Channel as ITextChannel;

            try
            {
                Random rnd = new Random();
                //var client = new HttpClient();
                var webClient2 = new HttpClient();
                string link = "https://e926.net/post/index.xml?limit=1&page=&tags=" + tags;
                string text_j = "";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(link),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

                string linkJson = "https://e926.net/post/index.json?limit=1&page=" + rnd.Next(751).ToString() + "&tags=" + tags + "";

                text_j = await client.GetStringAsync(link);
                text_j = text_j.Substring(1, text_j.Length - 2);

                //await ReplyAsync(text);
                dynamic keys = JObject.Parse(text_j);

                await ReplyAsync(keys.file_url.ToString());
                //string text = client.DownloadString(link);
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

        [Command("safebooru", RunMode = RunMode.Async)]
        public async Task safebooru(string tags = "")
        {
            try
            {
                //var client = new HttpClient();
                string link = "http://safebooru.org/index.php?page=dapi&s=post&q=index&limit=1&tags=" + tags;

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(link),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

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

                link = "http://safebooru.org/index.php?page=dapi&s=post&q=index&limit=1&pid=" + rnd.Next(count) + "&tags=" + tags;
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

                await ReplyAsync("http:" + imgUrl);

            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }

        }

        [Command("konachan", RunMode = RunMode.Async)]
        public async Task konachan(string tags = "")
        {
            if (!(await GeneralTools.ChannelNSFW(Context)))
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else
            if (CheckTrigger())
            {
                try
                {
                    //var client = new HttpClient();
                    string link = "http://konachan.com/post.xml?limit=1&tags=" + tags;

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(link),
                        Method = HttpMethod.Get,
                    };

                    request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

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

                    link = "http://konachan.com/post.xml?limit=1&page=" + rnd.Next(count) + "&tags=" + tags;
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

        }

        //shorthands

        //removed for now
        /*
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
        */
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
