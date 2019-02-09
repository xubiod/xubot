using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

//from trey wilson
//https://github.com/treydwilson/MarkovChainSentenceGenerator/tree/master/TextMarkovChains
namespace xubot_core.src
{
    public class TextMarkovChain
    {
        private Dictionary<string, Chain> chains;
        private Chain head;

        public TextMarkovChain()
        {
            chains = new Dictionary<string, Chain>();
            head = new Chain("[]");
            chains.Add("[]", head);
        }

        public void feed(string s)
        {
            s = s.ToLower();
            s = s.Replace('/', ' ').Replace(',', ' ').Replace("[]", "");
            s = s.Replace(".", " .").Replace("!", " !").Replace("?", " ?");
            string[] splitValues = s.Split(' ');

            addWord("[]", splitValues[0]);

            for (int i = 0; i < splitValues.Length - 1; i++)
            {
                if (splitValues[i] == "." ||
                    splitValues[i] == "?" ||
                    splitValues[i] == "!")
                    addWord("[]", splitValues[i + 1]);
                else
                    addWord(splitValues[i], splitValues[i + 1]);
            }
        }

        private void addWord(string prev, string next)
        {
            if (chains.ContainsKey(prev) && chains.ContainsKey(next))
                chains[prev].addWord(chains[next]);
            else if (chains.ContainsKey(prev))
            {
                chains.Add(next, new Chain(next));
                chains[prev].addWord(chains[next]);
            }
        }

        public void feed(XmlDocument xd)
        {
            XmlNode root = xd.ChildNodes[0];
            foreach (XmlNode n in root.ChildNodes)
            {
                //First add all chains that are not there already
                Chain nc = new Chain(n);
                if (!chains.ContainsKey(nc.word))
                    chains.Add(nc.word, nc);
            }

            foreach (XmlNode n in root.ChildNodes)
            {
                //Now that all words have been added, we can add the probabilities
                XmlNode nextChains = n.ChildNodes[0];
                Chain current = chains[n.Attributes["Word"].Value.ToString()];
                foreach (XmlNode nc in nextChains)
                {
                    Chain c = chains[nc.Attributes["Word"].Value.ToString()];
                    current.addWord(c, Convert.ToInt32(nc.Attributes["Count"].Value));
                }
            }
        }

        public void save(string path)
        {
            XmlDocument xd = getDataAsXML();
            xd.Save(path);
        }

        public XmlDocument getDataAsXML()
        {
            XmlDocument xd = new XmlDocument();
            XmlElement root = xd.CreateElement("Chains");
            xd.AppendChild(root);

            foreach (string key in chains.Keys)
                root.AppendChild(chains[key].getXMLElement(xd));

            return xd;
        }

        public bool readyToGenerate()
        {
            return head.getNextChain() != null;
        }

        public string generateSentence()
        {
            StringBuilder s = new StringBuilder();
            Chain nextString = head.getNextChain();
            while (nextString.word != "!" && nextString.word != "?" && nextString.word != ".")
            {
                s.Append(nextString.word);
                s.Append(" ");
                nextString = nextString.getNextChain();
                if (nextString == null)
                    return s.ToString();
            }

            s.Append(nextString.word); //Add punctuation at end

            s[0] = char.ToUpper(s[0]);

            return s.ToString();
        }

        //xubiod's additions BEGIN
        public void flush()
        {
            chains.Clear();
            head.flush();
        }
        //END

        private class Chain
        {
            public string word;

            private Dictionary<string, ChainProbability> chains;
            private int fullCount;

            //more xubiod additions
            public void flush()
            {
                chains.Clear();
            }
            //end

            public Chain(string w)
            {
                word = w;
                chains = new Dictionary<string, ChainProbability>();
                fullCount = 0;
            }

            public Chain(XmlNode node)
            {
                word = node.Attributes["Word"].Value.ToString();
                fullCount = 0;  //Full Count is stored, but this will be loaded when adding new words to the chain.  Default to 0 when loading XML
                chains = new Dictionary<string, ChainProbability>();
            }

