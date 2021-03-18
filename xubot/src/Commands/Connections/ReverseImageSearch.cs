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
using xubot.src.Attributes;

namespace xubot.src.Commands.Connections
{
    public class ReverseImageSearch
    {
        [Group("sauce"), Alias("saucenao"), Summary("Uses the SauceNAO API to reverse image search.")]
        public class SauceNao : ModuleBase
        {
            private const string SingleResultUrl = "https://saucenao.com/search.php?db=999&output_type=2&numres=1";
            private const string TopResultUrl = "https://saucenao.com/search.php?db=999&output_type=2&numres=";
            private static readonly string APIKey = $"&api_key={Program.JSONKeys["keys"].Contents.saucenao}&url=";

            private enum RequestsLeftType { NoFormatting, Message, Embed }

            private static HttpClient client = new HttpClient();

            [Example(true)]
            [Command("get", RunMode = RunMode.Async), Alias(""), Summary("Uses SauceNAO to get the \"sauce\" of an attached image, returning the number 1 result.")]
            public async Task GetSauce()
            {
                if (Context.Message.Attachments.Count == 0)
                {
                    await Util.Error.BuildError("No attachments or parameters were given.", Context);
                    return;
                }
                await GetSauce(Util.File.ReturnLastAttachmentURL(Context));
            }

            [Example("example.com/example_img")]
            [Command("get", RunMode = RunMode.Async), Alias(""), Summary("Uses SauceNAO to get the \"sauce\" of the given URL, returning the number 1 result.")]
            public async Task GetSauce(string url)
            {
                if (!Util.String.ValidateURL(url))
                {
                    await Util.Error.BuildError("Invalid URL", Context);
                    return;
                }

                using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                {
                    string assembledURL = SingleResultUrl + APIKey + HttpUtility.UrlEncode(url);
                    dynamic keys = JObject.Parse(await client.GetStringAsync(assembledURL));

                    if (keys.header.status != 0)
                    {
                        BuildSauceNAOError(keys, Context);
                        return;
                    }

                    string similarity = (keys.results[0].header.similarity ?? "?").ToString();
                    string src = (keys.results[0].data.source ?? (keys.results[0].data.ext_urls[0] ?? "?")).ToString();

                    if (src == "?")
                    {
                        await ReplyAsync($"SauceNAO didn't give me a source...\n{GetRequestsLeft(keys, RequestsLeftType.Message)}");
                        return;
                    }
                    await ReplyAsync($"I am **{similarity}%** confident it is **{src}** thanks to SauceNAO.\n" + GetRequestsLeft(keys, RequestsLeftType.Message));
                }
            }

            [Example("5", true)]
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

            [Example("5 example.com/example_img")]
            [Command("top", RunMode = RunMode.Async), Summary("Uses SauceNAO to get the \"sauce\" of the given URL, returning the top results. Limited from 1 to 5.")]
            public async Task GetTop(int amount, string url)
            {
                amount = (int)MathF.Max(1.0f, MathF.Min(5.0f, amount));

                if (!Util.String.ValidateURL(url))
                {
                    await Util.Error.BuildError("Invalid URL", Context);
                    return;
                }
                using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                {
                    string assembledURL = TopResultUrl + amount.ToString() + APIKey + HttpUtility.UrlEncode(url);
                    dynamic keys = JObject.Parse(await client.GetStringAsync(assembledURL));

                    if (keys.header.status != 0)
                    {
                        BuildSauceNAOError(keys, Context);
                        return;
                    }

                    JArray ext_urls;

                    string similarity, src;
                    string extraData = "";

                    List<EmbedFieldBuilder> embedFields = new List<EmbedFieldBuilder>();

                    for (int i = 0; i < amount; i++)
                    {
                        similarity = (keys.results[i].header.similarity ?? "No similarity given").ToString();
                        src = (keys.results[i].data.source ?? "").ToString();
                        ext_urls = (keys.results[i].data.ext_urls ?? null);

                        if (ext_urls is JArray) extraData = (src == "" ? "" : ", ") + String.Join(", ", ext_urls);

                        embedFields.Add(new EmbedFieldBuilder
                        {
                            Name = "No. " + (i + 1).ToString(),
                            IsInline = false,
                            Value = similarity + "% (" + GetConfidenceString(float.Parse(similarity)) + ") - " + src + extraData
                        });

                        extraData = "";
                    }

                    EmbedBuilder embedd = GetTemplate(keys, "SauceNAO of given image - Top " + amount.ToString(), embedFields);

                    await ReplyAsync("", false, embedd.Build());
                }
            }

            [Example(true)]
            [Command("details", RunMode = RunMode.Async), Alias("detail", "full"), Summary("Uses SauceNAO to get the \"sauce\" of an attached image, returning a detailed report on it.")]
            public async Task GetDetails()
            {
                if (Context.Message.Attachments.Count == 0)
                {
                    await Util.Error.BuildError("No attachments or parameters were given.", Context);
                    return;
                }
                await GetDetails(Util.File.ReturnLastAttachmentURL(Context));
            }

