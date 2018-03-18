using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace xubot.src
{
    public class MarkovComm : ModuleBase
    {
        public static TextMarkovChain xuMarkov = new TextMarkovChain();

        [Command("markov")]
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

        [Command("markov")]
        public async Task outputMarkov(string input)
        {
            try
            {
                xuMarkov.feed(input);

                await ReplyAsync("Learned string.");
            }
            catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("markov?i")]
        public async Task importMarkov()
        {
            try
            {
                string url = GeneralTools.ReturnAttachmentURL(Context);
                await GeneralTools.DownloadAttachmentAsync(Path.Combine(Path.GetTempPath(), "markov.txt"), url);

                string fileContents = File.ReadAllText(Path.Combine(Path.GetTempPath(), "markov.txt"));

                if (fileContents.Length > 1500)
                {
                    xuMarkov.feed(fileContents);

                    await ReplyAsync("Learned from file.");
                } else
                {
                    await ReplyAsync("File has too many characters in it (over 1500). Shorten it.");
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