            public void addWord(Chain chain, int increase = 1)
            {
                fullCount += increase;
                if (chains.ContainsKey(chain.word))
                    chains[chain.word].count += increase;
                else
                    chains.Add(chain.word, new ChainProbability(chain, increase));
            }

            public Chain getNextChain()
            {
                //Randomly get the next chain
                //Trey:  As this gets bigger, this is a remarkably inefficient way to randomly get the next chain.
                //The reason it is implemented this way is it allows new sentences to be read in much faster
                //since it will not need to recalculate probabilities and only needs to add a counter.  I don't
                //believe the tradeoff is worth it in this case.  I need to do a timed evaluation of this and decide.
                int currentCount = RandomHandler.random.Next(fullCount);
                foreach (string key in chains.Keys)
                {
                    for (int i = 0; i < chains[key].count; i++)
                    {
                        if (currentCount == 0)
                            return chains[key].chain;
                        currentCount--;
                    }
                }
                return null;
            }

            public XmlElement getXMLElement(XmlDocument xd)
            {
                XmlElement e = xd.CreateElement("Chain");
                e.SetAttribute("Word", this.word);
                e.SetAttribute("FullCount", this.fullCount.ToString());

                XmlElement nextChains = xd.CreateElement("NextChains");
                XmlElement nextChain;

                foreach (string key in chains.Keys)
                {
                    nextChain = xd.CreateElement("Chain");
                    nextChain.SetAttribute("Word", chains[key].chain.word);
                    nextChain.SetAttribute("Count", chains[key].count.ToString());
                    nextChains.AppendChild(nextChain);
                }

                e.AppendChild(nextChains);

                return e;
            }
        }

        private class ChainProbability
        {
            public Chain chain;
            public int count;

            public ChainProbability(Chain chain, int count)
            {
                this.chain = chain;
                this.count = count;
            }
        }
    }

    public static class RandomHandler
    {
        //Handles the global random object
        private static System.Random _random;
        public static System.Random random
        {
            get
            {
                if (_random == null)
                    _random = new Random();

                return _random;
            }
        }
    }

    public class DeepMarkovChain
    {
        /* Order of action:
         * Add first word to header chain
         * Add second word to header chain as a nextWord
         * Add third word as nextWord to first word
         * Add fourth word as nextWord to second word
         * etc. etc.
         */

        private Dictionary<string, DoubleChain> chains;
        private DoubleChain head;

        public DeepMarkovChain()
        {
            chains = new Dictionary<string, DoubleChain>();
            head = new DoubleChain() { text = "[]" };
            chains.Add("[]", head);
        }

        public void feed(string s)
        {
            s = s.ToLower();
            s = s.Replace("/", "").Replace(",", "").Replace("[]", "");
            s = s.Replace("\r", "").Replace("\n", "");
            s = s.Replace(".", " .").Replace("!", " !").Replace("?", " ?");

            string[] splitValues = s.Split(' ');

            if (splitValues.Length >= 2) //Every input should have a word and punctuation
            {
                addWord("[]", splitValues[0], splitValues[1]);

                for (int i = 0; i < splitValues.Length - 2; i++)
                {
                    if (splitValues[i] == "."
                        || splitValues[i] == "!"
                        || splitValues[i] == "?")
                        addWord("[]", splitValues[i + 1], splitValues[i + 2]);
                    else
                        addWord(splitValues[i], splitValues[i + 1], splitValues[i + 2]);
                }
            }
        }

        private void addWord(string prev, string next, string nextNext)
        {
            if (!chains.ContainsKey(prev))
                return;
            if (!chains.ContainsKey(next))
                chains.Add(next, new DoubleChain() { text = next });
            if (!chains.ContainsKey(nextNext))
                chains.Add(nextNext, new DoubleChain() { text = nextNext });

            chains[prev].addWord(chains[next]);
            chains[prev].addNextWord(chains[next], chains[nextNext]);
        }

