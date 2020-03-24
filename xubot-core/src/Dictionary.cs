using Discord.Commands;
using Discord;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xubot_core;
using System.IO;

namespace xubot_core.src
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
        public async Task Define(string _word, string _langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "entries",
                    langID = _langID,
                    word = _word,
                    filters = "/definitions"
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> alldef = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = _word + " / " + _langID,
                        IsInline = false
                    }
                };

                string _all_def_str = "";
                int _count = 1;

                foreach (var _key in keys.results[0].lexicalEntries[0].entries[0].senses)
                {
                    _all_def_str += "**" + _count.ToString() + "**. " + _key.definitions[0] + "\n";
                    _count++;
                }

                alldef.Add(new EmbedFieldBuilder
                {
                    Name = "Definition(s)",
                    Value = _all_def_str,
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
                    Fields = alldef
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
        public async Task Inflections(string _word, string _langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "inflections",
                    langID = _langID,
                    word = _word,
                    filters = ""
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> alldef = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = _word + " / " + _langID,
                        IsInline = false
                    }
                };

                foreach (var _def in keys.results[0].lexicalEntries)
                {
                    alldef.Add(new EmbedFieldBuilder
                    {
                        Name = "Inflection of: " + _def.inflectionOf[0].id,
                        Value = "Type: " + _def.grammaticalFeatures[0].text + "\n" +
                                "Kind: " + _def.grammaticalFeatures[0].type,
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
                    Fields = alldef
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
        public async Task Syn(string _word, string _langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "entries",
                    langID = _langID,
                    word = _word,
                    filters = "/synonyms"
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> alldef = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = _word + " / " + _langID,
                        IsInline = false
                    }
                };

                string _all_def_str = "";

                foreach (var _key in keys.results[0].lexicalEntries[0].entries[0].senses)
                {
                    foreach (var _subsenses in _key.subsenses)
                    {
                        foreach (var _syn in _subsenses.synonyms)
                        {
                            _all_def_str += "" + _syn.text + ", ";
                        }
                    }
                }

                _all_def_str = _all_def_str.Remove(_all_def_str.Length - 2);

                alldef.Add(new EmbedFieldBuilder
                {
                    Name = "Synonym(s)",
                    Value = _all_def_str,
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
                    Fields = alldef
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
        public async Task Ant(string _word, string _langID = "en")
        {
            try
            {
                await GetJSON(new DictInputs
                {
                    get = "entries",
                    langID = _langID,
                    word = _word,
                    filters = "/antonyms"
                });

                dynamic keys = JObject.Parse(text);

                List<EmbedFieldBuilder> alldef = new List<EmbedFieldBuilder>
                {
                    new EmbedFieldBuilder
                    {
                        Name = "Word / Region",
                        Value = _word + " / " + _langID,
                        IsInline = false
                    }
                };

                string _all_def_str = "";

                foreach (var _key in keys.results[0].lexicalEntries[0].entries[0].senses)
                {
                    foreach (var _syn in _key.antonyms)
                    {
                        _all_def_str += "" + _syn.text + ", ";
                    }
                }

                _all_def_str = _all_def_str.Remove(_all_def_str.Length - 2);

                alldef.Add(new EmbedFieldBuilder
                {
                    Name = "Antonyms(s)",
                    Value = _all_def_str,
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
                    Fields = alldef
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

                List<string> _all_mono = new List<string>();
                List<string> _all_bi = new List<string>();

                foreach (var _key in keys.results)
                {
                    if (_key.targetLanguage != null)
                    {
                        _all_bi.Add((_key.source + " (" + _key.sourceLanguage.language + " (**" + _key.sourceLanguage.id + "**) => " + _key.targetLanguage.language + " (**" + _key.targetLanguage.id + "**))\n").ToString());
                    }
                    else
                    {
                        _all_mono.Add((_key.source + " (" + _key.sourceLanguage.language + " (**" + _key.sourceLanguage.id + "**))\n").ToString());
                    }
                }

                //string _first_def = keys.results[0].lexicalEntries[0].entries[0].senses[0].definitions[0].ToString();

                string _m_01 = "";
                string _m_02 = "";

                string _b_01 = "";
                string _b_02 = "";

                int _c = 0;

                foreach (var _q in _all_mono)
                {
                    if (_c < 10)
                    {
                        _m_01 += _q;
                    }
                    else
                    {
                        _m_02 += _q;
                    }
                    _c++;
                }

                _c = 0;

                foreach (var _q in _all_bi)
                {
                    if (_c < 10)
                    {
                        _b_01 += _q;
                    }
                    else
                    {
                        _b_02 += _q;
                    }
                    _c++;
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
                                Value = _m_01,
                                IsInline = true
                            },
                            new EmbedFieldBuilder {
                                Name = "Monolingual (pt 2)",
                                Value = _m_02,
                                IsInline = true
                            },
                            new EmbedFieldBuilder {
                                Name = "Bilingual (pt 1)",
                                Value = _b_01,
                                IsInline = true
                            },
                            new EmbedFieldBuilder {
                                Name = "Bilingual (pt 2)",
                                Value = _b_02,
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
