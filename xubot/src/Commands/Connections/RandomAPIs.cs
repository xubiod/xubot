using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public static HttpClient httpClient = new();

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

            var request = WebRequest.Create(link);
            request.ContentType = "application/json; charset=utf-8";

            string text;
            var response = (HttpWebResponse)await request.GetResponseAsync();

            using (var sr = new StreamReader(response.GetResponseStream() ?? throw new NullReferenceException()))
            {
                text = await sr.ReadToEndAsync();
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
                using (HttpClient hc = new HttpClient()) { cont = await hc.GetStringAsync("http://shibe.online/api/shibes"); }
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
                using (HttpClient hc = new HttpClient()) { cont = await hc.GetStringAsync("http://shibe.online/api/birds"); }
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
    }
}
