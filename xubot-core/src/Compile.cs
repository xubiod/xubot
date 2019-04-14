using NLua;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using xubot_core.src;

namespace xubot_core.src
{
    public class Compile : ModuleBase
    {
        //public static string _eval = "";
        //public static string _result = "";
        public static string _result_input = "";

        /// @param engine interp engine
        /// @param description erm...
        /// @param highlight_js_lang lang code for hilighting
        public static Embed BuildEmbed(string lang, string description, string highlight_js_lang, string _eval, string _result)
        {
            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "**Language:** `" + lang + "`",
                Color = Discord.Color.Orange,
                Description = description,

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

                                Value = "```" + highlight_js_lang + "\n" + _eval + "```",

                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Result",
                                Value = "```\n" + _result + "```",
                                IsInline = false
                            }
                        }
            };

            return embedd.Build();
            //await ReplyAsync("", false, embedd);
        }

        private static string TableToString(LuaTable t)
        {
            object[] keys = new object[t.Keys.Count];
            object[] values = new object[t.Values.Count];
            t.Keys.CopyTo(keys, 0);
            t.Values.CopyTo(values, 0);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < keys.Count(); i++)
            {
                builder.AppendLine($"{keys[i]} = {values[i]} ");
            }

            return builder.ToString();
        }

        [Group("interp"), Summary("Interperts other languages and displays output.")]
        public class codeCompile : ModuleBase
        {
            [Command("js", RunMode = RunMode.Async), Summary("Executes JavaScript.")]
            public async Task js(string eval)
            {
                await ReplyAsync("I'm sorry, but the .NET Core port does not have this yet. I'm trying my best to make it work out, promise! (but don't count on it soon)\n\n- xubiod#0258");
                /*
                string local_result;
                int _timeout = 15;

                Process code_handler = Process.Start(Environment.CurrentDirectory + "\\code-handler\\xubot-code-compiler.exe", "js " + eval);

                string uri = Path.GetTempPath() + "InterpResult.xubot";

                code_handler.WaitForExit(_timeout * 1000);

                if (!code_handler.HasExited)
                {
                    code_handler.Kill();
                    local_result = _timeout + " seconds past w/o result.";
                    await ReplyAsync("", false, BuildEmbed("Javascript", "using Jurassic", "js", eval, local_result));
                }
                else
                {
                    if (File.Exists(uri))
                    {
                        local_result = File.ReadAllText(uri);
                        File.Delete(uri);

                        await ReplyAsync("", false, BuildEmbed("Javascript", "using Jurassic", "js", eval, local_result));
                    }
                    else
                    {
                        local_result = "Result was not stored.";
                        await ReplyAsync("", false, BuildEmbed("Javascript", "using Jurassic", "js", eval, local_result));
                    }
                }
                */
            }

            [Command("lua", RunMode = RunMode.Async), Summary("Executes Lua with some restrictions.")]
            public async Task lua(string eval)
            {
                await ReplyAsync("I'm sorry, but the .NET Core port does not have this yet. I'm trying my best to make it work out, promise! (but don't count on it soon)\n\n- xubiod#0258");
                /*
                
                if (!GeneralTools.UserTrusted(Context))
                {
                    await ReplyAsync("User is not trusted.");
                    return;
                }

                string local_result;
                int _timeout = 15;

                Process code_handler = Process.Start(Environment.CurrentDirectory + "\\code-handler\\xubot-code-compiler.exe", "lua " + eval);

                string uri = Path.GetTempPath() + "InterpResult.xubot";

                code_handler.WaitForExit(_timeout * 1000);

                if (!code_handler.HasExited)
                {
                    code_handler.Kill();
                    local_result = _timeout + " seconds past w/o result.";
                    await ReplyAsync("", false, BuildEmbed("Lua", "using NLua", "lua", eval, local_result));
                }
                else
                {
                    if (File.Exists(uri))
                    {
                        local_result = File.ReadAllText(uri);
                        File.Delete(uri);

                        await ReplyAsync("", false, BuildEmbed("Lua", "using NLua", "lua", eval, local_result));
                    }
                    else
                    {
                        local_result = "Result was not stored.";
                        await ReplyAsync("", false, BuildEmbed("Lua", "using NLua", "lua", eval, local_result));
                    }
                }*/
            }
            
            [Command("deadfish", RunMode = RunMode.Async), Summary("Interperts Deadfish and outputs the results.")]
            public async Task deadfish(string eval)
            {
                string local_result = SmallLangInterps.Deadfish.Execute(eval);
                await ReplyAsync("", false, BuildEmbed("Deadfish", "using a built-in interpeter (adapted from https://esolangs.org)", "", eval, local_result));
            }

            [Command("brainfuck", RunMode = RunMode.Async), Alias("brainf***", "brainf**k", "b****fuck", "bf"), Summary("Interperts Brainfuck and outputs the result.")]
            public async Task brainfuck(string eval, string ascii_input = "")
            {
                string temp_eval;
                if (ascii_input != "")
                {
                    temp_eval = "Code: " + eval.Replace("\n", String.Empty) + "\n\nASCII Input: " + ascii_input;
                }
                else
                {
                    temp_eval = "Code: " + eval.Replace("\n", String.Empty);
                }
                string local_result = SmallLangInterps.Brainfuck.Execute(eval, ascii_input);

                await ReplyAsync("", false, BuildEmbed("Brainfuck", "using a built-in interpeter (adapted from https://github.com/james1345-1/Brainfuck/)", "bf", temp_eval, local_result));
            }
        }
    }
}
