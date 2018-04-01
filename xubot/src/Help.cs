using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot
{
    public class Help : ModuleBase
    {

        [Group("help"), Alias("h"), Summary("a calculator, but shittier")]
        public class help : ModuleBase
        {
            [Command]
            public async Task Default(int page = 1)
            {
                EmbedBuilder embedd = null;

                switch (page)
                {
                    case 1:
                        {
                            embedd = new EmbedBuilder
                            {
                                Title = "List of Commands",
                                Color = Discord.Color.Orange,
                                Description = "For extended help on one command, use `[>help [COMM]`.",

                                Footer = new EmbedFooterBuilder
                                {
                                    Text = "xubot :p"
                                },
                                Timestamp = DateTime.UtcNow,
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Actual usable stuff** Page " + page + "/6\n\n" +
                                        "`[>help [COMM]` - Welp, you know about this one.\n" +
                                        "`[>echo [TYPE]` - Repeats what you give it\n" +
                                        "`[>math [TYPE]` - Does stuff that no one likes. At all.\n" +
                                        "`[>settings [COM]` - Bot settings. *You must be the* ***[Owner of the bot]*** *to execute.*\n" +
                                        "`[>insult [COM]` - Tells you something rude.\n" +
                                        "`[>cat` - Cat.\n" +
                                        "`[>compare` - Converts one value to another system.\n",
                                        IsInline = false
                                    }
                                }
                            };
                            break;
                        }
                    case 2:
                        {
                            embedd = new EmbedBuilder
                            {
                                Title = "List of Commands",
                                Color = Discord.Color.Orange,
                                Description = "For extended help on one command, use `[>help [COMM]`.",

                                Footer = new EmbedFooterBuilder
                                {
                                    Text = "xubot :p"
                                },
                                Timestamp = DateTime.UtcNow,
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Actual usable stuff** Page " + page + "/6\n\n" +
                                        "`[>post [SERVICE]` - Posts something to a service.\n" +
                                        "`[>pattern [COM]` - Makes a pattern with a string.\n" +
                                        "`[>db [ID]` - Generates a Discord bot link to add a bot to a server. *There is no decitated help page for this command.*\n" +
                                        "`[>reddit [ARGUE]` - Gets a random reddit post in any subreddit.\n" +
                                        "`[>number [COM] [INT]` - Gets a fact about a number.\n" +
                                        "`[>pic [COM]` - Commands relating to images and image manipulation.\n" +
                                        "`[>file [COM]` - Commands relating to files and file manipulation.\n",
                                        IsInline = false
                                    }
                                }
                            };
                            break;
                        }
                    case 3:
                        {
                            embedd = new EmbedBuilder
                            {
                                Title = "List of Commands",
                                Color = Discord.Color.Orange,
                                Description = "For extended help on one command, use `[>help [COMM]`.",

                                Footer = new EmbedFooterBuilder
                                {
                                    Text = "xubot :p"
                                },
                                Timestamp = DateTime.UtcNow,
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Actual usable stuff** Page " + page + "/6\n\n" +
                                        "`[>interp [LANG] [CODE]` - Interpets code in differnet programming languages.\n" +
                                        "`[>debug [COM]` - Debugs/tests a feature. *You must be the* ***Bot Owner*** *.*\n" +
                                        "`[>expand-googl [GOO.GL LINK]` - Expands a goo.gl link to show where it redirects to.\n" +
                                        "`[>email-check [EMAIL]` - Runs a check on a email to detect if it's disposable, etc.\n" +
                                        "`[>leetspeak [STRING]` - Converts string to shitty and overdone 1337 speak.\n" +
                                        "`[>moarleetspeak [STRING]` - Converts string to shitty and VERY overdone 1337 speak.\n"+
                                        "`[>gen [INT]` - Generates a random integer with the provided integer as maximum.\n",
                                        IsInline = false
                                    }
                                }
                            };
                            break;
                        }
                    case 4:
                        {
                            embedd = new EmbedBuilder
                            {
                                Title = "List of Commands",
                                Color = Discord.Color.Orange,
                                Description = "For extended help on one command, use `[>help [COMM]`.",

                                Footer = new EmbedFooterBuilder
                                {
                                    Text = "xubot :p"
                                },
                                Timestamp = DateTime.UtcNow,
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Actual usable stuff** Page " + page + "/6\n\n" +
                                        "`[>pet` - Umm... why?\n" +
                                        "`[>hug` - OK..?\n" +
                                        "`[>sex` - why is this here...\n" +
                                        "`[>cuddle` - Do this to be creep\n" +
                                        "`[>poke` - stahppp\n" +
                                        "`[>highfive` - No. Just... no...\n" +
                                        "`[>existental_crisis` - Self explainitory.\n",
                                        IsInline = false
                                    }
                                }
                            };
                            break;
                        }
                    case 5:
                        {
                            embedd = new EmbedBuilder
                            {
                                Title = "List of Commands",
                                Color = Discord.Color.Orange,
                                Description = "For extended help on one command, use `[>help [COMM]`.",

                                Footer = new EmbedFooterBuilder
                                {
                                    Text = "xubot :p"
                                },
                                Timestamp = DateTime.UtcNow,
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Actual usable stuff** Page " + page + "/6\n\n" +
                                        "`[>bake_cake` - It's mine.\n" +
                                        "`[>read_me_a_story` - Ugh...\n" +
                                        "`[>microbrew_some_local_kombucha` - What is this???\n" +
                                        "`[>record_a_mixtape` - ...\n" +
                                        "`[>paint_a_happy_little_tree` - I'm not Bob Ross.\n" +
                                        "`[>base65536 encode [STRING]` - Encodes a string into base 65536.\n" +
                                        "`[>base65536 decode [STRING]` - Decodes an encoding string from base 65536.\n",
                                        IsInline = false
                                    }
                                }
                            };
                            break;
                        }
                    case 6:
                        {
                            embedd = new EmbedBuilder
                            {
                                Title = "List of Commands",
                                Color = Discord.Color.Orange,
                                Description = "For extended help on one command, use `[>help [COMM]`.",

                                Footer = new EmbedFooterBuilder
                                {
                                    Text = "xubot :p"
                                },
                                Timestamp = DateTime.UtcNow,
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Actual usable stuff** Page " + page + "/6\n\n" +
                                        "`[>github [COMM]` - Gets information based on parameters and the command." +
                                        "`[>servertriggers add <JOIN MSG> <NSFW OVERRIDE>` - Adds the current server into the per-server triggers.\n" +
                                        "`[>servertriggers edit [KEY] [NEW VALUE]` - Edits this per-server trigger to the new value.\n" +
                                        "`[>markov` - Generates a sentence from a Markov chain." +
                                        "`[>markov [STRING]` - Gets a sentence and adds it to the Markov chain.\n" +
                                        "`[>markov?i [FILE]` - Adds text from a file into the Markov chain.\n" +
                                        "`[>markov?e` - Exports the Markov chain as a XML document.",
                                        IsInline = false
                                    }
                                }
                            };
                            break;
                        }
                    default:
                        {
                            embedd = new EmbedBuilder
                            {
                                Title = "List of Commands",
                                Color = Discord.Color.Orange,
                                Description = "For extended help on one command, use `[>help [COMM]`.",

                                Footer = new EmbedFooterBuilder
                                {
                                    Text = "xubot :p"
                                },
                                Timestamp = DateTime.UtcNow,
                                Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Actual usable stuff** Page ?/6\n\n" +
                                        "You have entered an invalid help page.",
                                        IsInline = false
                                    }
                                }
                            };
                            break;
                        }
                }

                await ReplyAsync("", false, embedd);
            }
            
            [Command("key")]
            public async Task helpKey()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Help Key",
                    Color = Discord.Color.Orange,
                    Description = "",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Key",
                                Value = "`[>` - Command\n" +
                                "`[INPUT]` - Required input\n" +
                                "`<INPUT>` - Optional input\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("echo")]
            public async Task echo()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Echo",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Echo command help**\n\n" +
                                "`[>echo [STRING]` - Returns the given string.\n" +
                                "`[>echo repeat [STRING] [INT] [STRING]` - Returns the given string, repeated and divided by the second string. *You must have the* ***[Manage Messages]*** *permission.*\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("math")]
            public async Task math()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Math",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Math command help**\n\n" +
                                "`[>math add [FLOAT] [FLOAT]` - Add two floats together.\n" +
                                "`[>math sub [FLOAT] [FLOAT]` - Subtract two floats together.\n" +
                                "`[>math multi [FLOAT] [FLOAT]` - Multiply two floats together.\n" +
                                "`[>math divide [FLOAT] [FLOAT]` - Divide two floats together.\n" +
                                "`[>math mod [FLOAT] [FLOAT]` - Modulos two floats together.\n" +
                                "`[>math pow [DOUBLE] [DOUBLE]` - 1st double ^ 2nd double.\n" +
                                "`[>math sqrt [DOUBLE]` - Add two floats together.\n" +
                                "`[>math sin [DOUBLE]` - Sines a double.\n" +
                                "`[>math asin [DOUBLE]` - Asines a double and returns an angle.\n" +
                                "`[>math sinh [DOUBLE]` - Hyperbolic sines a double.\n" +
                                "`[>math cos [DOUBLE]` - Cosines a double.\n" +
                                "`[>math acos [DOUBLE]` - Acosines a double and returns an angle.\n" +
                                "`[>math cosh [DOUBLE]` - Hyperbolic cosines a double.\n" +
                                "`[>math tan [DOUBLE]` - Tangents a double.\n" +
                                "`[>math atan [DOUBLE]` - Atangents a double and returns an angle.\n" +
                                "`[>math tanh [DOUBLE]` - Hyperbolic tangents a double.\n" +
                                "`[>math quickeval [STRING]` - Evaluates an equation.\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("settings")]
            public async Task set()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Settings",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Setting command help** *(You must be the* ***[Owner of the bot]*** *to execute most of these commands.)*\n\n" +
                                "`[>settings !` - Kills the bot. **(Owner only)**\n" +
                                "`[>settings game [STRING]` - Sets the game. **(Owner only)**\n" +
                                "`[>settings status [STRING]` - Sets the status. **(Owner only)**\n" +
                                "`[>settings stream [STRING]` - Sets the game and the status to streaming. **(Owner only)**\n" +
                                "`[>settings prefix [STRING]` - Sets the prefix for this session. **(Owner only)**\n" +
                                "`[>settings nsfw-commands set [BOOL]` - Sets the NSFW command lock for this session. **(Owner only)**\n" +
                                "`[>settings nsfw-commands` - Gets the NSFW command lock state. \n" +
                                "`[>settings ping` - Gets the latency. \n" +
                                "`[>settings cs` - Gets the connection state. \n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("insult")]
            public async Task insult_help()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Insult",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Insult command help**\n\n" +
                                "`[>insult init` - Clears and initilizes the lists. *You must be the* ***[Owner of the bot]*** *to execute.*\n" +
                                "`[>insult list` - Lists everything on the insult lists that it uses for generation.\n" +
                                "`[>insult add [CHAR] [STRING]` - Adds string to the list called for in the character.\n" +
                                "`[>insult generate` - Makes an insult.",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("convert")]
            public async Task convert_help()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Convert",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Convert command help**\n\n" +
                                "`[>convert temp [DOUBLE] [c2f / f2c]` - Coverts Celsius to Fahrenheit or vise versa.\n" +
                                "`[>convert length [DOUBLE] [ft2m / m2ft]` - Coverts Feet to Meters or vise versa.\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("post")]
            public async Task post_help()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Post",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Post command help**\n\n" +
                                "`[>post reddit [TITLE] [CONTENT]` - Posts to **reddit.com/r/xubot-subreddit/**.\n" +
                                "`[>post twitter [CONTENT]` - Posts to **twitter.com/xubot_bot**. [A] gets replaced with @, and [H] gets replaced with #.\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("patt"), Alias("pattern")]
            public async Task pattern_help()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Pattern",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Pattern command help**\n\n" +
                                "`[>pattern generate [STRING] [STRING]` - Makes a pattern with the two strings.\n" +
                                "`[>pattern generate-preset [STRING] [STRING] [STRING]` - Makes a pattern from the preset found with the searchterm.\n" +
                                "`[>pattern set [STRING] [STRING] [STRING] [STRING] [STRING]` - Sets the pattern with binary strings.",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("reddit"), Alias("reddit")]
            public async Task reddit_help()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Reddit",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Reddit command help**\n\n" +
                                "`[>reddit [PARAMS]`\n\n" +
                                "`[SUBREDDIT]` - *Required* The subreddit to get the post from.\n" +
                                "`<QUERY>` - A search query to look for a post.\n" +
                                "`<SORTING>` - The sorting method to search the subreddit.\n" +
                                "`<HIDE>` - Hides the result if it can be embedded.\n",
                                IsInline = false
                            },
                        new EmbedFieldBuilder
                            {
                                Name = "Commands Pg2",
                                Value = "**Reddit command help**\n\n" +
                                "`[>reddit?r` - Picks a random post from a default subreddit.\n" +
                                "`[>reddit?l` - Picks a random post from the last subreddit, including previous arguments.\n" +
                                "`[>reddit?sub [SUBREDDIT]` - Shows information from the given subreddit.",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("pic")]
            public async Task pictures()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Image/File",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Image command help**\n\n" +
                                "`[>pic ocr [IMAGE ATTACHMENT]` - Attempts to read the image attached to the command.\n" +
                                "`[>pic ocr [LINK]` - Attempts to read the image linked to the command.\n" + 
                                "`[>pic manip [FILTER] [IMAGE ATTACHMENT]` - Play around with images with filters! *[list of filters can only be seen in the source code so far, sorry!]*",
                                IsInline = false
                            }
                            //images
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("interp")]
            public async Task interphelp()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Interp",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**Interp command help**\n\n" +
                                "`[>interp js [CODE]` - Takes code and interprets it as JavaScript.\n" +
                                "`[>interp lua [CODE]` - Takes code and interprets it as Lua. *(IO and OS functions are nullified)*\n" +
                                "`[>interp powershell [CODE]` - Takes code and runs it in Powershell. *(Some cmdlets are blocked)*\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("github")]
            public async Task githibhelp()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: GitHub",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**GitHub command help**\n\n" +
                                "`[>github repo [USER] [REPO]` - Gets information about a GitHub repository.\n" +
                                "`[>github commit [USER] [REPO] [COMMIT]` - Gets information about a GitHub commit on a repo.\n" +
                                "`[>github rlc [USER] [REPO]` - Gets information about the latest GitHub commit for a repo.\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("ssh")]
            public async Task sshhelp()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "List of Commands: SSH",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Commands",
                                Value = "**SSH command help**\n\n" +
                                "`[>ssh connect [HOST] [PORT] [USER] [PASS]` - Connects to a system.\n" +
                                "`[>ssh qc [NICK]` - Connects to a system within the Quick Connect file.\n" +
                                "`[>ssh send [CMD]` - Sends a command to the connected system.\n" +
                                "`[>ssh disconnect [CODE]` - Disconnects the connected system.\n",
                                IsInline = false
                            }
                        }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("markov")]
            public async Task markovhelp()
            {
                Embed embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Markov",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Markov command help**\n\n" +
                                        "`[>markov` - Generates a sentence from a Markov chain." +
                                        "`[>markov [STRING]` - Gets a sentence and adds it to the Markov chain.\n" +
                                        "`[>markov?i [FILE]` - Adds text from a file into the Markov chain.\n" +
                                        "`[>markov?e` - Exports the Markov chain as a XML document.",
                                        IsInline = false
                                    }
                                }
                };

                await ReplyAsync("", false, embedd);
            }

            [Command("eco"), Alias("economy")]
            public async Task ecohelp()
            {
                Embed embedd = new EmbedBuilder
                {
                    Title = "List of Commands: Economy",
                    Color = Discord.Color.Orange,
                    Description = "For extended help on one command, use `[>help [COMM]`.",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                                {
                                    new EmbedFieldBuilder
                                    {
                                        Name = "Commands",
                                        Value = "**Economy command help**\n\n" +
                                        "`[>eco collect` - Generates and gives you currency based on last generation.\n" +
                                        "`[>eco balance` - Gets your economy balance.\n" +
                                        "`[>eco transfer [AMOUNT] [ID]` - Transfers currency to another user.",
                                        IsInline = false
                                    }
                                }
                };

                await ReplyAsync("", false, embedd);
            }


        }
    }
}