            [Example("example.com/example_img")]
            [Command("details", RunMode = RunMode.Async), Alias("detail", "full"), Summary("Uses SauceNAO to get the \"sauce\" of a URL, returning a detailed report on it.")]
            public async Task GetDetails(string url)
            {
                if (!Util.String.ValidateURL(url))
                {
                    await Util.Error.BuildError("Invalid URL", Context);
                    return;
                }

                using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
                {
                    string assembledURL = SingleResultUrl + APIKey + HttpUtility.UrlEncode(url);
                    dynamic keys = JObject.Parse(await client.GetStringAsync(assembledURL));

                    if (keys.header.status != 0)
                    {
                        BuildSauceNAOError(keys, Context);
                        return;
                    }

                    string src = (keys.results[0].data.source ?? (keys.results[0].data.ext_urls[0] ?? "?")).ToString();

                    if (src == "?")
                    {
                        await ReplyAsync($"SauceNAO didn't give me a source...\n{GetRequestsLeft(keys, RequestsLeftType.Message)}");

                        return;
                    }

                    List<EmbedFieldBuilder> embedFields = new List<EmbedFieldBuilder>();

                    embedFields.Add(new EmbedFieldBuilder { Name = "Similarity", IsInline = true, Value = (keys.results[0].header.similarity ?? "Not given").ToString() });
                    embedFields.Add(new EmbedFieldBuilder { Name = "Source", IsInline = true, Value = (keys.results[0].data.source ?? "Not given").ToString() });
                    embedFields.Add(new EmbedFieldBuilder { Name = "Thumbnail", IsInline = true, Value = (keys.results[0].header.thumbnail ?? "Not given").ToString() });
                    embedFields.Add(new EmbedFieldBuilder { Name = "Index", IsInline = true, Value = (keys.results[0].header.index_id ?? "ID Not given").ToString() + "\n" + (keys.results[0].header.index_name ?? "Name Not given").ToString() });

                    embedFields.Add(new EmbedFieldBuilder { Name = "Title", IsInline = true, Value = (keys.results[0].data.title ?? "Not given").ToString() });
                    embedFields.Add(new EmbedFieldBuilder { Name = "Extra Links", IsInline = true, Value = (String.Join(", ", (JArray)keys.results[0].data.ext_urls) ?? "Not given").ToString() });

                    EmbedBuilder embedd = GetTemplate(keys, "SauceNAO of given image - Detailed output", embedFields);

                    await ReplyAsync("", false, embedd.Build());
                }
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

            private string GetRequestsLeft(dynamic keys, RequestsLeftType type)
            {
                int short_remain = ((JObject)keys.header).Value<int>("short_remaining");
                int long_remain =  ((JObject)keys.header).Value<int>("long_remaining");

                switch (type)
                {
                    case RequestsLeftType.Message:
                        {
                            return $"> *I have __{short_remain} requests__ left for the next __30 seconds__, " +
                                   $"and __{long_remain} requests__ left for the next __24 hours__.*\n\n" +
                                   $"> ***__Please be respectful to the developers of SauceNAO and their API,__***\n> ***__and make sure others who have the bot can use this command.__***";
                        }
                    case RequestsLeftType.Embed:
                        {
                            return $"*I have __{short_remain} requests__ left for the next __30 seconds__, " +
                                   $"and __{long_remain} requests__ left for the next __24 hours__.* " +
                                   $"***__Please be respectful to the developers of SauceNAO and their API, and make sure others who have the bot can use this command.__***";
                        }
                    case RequestsLeftType.NoFormatting:
                    default:
                        {
                            return $"I have {short_remain} requests left for the next 30 seconds, " +
                                   $"and {long_remain} requests left for the next 24 hours.\n" +
                                   $"Please be respectful to the developers of SauceNAO and their API, and make sure others who have the bot can use this command.";
                        }
                }
            }

            private async Task BuildSauceNAOError(dynamic keys, ICommandContext Context)
            {
                string requestsLeft = GetRequestsLeft(keys, RequestsLeftType.NoFormatting);

                await Util.Error.BuildError($"SauceNAO returned an error:\n\n{Util.String.StripHTML(keys.header.message.ToString())}\n\n[NOTE]\n{requestsLeft}", Context);
            }

            private EmbedBuilder GetTemplate(dynamic keys, string title, List<EmbedFieldBuilder> embedFields)
            {
                return new EmbedBuilder()
                {
                    Title = title,
                    Description = GetRequestsLeft(keys, RequestsLeftType.Embed),
                    Color = Discord.Color.LighterGrey,
                    ThumbnailUrl = Program.xuClient.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder
                    {
                        Text = Util.Globals.EmbedFooter,
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = embedFields
                };
            }
        }
    }
}
