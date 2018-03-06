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
        public static Embed CompEmbed(string engine, string description, string highlight_js_lang) {
            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "**Interperter/Language:** `" + engine + "`",
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
                builder.AppendLine($"{keys[i]} = {values[i]}");
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

                    Process code_handler = Process.Start(Environment.CurrentDirectory + "\\code-handler\\xubot-code-handler.exe", "js " + _eval + "");

                    string uri = Path.GetTempPath() + "InterpResult.xubot";

                    code_handler.WaitForExit(_timeout * 1000);

                    if (!code_handler.HasExited)
                    {
                        code_handler.Kill();
                        _result = _timeout + " seconds past w/o result.";
                        await ReplyAsync("", false, CompEmbed("Jurassic/Javascript","using Jurassic","js"));
                    }
                    else
                    {
                        if (File.Exists(uri))
                        {
                            _result = File.ReadAllText(uri);
                            File.Delete(uri);

                            await ReplyAsync("", false, CompEmbed("Jurassic/Javascript","using Jurassic","js"));
                        }
                        else
                        {
                            _result = "Result was not stored.";
                            await ReplyAsync("", false, CompEmbed("Jurassic/Javascript","using Jurassic","js"));
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

                    Process code_handler = Process.Start(Environment.CurrentDirectory + "\\code-handler\\xubot-code-handler.exe", "lua " + _eval + "");

                    string uri = Path.GetTempPath() + "InterpResult.xubot";

                    code_handler.WaitForExit(_timeout * 1000);

                    if (!code_handler.HasExited)
                    {
                        code_handler.Kill();
                        _result = _timeout + " seconds past w/o result.";
                        await ReplyAsync("", false, CompEmbed("NLua/Lua","using NLUA","lua"));
                    }
                    else
                    {
                        if (File.Exists(uri))
                        {
                            _result = File.ReadAllText(uri);
                            File.Delete(uri);

                            await ReplyAsync("", false, CompEmbed("NLua/Lua","using NLUA","lua"));
                        }
                        else
                        {
                            _result = "Result was not stored.";
                            await ReplyAsync("", false, CompEmbed("NLua/Lua","using NLUA","lua"));
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
                            await ReplyAsync("", false, CompEmbed("Powershell","Powershell","powershell"));
                        }
                        else
                        {
                            _result = psout;
                            await ReplyAsync("", false, CompEmbed("Powershell","Powershell","powershell"));
                        }
                    }
                    else
                    {
                        _result = CompileTools.PowershellDangerous(eval);
                        await ReplyAsync("", false, CompEmbed("Powershell","Powershell","powershell"));
                    }
                });
            }
        }
    }

    public class CompileTools
    {
        public static string PowershellDangerous(string input)
        {
            if (input.ToLower().Contains("start-process") || input.ToLower().Contains("system.diagnostics.process"))
            {
                return "Starting processess is disallowed.";
            }
            else if (input.ToLower().Contains("keys.json"))
            {
                return "Interacting with my API keys is disallowed.";
            }
            else if (input.ToLower().Contains("delete") || input.ToLower().Contains("remove-item"))
            {
                return "Deleting/removing anything is disallowed.";
            }
            else if (input.ToLower().Contains("rename-item") || input.ToLower().Contains("cpi") || input.ToLower().Contains("ren"))
            {
                return "Renaming files are disallowed.";
            }
            else if (input.ToLower().Contains("move-item") || input.ToLower().Contains("mi") || input.ToLower().Contains("mv") || input.ToLower().Contains("move"))
            {
                return "Moving files are disallowed.";
            }
            else if (input.ToLower().Contains("copy-item") || input.ToLower().Contains("cpi") || input.ToLower().Contains("cp") || input.ToLower().Contains("copy"))
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
            else if (input.ToLower().Contains("get-item") || input.ToLower().Contains("gu"))
            {
                return "Changing my computer's date is not nice. So stop it.";
            }
            else
            {
                return "";
            }
        }
    }
}
