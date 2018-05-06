using Discord.Commands;
using Discord;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xubot;
using System.IO;

namespace xubot.src
{
    public class DictionaryComm : ModuleBase
    {
        public static string result = "";
        public static string text = "";

        public struct DictInputs
        {
            public string langID;
            public string word;
            public string filters;
        }

        static string AssembleURL(DictInputs inputs)
        {
            return "https://od-api.oxforddictionaries.com:443/api/v1/entries/" + inputs.langID.ToLower() + "/" + inputs.word.ToLower() + "/" + inputs.filters.ToLower();
        }

        public static Task GetJSON(DictInputs inputs)
        {
            WebClient dicWeb = new WebClient();

            dicWeb.Headers.Add("Accept", "application/json");
            dicWeb.Headers.Add("app_id: " + Program.keys.oxford_dic_id);
            dicWeb.Headers.Add("app_key: " + Program.keys.oxford_dic_key);

            text = dicWeb.DownloadString(AssembleURL(inputs));
            return Task.CompletedTask;
        }

        //AssembleURL("en", "", "definitions")

        [Command("define")]
        public async Task dictionary_comm(string _word, string _langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    langID = _langID,
                    word = _word,
                    filters = "definitions"
                });

                dynamic keys = JObject.Parse(text);

                string _first_def = keys.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0].ToString();

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Oxford Dictionary API",
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
                                Name = "Word / Region",
                                Value = _word + " / " + _langID,
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Definition",
                                Value = _first_def,
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception e) {
                await GeneralTools.CommHandler.BuildError(e, Context);
            }
        }
    }
}
