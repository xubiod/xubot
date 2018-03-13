using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic;
using NLua;
using Discord;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;

#pragma warning disable CS4014 

namespace xubot
{
    public class Compile : ModuleBase
    {
        public static ScriptEngine jsEngine = new ScriptEngine();

        public static string _eval = "";
        public static string _result = "";
        public static string _result_input = "";

        /// @param engine interp engine
        /// @param description erm...
        /// @param highlight_js_lang lang code for hilighting
        public static Embed BuildEmbed(string lang, string description, string highlight_js_lang) {
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

            return embedd;
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

        [Group("interp")]
        public class codeCompile : ModuleBase
        {
            [Command("js")]
            public async Task js(string eval)
            {
                Task.Run(async () =>
                {

                    _eval = eval;
                    int _timeout = 15;

                    Process code_handler = Process.Start(Environment.CurrentDirectory + "\\code-handler\\xubot-code-compiler.exe", "js " + _eval);

                    string uri = Path.GetTempPath() + "InterpResult.xubot";

                    code_handler.WaitForExit(_timeout * 1000);

                    if (!code_handler.HasExited)
                    {
                        code_handler.Kill();
                        _result = _timeout + " seconds past w/o result.";
                        await ReplyAsync("", false, BuildEmbed("Javascript", "using Jurassic", "js"));
                    }
                    else
                    {
                        if (File.Exists(uri))
                        {
                            _result = File.ReadAllText(uri);
                            File.Delete(uri);

                            await ReplyAsync("", false, BuildEmbed("Javascript", "using Jurassic", "js"));
                        }
                        else
                        {
                            _result = "Result was not stored.";
                            await ReplyAsync("", false, BuildEmbed("Javascript", "using Jurassic", "js"));
                        }
                    }
                });
            }

            [Command("lua")]
            public async Task lua(string eval)
            {
                Task.Run(async () =>
                {
                    _eval = eval;
                    int _timeout = 15;

                    Process code_handler = Process.Start(Environment.CurrentDirectory + "\\code-handler\\xubot-code-compiler.exe", "lua " + _eval);

                    string uri = Path.GetTempPath() + "InterpResult.xubot";

                    code_handler.WaitForExit(_timeout * 1000);

                    if (!code_handler.HasExited)
                    {
                        code_handler.Kill();
                        _result = _timeout + " seconds past w/o result.";
                        await ReplyAsync("", false, BuildEmbed("Lua", "using NLua", "lua"));
                    }
                    else
                    {
                        if (File.Exists(uri))
                        {
                            _result = File.ReadAllText(uri);
                            File.Delete(uri);

                            await ReplyAsync("", false, BuildEmbed("Lua", "using NLua", "lua"));
                        }
                        else
                        {
                            _result = "Result was not stored.";
                            await ReplyAsync("", false, BuildEmbed("Lua", "using NLua", "lua"));
                        }
                    }
                });
            }

            [Command("powershell")]
            public async Task ps(string eval)
            {
                Task.Run(async () =>
                {
                    _eval = eval;

                    if (CompileTools.PowershellDangerous(eval) == "")
                    {
                        int _timeout = 5;

                        Process psproc = new Process();

                        psproc.StartInfo.UseShellExecute = false;
                        psproc.StartInfo.RedirectStandardOutput = true;
                        psproc.StartInfo.FileName = "powershell.exe";
                        psproc.StartInfo.Arguments = "-Command " + eval;
                        psproc.Start();

                        string psout = psproc.StandardOutput.ReadToEnd();
                        psproc.WaitForExit(_timeout * 1000);

                        if (!psproc.HasExited)
                        {
                            psproc.Kill();
                            _result = _timeout + " seconds past w/o result.";
                            await ReplyAsync("", false, BuildEmbed("Powershell", "Using Direct Execution", "powershell"));
                        }
                        else
                        {
                            _result = psout;
                            await ReplyAsync("", false, BuildEmbed("Powershell", "Using Direct Execution", "powershell"));
                        }
                    }
                    else
                    {
                        _result = CompileTools.PowershellDangerous(eval);
                        await ReplyAsync("", false, BuildEmbed("Powershell", "Using Direct Execution", "powershell"));
                    }
                });
            }

            [Command("powershell-sudo")]
            public async Task ps_sudo(string eval)
            {
                Task.Run(async () =>
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.Async = true;

                    bool trusted = false;

                    var xdoc = XDocument.Load("Trusted.xml");

                    var items = from i in xdoc.Descendants("trust")
                                select new
                                {
                                    user = (string)i.Attribute("id")
                                };

                    foreach (var item in items)
                    {
                        if (item.user == Context.Message.Author.Id.ToString())
                        {
                            trusted = true;
                        }
                    }

                    if (trusted)
                    {
                        if (CompileTools.PowershellSudoDangerous(eval) == "")
                        {
                            _eval = eval;

                            int _timeout = 5;

                            Process psproc = new Process();

                            psproc.StartInfo.UseShellExecute = false;
                            psproc.StartInfo.RedirectStandardOutput = true;
                            psproc.StartInfo.FileName = "powershell.exe";
                            psproc.StartInfo.Arguments = "-Command " + eval;
                            psproc.Start();

                            string psout = psproc.StandardOutput.ReadToEnd();
                            psproc.WaitForExit(_timeout * 1000);

                            if (!psproc.HasExited)
                            {
                                psproc.Kill();
                                _result = _timeout + " seconds past w/o result.";
                                await ReplyAsync("", false, BuildEmbed("Powershell", "Using Direct Execution", "powershell"));
                            }
                            else
                            {
                                _result = psout;
                                await ReplyAsync("", false, BuildEmbed("Powershell", "Using Direct Execution", "powershell"));
                            }
                        }
                    } else
                    {
                        await ReplyAsync("You are not a trusted user. Stop trying to do this.");
                    }
                });
            }
        }
    }

