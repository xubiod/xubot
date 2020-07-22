using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace xubot_core.src.Connections
{
    public class ReverseImageSearch
    {
        [Group("sauce"), Alias("saucenao"), Summary("Uses the SauceNAO API to reverse image search.")]
        public class SauceNao : ModuleBase
        {
            private static readonly string SingleResultUrl = "https://saucenao.com/search.php?db=999&output_type=2&numres=1" + APIKey;
            private static readonly string TopResultUrl = "https://saucenao.com/search.php?db=999&output_type=2&numres=";
            private static readonly string APIKey = "&api_key=" + Program.keys.saucenao + "&url=";

            private static HttpClient client = new HttpClient();

            [Command("", RunMode = RunMode.Async)]
            public async Task NamelessGet()
            {
                await GetSauce();
            }

            [Command("", RunMode = RunMode.Async)]
            public async Task NamelessGet(string url)
            {
                await GetSauce(url);
            }

            [Command("get", RunMode = RunMode.Async), Summary("Uses SauceNAO to get the \"sauce\" of an attached image, returning the number 1 result.")]
            public async Task GetSauce()
            {
                if (Context.Message.Attachments.Count == 0)
                {
                    await Util.Error.BuildError("No attachments or parameters were given.", Context);
                    return;
                }
                await GetSauce(Util.File.ReturnLastAttachmentURL(Context));
            }

            [Command("get", RunMode = RunMode.Async), Summary("Uses SauceNAO to get the \"sauce\" of the given URL, returning the number 1 result.")]
            public async Task GetSauce(string url)
            {
                if (!Util.Str.ValidateURL(url))
                {
                    await Util.Error.BuildError("Invalid URL", Context);
                    return;
                }

                string assembledURL = SingleResultUrl + HttpUtility.UrlEncode(url);
                dynamic keys = JObject.Parse(await client.GetStringAsync(assembledURL));

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
                await ReplyAsync("I am **" + similarity + "%** confident it is **" + src + "** thanks to SauceNAO.\n" +
                    "> *I have __" + ((JObject)keys.header).Value<int>("short_remaining").ToString() + " requests__ left for the next __30 seconds__, " +
                    "and __" + ((JObject)keys.header).Value<int>("long_remaining").ToString() + " requests__ left for the next __24 hours__.*\n\n" +
                    "> ***__Please be respectful to the developers of SauceNAO and their API,__***\n> ***__and make sure others who have the bot can use this command.__***");
            }

            [Command("top", RunMode = RunMode.Async), Summary("Uses SauceNAO to get the \"sauce\" of an attached image, returning the top results. Limited from 1 to 5, defaults to 5.")]
            public async Task GetTop(int amount = 5)
            {
                if (Context.Message.Attachments.Count == 0)
                {
                    await Util.Error.BuildError("No attachments or parameters were given.", Context);
                    return;
                }
                await GetTop(amount, Util.File.ReturnLastAttachmentURL(Context));
            }

            [Command("top", RunMode = RunMode.Async), Summary("Uses SauceNAO to get the \"sauce\" of the given URL, returning the top results. Limited from 1 to 5.")]
            public async Task GetTop(int amount, string url)
            {
                amount = (int)MathF.Max(1.0f, MathF.Min(5.0f, amount));

                if (!Util.Str.ValidateURL(url))
                {
                    await Util.Error.BuildError("Invalid URL", Context);
                    return;
                }

                string assembledURL = TopResultUrl + amount.ToString() + APIKey + HttpUtility.UrlEncode(url);
                dynamic keys = JObject.Parse(await client.GetStringAsync(assembledURL));
                JArray ext_urls;

                string similarity, src;
                string extraData = "";

                List<EmbedFieldBuilder> embedFields = new List<EmbedFieldBuilder>();

                for (int i = 0; i < amount; i++)
                {
                    similarity = (keys.results[i].header.similarity ?? "No similarity given").ToString();
                    src = (keys.results[i].data.source ?? "").ToString();
                    ext_urls = (keys.results[i].data.ext_urls ?? null);

                    if (ext_urls is JArray) extraData = String.Join(", ", ext_urls);

                    embedFields.Add(new EmbedFieldBuilder
                    {
                        Name = "No. " + (i + 1).ToString(),
                        IsInline = false,
                        Value = similarity + "% - " + src + extraData
                    });

                    extraData = "";
                }

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "SauceNAO of given image - Top " + amount.ToString(),
                    Description = "*I have __" + ((JObject)keys.header).Value<int>("short_remaining").ToString() + " requests__ left for the next __30 seconds__, " +
                                  "and __" + ((JObject)keys.header).Value<int>("long_remaining").ToString() + " requests__ left for the next __24 hours__.* " +
                                  "***__Please be respectful to the developers of SauceNAO and their API, and make sure others who have the bot can use this command.__***",
                    Color = Discord.Color.LighterGrey,
                    ThumbnailUrl = Program.xuClient.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = embedFields
                };

                await ReplyAsync("", false, embedd.Build());
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
