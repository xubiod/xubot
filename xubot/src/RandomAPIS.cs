﻿using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace xubot
{
    public class RandomAPIS : ModuleBase
    {
        public static HttpClient httpClient = new HttpClient();

        [Group("number")]
        public class number : ModuleBase
        {
            [Command("trivia")]
            public async Task trivia(int number)
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/" + number.ToString() + "/trivia");
                var final = System.Text.Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("trivia")]
            public async Task trivia()
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/random/trivia");
                var final = System.Text.Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("year")]
            public async Task year(int number)
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/" + number.ToString() + "/year");
                var final = System.Text.Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("year")]
            public async Task year()
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/random/year");
                var final = System.Text.Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("math")]
            public async Task math(int number)
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/" + number.ToString() + "/math");
                var final = System.Text.Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("math")]
            public async Task math()
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/random/math");
                var final = System.Text.Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }
        }

        [Command("email-check")]
        public async Task check(string email)
        {
            string link = "https://www.validator.pizza/email/" + email;

            var request = WebRequest.Create(link);
            request.ContentType = "application/json; charset=utf-8";

            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            dynamic parsedTxt = JObject.Parse(text);
            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Validator.pizza Email Validator",
                Color = Discord.Color.Orange,
                Description = "**" + parsedTxt.remaining_requests.ToString() + "** requests left for the hour",

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Input",
                                Value = parsedTxt.email,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "MX Records?",
                                Value = parsedTxt.mx.ToString(),
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Disposable domain?",
                                Value = parsedTxt.disposable.ToString(),
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Alias?",
                                Value = parsedTxt.alias.ToString(),
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd);
        }

        [Command("expand-googl")]
        public async Task nameGen(string link)
        {
            string sendLink = "https://www.googleapis.com/urlshortener/v1/url?shortUrl=" + link + "&key=" + Program.keys.googleLinkShort.ToString();

            var request = WebRequest.Create(sendLink);
            request.ContentType = "application/json; charset=utf-8";

            string text;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            dynamic parsedTxt = JObject.Parse(text);
            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "goo.gl Link Expander",
                Color = Discord.Color.Orange,
                Description = "",

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "ID",
                                Value = parsedTxt.id,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Expanded Link",
                                Value = parsedTxt.longUrl,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Status",
                                Value = parsedTxt.status.ToString(),
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd);
        }

        [Command("cat")]
        public async Task cat()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            string final = "";
            string link = ("http://thecatapi.com/api/images/get?api_key=" + Program.keys.cat.ToString() + "&format=xml");

            var xdoc = XDocument.Load(link);

            var items = from i in xdoc.Descendants("url")
                        select new
                        {
                            i.Value
                        };

            foreach (var item in items)
            {
                final = item.Value;
            }

            //byte[] data = await httpClient.GetByteArrayAsync();
            //var final = System.Text.Encoding.Default.GetString(data);

            await ReplyAsync("Cat.\n" + final);
        }

        [Group("steam")]
        public class stem : ModuleBase
        {
            [Command("user", RunMode = RunMode.Async)]
            public async Task user(string id)
            {
                try
                {
                    var webClient = new WebClient();
                    string link = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + Program.keys.steam.ToString() + "&steamids=" + id;
                    string link2 = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=" + Program.keys.steam.ToString() + "&steamid=" + id;

                    webClient.Headers.Add("user-agent", "xubotSteam/2.0");

                    Uri request = new Uri(link);

                    string text = webClient.DownloadString(request);
                    string text2 = webClient.DownloadString(link2);
                    text = text.Substring(31);
                    text = text.Substring(0, text.Length - 9);
                    //await ReplyAsync(text);
                    dynamic keys = JObject.Parse(text);
                    dynamic keys2 = JObject.Parse(text2);

                    ulong created = Convert.ToUInt64(keys.timecreated);
                    DateTime createdDT = UnixTimeStampToDateTime(created);

                    ulong logoff = Convert.ToUInt64(keys.lastlogoff);
                    DateTime logoffDT = UnixTimeStampToDateTime(logoff);

                    EmbedBuilder embedd = new EmbedBuilder
                    {
                        Title = "Steam: User information on " + keys.personaname.ToString(),
                        Color = Discord.Color.Orange,
                        Description = "",
                        ThumbnailUrl = keys.avatarfull,

                        Footer = new EmbedFooterBuilder
                        {
                            Text = "xubot :p"
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Steam ID",
                                Value = keys.steamid.ToString(),
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Amount of Owned Games",
                                Value = keys2.response.game_count.ToString(),
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Profile URL",
                                Value = keys.profileurl.ToString(),
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Created on (UTC)",
                                Value = createdDT.ToUniversalTime().ToLongDateString() + " @ " + createdDT.ToUniversalTime().ToLongTimeString(),
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Last Online (UTC)",
                                Value = logoffDT.ToUniversalTime().ToLongDateString() + " @ " + logoffDT.ToUniversalTime().ToLongTimeString(),
                                IsInline = false
                            }

                        }
                    };

                    await ReplyAsync("", false, embedd);
                }
                catch (Exception exp)
                {
                    await ReplyAsync(exp.Message);
                }
            }

            public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
            {
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
                return dtDateTime;
            }
        }

    }
}
