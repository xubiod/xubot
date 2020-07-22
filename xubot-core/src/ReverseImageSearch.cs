using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace xubot_core.src
{
    public class ReverseImageSearch
    {
        public class SauceNao : ModuleBase
        {
            private static readonly string Url = "https://saucenao.com/search.php?db=999&output_type=2&numres=1&api_key=" + Program.keys.saucenao + "&url=";
            private static HttpClient client = new HttpClient();

            [Command("sauce", RunMode = RunMode.Async), Summary("Uses SauceNAO to get the \"sauce\" of an attached image.")]
            public async Task GetSauce()
            {
                if (Context.Message.Attachments.Count == 0)
                {
                    await Util.Error.BuildError("No attachments or parameters were given.", Context);
                    return;
                }
                await GetSauce(Util.File.ReturnLastAttachmentURL(Context));
            }

            [Command("sauce", RunMode = RunMode.Async), Summary("Uses SauceNAO to get the \"sauce\" of the given URL.")]
            public async Task GetSauce(string url)
            {
                if (!Util.Str.ValidateURL(url))
                {
                    await Util.Error.BuildError("Invalid URL", Context);
                    return;
                }

                string assembledURL = Url + HttpUtility.UrlEncode(url);
                dynamic keys = JObject.Parse(await client.GetStringAsync(assembledURL));
                JToken dummy;

                string similarity = (keys.results[0].header.similarity ?? "?").ToString();
                string src = (keys.results[0].data.source ?? "?").ToString();

                if (src == "?")
                {
                    await ReplyAsync("SauceNAO didn't give me a source...\n" +
                        "> *I have __" + ((JObject)keys.header).Value<int>("short_remaining").ToString() + " requests__ left for the next __30 seconds__, " +
                        "and __" + ((JObject)keys.header).Value<int>("long_remaining").ToString() + " requests__ left for the next __24 hours__.*\n\n" +
                        "> ***__Please be respectful to the developers of SauceNAO and their API,__***\n> ***__and make sure others who have the bot can use this command.__***");

                    return;
                }
                await ReplyAsync("I am **" + similarity + "%** confident it is " + src + " thanks to SauceNAO.\n" +
                    "> *I have __" + ((JObject)keys.header).Value<int>("short_remaining").ToString() + " requests__ left for the next __30 seconds__, " +
                    "and __" + ((JObject)keys.header).Value<int>("long_remaining").ToString() + " requests__ left for the next __24 hours__.*\n\n" +
                    "> ***__Please be respectful to the developers of SauceNAO and their API,__***\n> ***__and make sure others who have the bot can use this command.__***");
            }

            private string GetConfidenceString(float confidence)
            {
                switch (MathF.Floor(confidence * 0.06f))
                {
                    case 0: return "Very sketch";
                    case 1: return "Sketch";
                    case 2: return "Ok";
                    case 3: return "Good";
                    case 4: return "Great";
                    case 5: return "Excellent";
                    case 6: return "Match I assume";
                    default: return "???";
                }
            }
        }
    }
}
