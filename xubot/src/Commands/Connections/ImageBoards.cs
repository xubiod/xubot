using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Xml.Linq;
using xubot.src;
using Discord;
using System.Net.Http;
using xubot.src.Attributes;
using System.Web;

namespace xubot.src.Commands.Connections
{
    using static xubot.src.SpecialException;

    public class ImageBoards : ModuleBase
    {
        //public JObject jsonInt = new JObject();

        public static XDocument xdoc = new XDocument();
        public HttpClient client = new HttpClient();

        // (TECHNICALLY) OPTIMIZED
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("danbooru", RunMode = RunMode.Async), Summary("Retrives a post from danbooru.")]
        public async Task Danbooru(string tags = "", bool spoiler = false)
        {
            // this shouldn't use the methods because of how different it is

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

                if (!(await Util.IsChannelNSFW(Context)) && keys.rating == "e")
                {
                    await ReplyAsync("Move to a NSFW channel.");
                }
                else
                {
                    string url;
                    if (keys.large_file_url.ToString().Contains("http"))
                    {
                        url = keys.large_file_url.ToString();
                    }
                    else
                    {
                        url = "http://danbooru.donmai.us" + keys.large_file_url.ToString();
                    }

                    if (spoiler) url = "|| " + url + " ||";

                    await ReplyAsync(url);
                }

                client.Dispose();
                request.Dispose();
            }
            catch (Exception exp)
            {
                await Util.Error.BuildError(exp, Context);
            }
        }

        // BROKEN?
        [NSFWPossibilty("TBD")]
        [Command("e621", RunMode = RunMode.Async), Summary("Retrives a post from e621 (Currently not working, probs got myself banned lol).")]
        public async Task e621(string tags = "", bool spoiler = false)
        {
            await Util.Error.BuildError(new ICannotBeArsedToFixThisException("prob got banned lol"), Context);
            return;

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

                if (!(await Util.IsChannelNSFW(Context)) && keys.rating == "e")
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
                await Util.Error.BuildError(exp, Context);
            }
        }

        // OPTIMIZED
        [NSFWPossibilty("Porn, snuff, whatever gets drawn.")]
        [Command("rule34", RunMode = RunMode.Async), Summary("Retrives a post from rule34, to the bot's dismay.")]
        public async Task r34(string tags = "", bool spoiler = true)
        {
            await GetPostFromXML("https://rule34.xxx/index.php?page=dapi&s=post&q=index&limit=1", tags, Context, spoiler, "&pid=");
        }

        // OPTIMIZED
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("gelbooru", RunMode = RunMode.Async), Summary("Retrives a post from gelbooru.")]
        public async Task Gelbooru(string tags = "", bool spoiler = false)
        {
            await GetPostFromXML("https://www.gelbooru.com/index.php?page=dapi&s=post&q=index&limit=1", tags, Context, spoiler, "&pid=");
        }

        // OPTIMIZED
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("yandere", RunMode = RunMode.Async), Summary("Retrives a post from yandere.")]
        public async Task Yandere(string tags = "", bool spoiler = false)
        {
            await GetPostFromXML("https://yande.re/post.xml?limit=1", tags, Context, spoiler);
        }

        // BROKEN?
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("e926", RunMode = RunMode.Async), Summary("Retrives a post from e926 (Currently not working, probs got myself banned lol).")]
        public async Task e926(string tags = "", bool spoiler = false)
        {
            await Util.Error.BuildError(new ICannotBeArsedToFixThisException("prob got banned lol"), Context);

            //ITextChannel c = Context.Channel as ITextChannel;

            //await GetPostFromXML("https://e926.net/post/index.xml?limit=1", tags, Context);

            //await GetPostFromJSON("https://e926.net/post/index.xml?limit=1&page=", "https://e926.net/post/index.json?limit=1&page=", tags, Context, spoiler);

            /*
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
             */
        }

        // OPTIMIZED
        [Command("safebooru", RunMode = RunMode.Async), Summary("Retrives a post from safebooru.")]
        public async Task Safebooru(string tags = "", bool spoiler = false)
        {
            await GetPostFromXML("http://safebooru.org/index.php?page=dapi&s=post&q=index&limit=1", tags, Context, spoiler, "&pid=");
        }

        // OPTIMIZED
        [NSFWPossibilty("Is a possibilty (although not guranteed).")]
        [Command("konachan", RunMode = RunMode.Async), Summary("Retrives a post from konachan.")]
        public async Task Konachan(string tags = "", bool spoiler = false)
        {
            await GetPostFromXML("http://konachan.com/post.xml?limit=1", tags, Context, spoiler);
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

        public async Task GetPostFromJSON(string xmlLink, string jsonLink, string tags, ICommandContext Context, bool spoiler)
        {
            try
            {
                Random rnd = new Random();
                //var client = new HttpClient();
                var webClient2 = new HttpClient();
                string link = xmlLink + "&tags=" + tags;
                string text_j = "";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(link),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("user-agent", "xubot/" + ThisAssembly.Git.BaseTag);

                string linkJson = jsonLink + rnd.Next(751).ToString() + "&tags=" + tags + "";

                text_j = await client.GetStringAsync(link);
                text_j = text_j.Substring(1, text_j.Length - 2);

                //await ReplyAsync(text);
                dynamic keys = JObject.Parse(text_j);

                string url = keys.file_url.ToString();

                if (spoiler) url = "|| " + url + " ||";

                await ReplyAsync(url);
                //string text = client.DownloadString(link);
                //text = text.Substring(1, text.Length - 2);
                //await ReplyAsync(text);
                //dynamic keys = JObject.Parse(text);

                //await ReplyAsync(keys.file_url.ToString());
            }
            catch (Exception exp)
            {
                await Util.Error.BuildError(exp, Context);
            }
        }

        public async Task GetPostFromXML(string inputLink, string tags, ICommandContext Context, bool spoiler, string pageIn = "&page=")
        {
            if (!(await Util.IsChannelNSFW(Context)))
            {
                await ReplyAsync("Move to a NSFW channel.");
            }
            else
            {
                try
                {
                    //var client = new HttpClient();
                    string link = inputLink + "&tags=" + tags;
                    Console.WriteLine(link);

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(link),
                        Method = HttpMethod.Get
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

                    link = inputLink + pageIn + rnd.Next(count) + "&tags=" + tags;
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

                    if (!imgUrl.Contains("http"))
                    {
                        imgUrl = "http:" + imgUrl;
                    }

                    if (spoiler) imgUrl = "|| " + imgUrl + " ||";

                    await ReplyAsync(imgUrl);
                }
                catch (Exception exp)
                {
                    await Util.Error.BuildError(exp, Context);
                }
            }
        }
    }
}
