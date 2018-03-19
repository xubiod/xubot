using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

#pragma warning disable CS4014

namespace xubot.src
{
    public class MarkovComm : ModuleBase
    {
        public static TextMarkovChain xuMarkov = new TextMarkovChain();
        //public static List<string> includeTypes = new List<string>() { ".txt", ".log" };

        [Command("markov", RunMode = RunMode.Async)]
        public async Task outputMarkov()
        {
            try
            {
                string generated = xuMarkov.generateSentence();

                if (generated.Length > 1000)
                {
                    generated = generated.Substring(0, 1000) + "[...]";
                }

                await ReplyAsync(generated);
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("markov", RunMode = RunMode.Async)]
        public async Task outputMarkov(string input)
        {
            try
            {
                bool valid = true;
                List<char> charArray = input.ToList();

                foreach (char _p in charArray)
                {
                    if (Char.IsControl(_p) && _p != '\r' && _p != '\n') { valid = false; break; }
                }
                if (valid)
                {
                    xuMarkov.feed(input);

                    await ReplyAsync("Learned string.");
                }
                else
                {
                    await ReplyAsync("Hey... this has control characters in it! Stop it!");
                }
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("markov?i", RunMode = RunMode.Async)]
        public async Task importMarkov()
        {
            try
            {
                string url = GeneralTools.ReturnAttachmentURL(Context);
                await GeneralTools.DownloadAttachmentAsync(Path.Combine(Path.GetTempPath(), "markov.txt"), url);

                string fileContents = File.ReadAllText(Path.Combine(Path.GetTempPath(), "markov.txt"));

                bool valid = true;
                List<char> charArray = fileContents.ToList();

                foreach (char _p in charArray)
                {
                    if (Char.IsControl(_p) && _p != '\r' && _p != '\n')
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    xuMarkov.feed(fileContents);

                    await ReplyAsync("Learned from file.");
                }
                else
                {
                    await ReplyAsync("Hey... this has control characters in it! Stop it!");
                }
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("markov?export"), Alias("markov?e")]
        public async Task exportMarkov()
        {
            try
            {
                xuMarkov.save(Path.GetTempPath() + "XubotMarkov.xml");

                await Context.Channel.SendFileAsync(Path.GetTempPath() + "XubotMarkov.xml");
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("markov?r")]
        public async Task outputMarkovRecur()
        {
            try
            {
                string output = xuMarkov.generateSentence();
                await ReplyAsync(output);

                xuMarkov.feed(output);
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

    }
}