    public class CompileTools
    {
        public static string PowershellDangerous(string input)
        {
            if (input.ToLower().Contains("start-process") || input.ToLower().Contains("invoke-item") ||
                input.ToLower().Contains("ii ") || input.ToLower().Contains("system.diagnostics.process") ||
                input.ToLower().Contains("stop-service") || input.ToLower().Contains("spsv ") ||
                input.ToLower().Contains("start-service") || input.ToLower().Contains("sasv ") ||
                input.ToLower().Contains("restart-service") ||
                input.ToLower().Contains("stop-process") || input.ToLower().Contains("spps ") || input.ToLower().Contains("kill ") ||
                input.ToLower().Contains("suspend-service") || input.ToLower().Contains("resume-service") ||
                input.ToLower().Contains("invoke-expression") || input.ToLower().Contains("iex "))
            {
                return "Starting/closing processess is disallowed.";
            }
            else if (input.ToLower().Contains("keys.json"))
            {
                return "Interacting with my API keys is disallowed.";
            }
            else if (input.ToLower().Contains("delete") || input.ToLower().Contains("remove-item"))
            {
                return "Deleting/removing anything is disallowed.";
            }
            else if (input.ToLower().Contains("rename-item") || input.ToLower().Contains("cpi ") ||
                input.ToLower().Contains("ren "))
            {
                return "Renaming files are disallowed.";
            }
            else if (input.ToLower().Contains("move-item") || input.ToLower().Contains("mi ") ||
                input.ToLower().Contains("mv ") || input.ToLower().Contains("move "))
            {
                return "Moving files are disallowed.";
            }
            else if (input.ToLower().Contains("copy-item") || input.ToLower().Contains("cpi ") ||
                input.ToLower().Contains("cp ") || input.ToLower().Contains("copy "))
            {
                return "Copying files are disallowed.";
            }
            else if (input.ToLower().Contains("stop-computer") || input.ToLower().Contains("restart-computer"))
            {
                return "DA FUCK YOU DOING MATE (╯°□°）╯︵ ┻━┻";
            }
            else if (input.ToLower().Contains("set-date"))
            {
                return "Changing my computer's date is not nice. So stop it.";
            }
            else if (input.ToLower().Contains("get-item") || input.ToLower().Contains("gu ") ||
                input.ToLower().Contains("set-executionpolicy") ||
                input.ToLower().Contains("new-alias") || input.ToLower().Contains("nal ") ||
                input.ToLower().Contains("import-alias") || input.ToLower().Contains("ipal ") ||
                input.ToLower().Contains("get-alias") || input.ToLower().Contains("gal ") ||
                input.ToLower().Contains("set-alias") ||
                input.ToLower().Contains("export-alias") || input.ToLower().Contains("epal "))
            {
                return "No. Besides these are (mostly) based on session.";
            }
            else if (input.ToLower().Contains("clear-content") || input.ToLower().Contains("clc "))
            {
                return "Editing things is disallowed.";
            }
            else if (input.ToLower().Contains("new-item") || input.ToLower().Contains("ni "))
            {
                return "Creating things is disallowed.";
            }
            else if (input.ToLower().Contains("cmd") || input.ToLower().Contains("control") || input.ToLower().Contains("wmic") || input.ToLower().Contains("taskmgr") || input.ToLower().Contains("tasklist") || input.ToLower().Contains("del") || input.ToLower().Contains("sc") || input.ToLower().Contains(">") ) {
                    return "Starting executables from PATH to get around blocks (and piping to write a file) is disallowed.";
            }
            else
            {
                return "";
            }
        }
        public static string PowershellSudoDangerous(string input)
        {
            if (input.ToLower().Contains("keys.json"))
            {
                return "Interacting with my API keys is disallowed (even in sudo command).";
            }
            else
            {
                return "";
            }
        }

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

            return embedd;
            //await ReplyAsync("", false, embedd);
        }
    }
}
