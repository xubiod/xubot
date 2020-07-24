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

namespace xubot.src.Commands.Connections
{
    [Group("dictionary"), Alias("dict"), Summary("The mini-wrapper for the Oxford Dictionary API.")]
    public class DictionaryComm : ModuleBase
    {
        public static string result = "";
        public static string text = "";

        public struct DictInputs
        {
            public string get;
            public string langID;
            public string word;
            public string filters;
        }

        static string AssembleURL(DictInputs inputs)
        {
            if (inputs.langID != null && inputs.word != null)
            {
                return "https://od-api.oxforddictionaries.com:443/api/v1/" + inputs.get.ToLower() + "/" + inputs.langID.ToLower() + "/" + inputs.word.ToLower() + inputs.filters.ToLower();
            }
            else
            {
                return "https://od-api.oxforddictionaries.com:443/api/v1/" + inputs.get.ToLower();
            }
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

        [Command("define", RunMode = RunMode.Async), Summary("Defines a word using the Oxford Dictionary.")]
        public async Task Define(string word, string langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "entries",
                    langID = langID,
                    word = word,
                    filters = "/definitions"
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> allDefinitions = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = word + " / " + langID,
                        IsInline = false
                    }
                };

                string allDefinitionsString = "";
                int count = 1;

                foreach (var key in keys.results[0].lexicalEntries[0].entries[0].senses)
                {
                    allDefinitionsString += "**" + count.ToString() + "**. " + key.definitions[0] + "\n";
                    count++;
                }

                allDefinitions.Add(new EmbedFieldBuilder
                {
                    Name = "Definition(s)",
                    Value = allDefinitionsString,
                    IsInline = false
                });

