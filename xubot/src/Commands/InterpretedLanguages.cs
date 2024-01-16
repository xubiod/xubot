using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using xubot.Attributes;

namespace xubot.Commands;

public class InterpretedLanguages : ModuleBase
{
    public static Embed BuildEmbed(ICommandContext context, string language, string description, string syntaxHighlighting, string input, string result)
    {
        var embed = Util.Embed.GetDefaultEmbed(context, "**Language:** `" + language + "`", description, Color.Orange);
        embed.Fields =
        [
            new()
            {
                Name = "Input",
                Value = "```" + syntaxHighlighting + "\n" + input + "```",
                IsInline = false
            },

            new()
            {
                Name = "Result",
                Value = "```\n" + result + "```",
                IsInline = false
            }
        ];

        return embed.Build();
        //await ReplyAsync("", false, embed);
    }

/*
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
*/

    [Group("interpret"), Summary("Interprets other languages and displays output.")]
    public class CodeCompile : ModuleBase
    {
        [Command("js", RunMode = RunMode.Async), Summary("Executes JavaScript.")]
        public async Task Js(string input)
        {
            await ReplyAsync("I'm sorry, but the .NET Core port does not have this yet. I'm trying my best to make it work out, promise! (but don't count on it soon)\n\n- xubiod#0258");
            /*
            string local_result;
            int _timeout = 15;

            Process code_handler = Process.Start(Environment.CurrentDirectory + "\\code-handler\\xubot-code-compiler.exe", "js " + eval);

            string uri = Path.GetTempPath() + "InterpretedResult.xubot";

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
        public async Task Lua(string input)
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

            string uri = Path.GetTempPath() + "InterpretedResult.xubot";

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

        // ReSharper disable once StringLiteralTypo
        [Example("diissisdo")]
        [Command("deadfish", RunMode = RunMode.Async), Summary("Interprets Deadfish and outputs the results.")]
        public async Task Deadfish(string input)
        {
            var result = SmallLangInterpreters.Deadfish.Execute(input);
            await ReplyAsync("", false, BuildEmbed(Context, "Deadfish", "using a built-in interpreter (adapted from https://esolangs.org)", "", input, result));
        }

        [Example("++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.")]
        [Command("brainfuck", RunMode = RunMode.Async), Alias("brainf***", "brainf**k", "b****fuck", "bf"), Summary("Interprets Brainfuck and outputs the result.")]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task Brainfuck(string input, string asciiInput = "")
        {
            string embedInput;
            if (string.IsNullOrEmpty(asciiInput))
            {
                embedInput = $"Code: {input.Replace("\n", String.Empty)}\n\nASCII Input: {asciiInput}";
            }
            else
            {
                embedInput = $"Code: {input.Replace("\n", String.Empty)}";
            }
            var result = SmallLangInterpreters.Brainfuck.Execute(input, asciiInput);

            await ReplyAsync("", false, BuildEmbed(Context, "Brainfuck", "using a built-in interpreter (adapted from https://github.com/james1345-1/Brainfuck/)", "bf", embedInput, result));
        }
    }
}