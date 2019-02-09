using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;

namespace xubot_core.src
{
    public class MarkovComm : ModuleBase
    {
        public static TextMarkovChain xuMarkov = new TextMarkovChain();
        //public static List<string> includeTypes = new List<string>() { ".txt", ".log" };

        //use this later
        //Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");

        [Command("markov", RunMode = RunMode.Async), Summary("Generates a sentence based on the markov chain.")]
        public async Task outputMarkov()
        {
            if (!xuMarkov.readyToGenerate())
            {
                await ReplyAsync("I'm not ready!!!");
                return;
            }

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

        [Command("markov", RunMode = RunMode.Async), Summary("Inputs a string into the markov chain.")]
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
                if (valid && !ContainsHTML(input))
                {
                    xuMarkov.feed(input);

                    await ReplyAsync("Learned string.");
                }
                else if (!valid)
                {
                    await ReplyAsync("Hey... this has control characters in it! Stop it!");
                }
                else if (ContainsHTML(input))
                {
                    await ReplyAsync("Hey... this has HTML! It makes me stupid so no!");
                }
                else
                {
                    await ReplyAsync("I have no clue what you did but I don't like it.");
                }
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("markov?i", RunMode = RunMode.Async), Summary("Imports a file into the markov chain. Does not import files with control characters.")]
        public async Task importMarkov()
        {
            try
            {
                string url = GeneralTools.ReturnAttachmentURL(Context);
                await GeneralTools.DownloadAttachmentAsync(Path.Combine(Path.GetTempPath(), "markov.txt"), url);

                string input = File.ReadAllText(Path.Combine(Path.GetTempPath(), "markov.txt"));

                bool valid = true;
                List<char> charArray = input.ToList();

                foreach (char _p in charArray)
                {
                    if (Char.IsControl(_p) && _p != '\r' && _p != '\n')
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid && !ContainsHTML(input))
                {
                    xuMarkov.feed(input);

                    await ReplyAsync("Learned from file.");
                }
                else if (!valid)
                {
                    await ReplyAsync("Hey... this has control characters in it! Stop it!");
                }
                else if (ContainsHTML(input))
                {
                    await ReplyAsync("Hey... this has HTML! It makes me stupid so no!");
                }
                else
                {
                    await ReplyAsync("I have no clue what you did but I don't like it.");
                }
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("markov?export", RunMode = RunMode.Async), Alias("markov?e"), Summary("Exports the markov chain as a XML file.")]
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

        [Command("markov?r"), Summary("Outputs a sentence from the markov chain and refeeds it into the chain.")]
        public async Task outputMarkovRecur()
        {
            if (!xuMarkov.readyToGenerate())
            {
                await ReplyAsync("I'm not ready!!!");
                return;
            }

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

        [Command("markov?flush")]
        public async Task flush()
        {
            throw new NotImplementedException();

            if (GeneralTools.UserTrusted(Context))
            {
                xuMarkov.flush();
                await ReplyAsync("Markov chain flushed.");
            }
            else
            {
                await ReplyAsync("Only trusted users can flush the chain.");
            }
        }

        public static bool ContainsHTML(string input)
        {
            return new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>").IsMatch(input);
        }
    }
}