                //string _first_def = keys.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0].ToString();

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Oxford Dictionary API",
                    Color = Discord.Color.Orange,
                    Description = "Definition(s)",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = allDefinitions
                };

                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }

        //inflections
        [Command("inflection", RunMode = RunMode.Async), Summary("Shows inflections for a word using the Oxford Dictionary.")]
        public async Task Inflections(string word, string langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "inflections",
                    langID = langID,
                    word = word,
                    filters = ""
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> allInflections = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = word + " / " + langID,
                        IsInline = false
                    }
                };

                foreach (var inflection in keys.results[0].lexicalEntries)
                {
                    allInflections.Add(new EmbedFieldBuilder
                    {
                        Name = "Inflection of: " + inflection.inflectionOf[0].id,
                        Value = "Type: " + inflection.grammaticalFeatures[0].text + "\n" +
                                "Kind: " + inflection.grammaticalFeatures[0].type,
                        //grammaticalFeatures[0].text
                        //grammaticalFeatures[0].type
                        //inflectionOf[0].id
                        IsInline = false
                    });
                }

                //string _first_def = keys.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0].ToString();

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Oxford Dictionary API",
                    Color = Discord.Color.Orange,
                    Description = "Inflections",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = allInflections
                };

                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }

        //words with same meanings
        [Command("synonyms", RunMode = RunMode.Async), Alias("syn"), Summary("Gives a list of synonyms a word using the Oxford Dictionary.")]
        public async Task Syn(string word, string langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "entries",
                    langID = langID,
                    word = word,
                    filters = "/synonyms"
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> allSynonyms = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = word + " / " + langID,
                        IsInline = false
                    }
                };

                string allSynonymsString = "";

                foreach (var sense in keys.results[0].lexicalEntries[0].entries[0].senses)
                {
                    foreach (var subsense in sense.subsenses)
                    {
                        foreach (var synonym in subsense.synonyms)
                        {
                            allSynonymsString += "" + synonym.text + ", ";
                        }
                    }
                }

                allSynonymsString = allSynonymsString.Remove(allSynonymsString.Length - 2);

                allSynonyms.Add(new EmbedFieldBuilder
                {
                    Name = "Synonym(s)",
                    Value = allSynonymsString,
                    IsInline = false
                });

                //string _first_def = keys.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0].ToString();

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Oxford Dictionary API",
                    Color = Discord.Color.Orange,
                    Description = "Synonym(s)",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = allSynonyms
                };

                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception e)
            {
                //await GeneralTools.CommHandler.BuildError(e, Context);
                await ReplyAsync("Either that word doesn't exist in the dictionary or it has no synonyms.");
            }
        }

        //words with opposite meanings
        [Command("antonyms", RunMode = RunMode.Async), Alias("ant"), Summary("Gives a list of antonyms a word using the Oxford Dictionary.")]
        public async Task Ant(string word, string langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "entries",
                    langID = langID,
                    word = word,
                    filters = "/antonyms"
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> allAntonyms = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = word + " / " + langID,
                        IsInline = false
                    }
                };

                string allAntonymsString = "";

                foreach (var sense in keys.results[0].lexicalEntries[0].entries[0].senses)
                {
                    foreach (var antonyms in sense.antonyms)
                    {
                        allAntonymsString += "" + antonyms.text + ", ";
                    }
                }

                allAntonymsString = allAntonymsString.Remove(allAntonymsString.Length - 2);

                allAntonyms.Add(new EmbedFieldBuilder
                {
                    Name = "Antonyms(s)",
                    Value = allAntonymsString,
                    IsInline = false
                });

                //string _first_def = keys.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0].ToString();

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Oxford Dictionary API",
                    Color = Discord.Color.Orange,
                    Description = "Antonyms(s)",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = allAntonyms
                };

                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception e)
            {
                //await GeneralTools.CommHandler.BuildError(e, Context);
                await ReplyAsync("Either that word doesn't exist in the dictionary or it has no antonyms.");
            }
        }

        //dictionary list
        [Command("list", RunMode = RunMode.Async), Summary("Gives a list of supported languages for the Oxford Dictionary.")]
        public async Task List()
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "languages"
                });

                dynamic keys = JObject.Parse(text);

                List<string> allMonolingualDicts = new List<string>();
                List<string> allBilingualDicts = new List<string>();

                foreach (var key in keys.results)
                {
                    if (key.targetLanguage != null)
                    {
                        allBilingualDicts.Add((key.source + " (" + key.sourceLanguage.language + " (**" + key.sourceLanguage.id + "**) => " + key.targetLanguage.language + " (**" + key.targetLanguage.id + "**))\n").ToString());
                    }
                    else
                    {
                        allMonolingualDicts.Add((key.source + " (" + key.sourceLanguage.language + " (**" + key.sourceLanguage.id + "**))\n").ToString());
                    }
                }

                //string _first_def = keys.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0].ToString();

                string monolingualListString0 = "";
                string monolingualListString1 = "";

                string bilingualListString0 = "";
                string bilingualListString1 = "";

                int count = 0;

                foreach (var dictionary in allMonolingualDicts)
                {
                    if (count < 10)
                    {
                        monolingualListString0 += dictionary;
                    }
                    else
                    {
                        monolingualListString1 += dictionary;
                    }
                    count++;
                }

                count = 0;

                foreach (var dictionary in allBilingualDicts)
                {
                    if (count < 10)
                    {
                        bilingualListString0 += dictionary;
                    }
                    else
                    {
                        bilingualListString1 += dictionary;
                    }
                    count++;
                }

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Oxford Dictionary API",
                    Color = Discord.Color.Orange,
                    Description = "Dictionaries: Complete List (IDs bolded)",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>() {
                            new EmbedFieldBuilder {
                                Name = "Monolingual (pt 1)",
                                Value = monolingualListString0,
                                IsInline = true
                            },
                            new EmbedFieldBuilder {
                                Name = "Monolingual (pt 2)",
                                Value = monolingualListString1,
                                IsInline = true
                            },
                            new EmbedFieldBuilder {
                                Name = "Bilingual (pt 1)",
                                Value = bilingualListString0,
                                IsInline = true
                            },
                            new EmbedFieldBuilder {
                                Name = "Bilingual (pt 2)",
                                Value = bilingualListString1,
                                IsInline = true
                            }
                    }
                };

                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception e)
            {
                await Util.Error.BuildError(e, Context);
            }
        }
    }
}