        public bool readyToGenerate()
        {
            DoubleChain next = head.getNextWord();
            if (next == null)
                return false;
            return head.getNextNextWord(next) != null;
        }

        public string generateSentence()
        {
            StringBuilder s = new StringBuilder();

            DoubleChain currentString = head.getNextWord();
            DoubleChain nextString = head.getNextNextWord(currentString);
            DoubleChain nextNextString = currentString.getNextNextWord(nextString);
            s.Append(currentString.text);
            s.Append(" ");
            s.Append(nextString.text);

            while (nextNextString.text != "!" && nextNextString.text != "?" && nextNextString.text != ".")
            {
                s.Append(" ");
                s.Append(nextNextString.text);
                currentString = nextString;
                nextString = nextNextString;
                nextNextString = currentString.getNextNextWord(nextString);
                if (nextNextString == null)
                    break;
            }

            s.Append(nextNextString.text); //Add punctuation
            s[0] = char.ToUpper(s[0]);

            return s.ToString();
        }

        //Still TODO:  Need to make actual main class  (feed and generate)
        //TODO: Need to add export and import to XML
        //With this implementation I should have an easier time writing decent sentences and doing specific requests
        //In the future I would like to write one that will go n levels deep for each word (This will allow
        //it to be customizable and easier to test the various levels of deepness.  The idea is that if you have
        //a lot of data to work with, deeper chains are better as they will more closely resemble actual sentences.
        //For less data, less depth is good because it allows the computer to improvise a little.
        //I wonder if it would be possible to store multiple sets of data for different amounts of depth?  Then the computer
        //could decide on its own whether to use deep or not deep sets of data.

        private class DoubleChain
        {
            public string text;
            public int fullCount;
            public Dictionary<string, ChainProbability> nextNodes;
            public Dictionary<string, Dictionary<string, ChainProbability>> nextNextNodes;

            public DoubleChain()
            {
                nextNextNodes = new Dictionary<string, Dictionary<string, ChainProbability>>();
                nextNodes = new Dictionary<string, ChainProbability>();
                fullCount = 0;
            }

            public void addWord(DoubleChain c)
            {
                fullCount++;
                if (nextNodes.ContainsKey(c.text))
                    nextNodes[c.text].count++;
                else
                {
                    nextNodes.Add(c.text, new ChainProbability(c, 1));
                    nextNextNodes.Add(c.text, new Dictionary<string, ChainProbability>());
                }
            }

            public void addNextWord(DoubleChain n, DoubleChain nn)
            {
                Dictionary<string, ChainProbability> d = nextNextNodes[n.text];

                if (d.ContainsKey(nn.text))
                    d[nn.text].count++;
                else
                    d.Add(nn.text, new ChainProbability(nn, 1));

                //Add to n as a normal word
                n.addWord(nn);
            }

            public DoubleChain getNextWord()
            {
                int currentCount = RandomHandler.random.Next(fullCount);
                foreach (string key in nextNodes.Keys)
                {
                    for (int i = 0; i < nextNodes[key].count; i++)
                    {
                        if (currentCount == 0)
                            return nextNodes[key].chain;
                        currentCount--;
                    }
                }
                return null;
            }

            public DoubleChain getNextNextWord(DoubleChain c)
            {
                Dictionary<string, ChainProbability> d = nextNextNodes[c.text];
                int fullCount = 0;
                foreach (string key in d.Keys)
                    fullCount += d[key].count;
                int currentCount = RandomHandler.random.Next(fullCount);
                foreach (string key in d.Keys)
                {
                    for (int i = 0; i < d[key].count; i++)
                    {
                        if (currentCount == 0)
                            return d[key].chain;
                        currentCount--;
                    }
                }
                return null;
            }
        }

        private class ChainProbability
        {
            public DoubleChain chain;
            public int count;
            public ChainProbability(DoubleChain c, int co)
            {
                chain = c;
                count = co;
            }
        }
    }
}
