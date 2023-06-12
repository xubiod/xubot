using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using xubot.Attributes;

namespace xubot.Commands.Connections
{
    public class RandomApis : ModuleBase
    {
        private static readonly HttpClient httpClient = new();

        [Group("number"), Summary("LEARNING AAAAAAAAAAAA")]
        public class Number : ModuleBase
        {
            [Example("42")]
            [Command("trivia", RunMode = RunMode.Async)]
            public async Task Trivia(int number)
            {
                byte[] trivia = await httpClient.GetByteArrayAsync($"http://numbersapi.com/{number}/trivia");
                var final = Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("trivia", RunMode = RunMode.Async)]
            public async Task Trivia()
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/random/trivia");
                var final = Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Example("2001")]
            [Command("year", RunMode = RunMode.Async)]
            public async Task Year(int number)
            {
                byte[] trivia = await httpClient.GetByteArrayAsync($"http://numbersapi.com/{number}/year");
                var final = Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("year", RunMode = RunMode.Async)]
            public async Task Year()
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/random/year");
                var final = Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Example("45")]
            [Command("math", RunMode = RunMode.Async)]
            public async Task Math(int number)
            {
                byte[] trivia = await httpClient.GetByteArrayAsync($"http://numbersapi.com/{number}/math");
                var final = Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }

            [Command("math", RunMode = RunMode.Async)]
            public async Task Math()
            {
                byte[] trivia = await httpClient.GetByteArrayAsync("http://numbersapi.com/random/math");
                var final = Encoding.Default.GetString(trivia);

                await ReplyAsync(final);
            }
        }

        [Example("example@example.com")]
        [Command("email-check", RunMode = RunMode.Async), Summary("Uses API to check if email is a temporary one for sCaMs OoOoOoO")]
        public async Task ValidEmail(string email)
        {
            string link = "https://www.validator.pizza/email/" + email;

            string text;
            using (var httpClient = new HttpClient())
            {
                var request = await httpClient.GetAsync(link);
                // request.ContentType = "application/json; charset=utf-8";

                text = await request.Content.ReadAsStringAsync();
            }

            dynamic parsedTxt = JObject.Parse(text);

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Validator.pizza Email Validator", $"**{parsedTxt.remaining_requests}** requests left for the hour", Color.Orange);
            embed.Fields = new List<EmbedFieldBuilder>
            {
                new()
                {
                    Name = "Input",
                    Value = parsedTxt.email,
                    IsInline = false
                },
                new()
                {
                    Name = "MX Records?",
                    Value = parsedTxt.mx.ToString(),
                    IsInline = false
                },
                new()
                {
                    Name = "Disposable domain?",
                    Value = parsedTxt.disposable.ToString(),
                    IsInline = false
                },
                new()
                {
                    Name = "Alias?",
                    Value = parsedTxt.alias.ToString(),
                    IsInline = false
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Command("cat", RunMode = RunMode.Async), Summary("Gets random cat picture. Best utilized when sad.")]
        public async Task CatImage()
        {
            /*XmlReaderSettings settings = new XmlReaderSettings
            {
                Async = true
            };
            */
            string final = "";
            string link = "http://thecatapi.com/api/images/get?api_key=" + $"{Program.JsonKeys["keys"].Contents.cat}&format=xml";

            var xDocument = XDocument.Load(link);

            var items = from i in xDocument.Descendants("url")
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

            await ReplyAsync($"Cat.\n{final}");
        }

        [Command("shibe", RunMode = RunMode.Async), Summary("Gets random Shibe Inu picture. Best utilized when sad.")]
        public async Task ShibeInuImage()
        {
            //http://shibe.online/api/shibes

            // i couldn't get jobject or jarray to parse it :P
            // which is why it's such a strange thing for json
            // don't fuckin judge me >:(

            try
            {
                string cont;
                using (HttpClient hc = new()) { cont = await hc.GetStringAsync("http://shibe.online/api/shibes"); }
                cont = cont.Trim('[', ']', '"');
                Console.WriteLine(cont);

                await ReplyAsync($"Shibe.\n{cont}");
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }

        [Command("bird", RunMode = RunMode.Async), Summary("Gets random bird picture. Best utilized when sad.")]
        public async Task BirdImage()
        {
            //http://shibe.online/api/birds

            // i couldn't get jobject or jarray to parse it :P
            // which is why it's such a strange thing for json
            // don't fuckin judge me >:(

            try
            {
                string cont;
                using (HttpClient hc = new()) { cont = await hc.GetStringAsync("http://shibe.online/api/birds"); }
                cont = cont.Trim('[', ']', '"');
                Console.WriteLine(cont);

                if (Util.Globals.Rng.Next(100) == 0)
                {
                    await ReplyAsync($"Birb.\n{cont}");
                }
                else
                {
                    await ReplyAsync($"Bird.\n{cont}");
                }
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }



        [Example("\"New York City\"")]
        [Command("timezone", RunMode = RunMode.Async), Summary("Returns the timezone from a given string.")]
        public async Task Timezone(string loc)
        {
            try
            {
                string link = $"https://www.amdoren.com/api/timezone.php?api_key={Program.JsonKeys["keys"].Contents.amdoren}&loc={loc}";

                string textJ;

                using (HttpClient httpClient = new())
                {
                    textJ = await httpClient.GetStringAsync(link);
                }

                // var webClient2 = new WebClient();

                //text_j = text_j.Substring(1, text_j.Length - 2);

                //await ReplyAsync(text);
                dynamic keys = JObject.Parse(textJ);

                if (keys.error_message.ToString() != "-")
                {
                    EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Timezone Location", "Error!", Color.Red);
                    embed.Footer.Text = $"The API requires free users to link to the API, so here it is:\n https://www.amdoren.com/time-zone-api/ \n{embed.Footer.Text}";
                    embed.Fields = new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                            Name = "The API returned: ",
                            Value = $"**{keys.error_message}**",
                            IsInline = false
                        }
                    };

                    await ReplyAsync("", false, embed.Build());
                }
                else
                {
                    EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Timezone Location", $"Timezone and time for {loc}", Color.Red);
                    embed.Footer.Text = $"The API requires free users to link to the API, so here it is:\n https://www.amdoren.com/time-zone-api/ \n{embed.Footer.Text}";
                    embed.Fields = new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                                Name = "Timezone: ",
                                Value = $"**{keys.timezone}**",
                                IsInline = true
                            },
                            new()
                            {
                                Name = "Current Time: ",
                                Value = $"**{keys.time}**",
                                IsInline = true
                            }
                    };

                    await ReplyAsync("", false, embed.Build());
                }
                //string text = webClient.DownloadString(link);
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
    }
}
