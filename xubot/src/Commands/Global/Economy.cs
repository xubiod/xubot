using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using xubot.src.Attributes;
using xubot.src.Commands.Global;

namespace xubot.src.Commands.Global
{
    [Group("economy"), Alias("eco"), Summary("Stuff relating to the economics system."), Deprecated]
    public class Economy : ModuleBase
    {
        public static bool _new_act = false;

        //confirm variables for transfer
        public static IUser _auth;

        public static IUser _transferTo;
        public static double _amount = 0;
        public static string _pass = "";
        public Random _r = Util.Globals.GlobalRandom;
        //public static ICommandContext _Context;

        [Command("collect"), Summary("Collects currency based on the amount of hours since last collection.")]
        public async Task Collect()
        {
            await Util.Error.Deprecated(Context);
            try
            {
                EconomyTools.AddOrRefreshAsync(Context.Message.Author);
                //string _lastupd = EconomyTools.ReadLastUpdate(Context.Message.Author);
                double _amount = 5.00;
                string _new_acct = "(This is a new account!)";
                if (!_new_act)
                {
                    _new_acct = "";
                    DateTime last_upd = DateTime.Parse(EconomyTools.ReadLastUpdate(Context.Message.Author));
                    double hr_since_up = DateTime.Now.ToOADate() - last_upd.ToOADate();

                    _amount = System.Math.Round(hr_since_up * 10000);
                }
                _new_act = false;
                EconomyTools.Adjust(Context.Message.Author, _amount);
                double _new = EconomyTools.ReadAmount(Context.Message.Author);

                _new_act = false;

                await EconomyTools.Build(Context, _new_acct, "Collected " + _amount + "#.", "Your balance is now " + _new + "#.");
            }
            catch (Exception exp)
            {
                await Util.Error.BuildError(exp, Context);
            }
        }

        [Command("balance"), Summary("Returns your balance.")]
        public async Task Balance()
        {
            await Util.Error.Deprecated(Context);
            if (EconomyTools.AccountExists(Context.Message.Author))
            {
                await EconomyTools.Build(Context, "Fake money! Woo!", "Amount in your account", EconomyTools.ReadAmount(Context.Message.Author).ToString() + "#");
            }
            else
            {
                await EconomyTools.Build(Context, "", "Problem!", "You don't have an account.");
            }
        }

        [Command("transfer", RunMode = RunMode.Async), Summary("Initializes a transfer to someone. Only one transfer is allowed at any given time.")]
        public async Task Transfer(double amount, ulong id)
        {
            await Util.Error.Deprecated(Context);

            //IUser transferTo = await Context.Guild.GetUserAsync(id);
            IUser transferTo = Program.xuClient.GetUser(id);

            if (EconomyTools.AccountExists(transferTo))
            {
                if (EconomyTools.ReadAmount(Context.Message.Author) > (amount * 1.1) && amount > 1)
                {
                    _auth = Context.Message.Author;
                    _transferTo = transferTo;
                    _amount = amount;

                    string code = _r.Next(9).ToString() + _r.Next(9).ToString() + _r.Next(9).ToString();
                    _pass = code;

                    await EconomyTools.Build_Transfer(Context, _amount, _transferTo, code);
                }
                else if (amount < 0.01)
                {
                    await ReplyAsync("You can't transfer negative or zero amounts.");
                }
                else
                {
                    await ReplyAsync("You can't transfer what you don't have!");
                }
            }
            else
            {
                await EconomyTools.Build(Context, "", "Problem!", "You are trying to transfer money to someone that doesn't have an account.");
            }
        }

        [Command("confirm"), Summary("Confirms a transfer to someone. Can only be done by the starter of the transfer, and incorrect codes cancel the transfer.")]
        public async Task ConfirmTransfer(string pass)
        {
            await Util.Error.Deprecated(Context);

            if (_pass == pass && _pass != "")
            {
                if (_auth == Context.Message.Author)
                {
                    EconomyTools.Adjust(_auth, System.Math.Round((_amount * -1.1) * 100) / 100, false);
                    EconomyTools.Adjust(_transferTo, _amount, false);
                    await ReplyAsync("Transfer of `" + _amount + "#` has been completed to " + _transferTo.Username + "#" + _transferTo.Discriminator + ".");
                }
                else
                {
                    await ReplyAsync("Confirming transactions can only be done by the author of the transfer.");
                }
            }
            else if (_pass == "")
            {
                await ReplyAsync("Transfer was not initialized.");
            }
            else
            {
                await ReplyAsync("Incorrect code. Transfer cancelled.");
            }
        }

        public class EconomyTools : ModuleBase
        {
            public static void AddOrRefreshAsync(IUser arg)
            {
                bool exists = false;

                Mood.xdoc = XDocument.Load("Economy.xml");

                var items = from i in Mood.xdoc.Descendants("reserve")
                            select new
                            {
                                user = i.Attribute("user")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    _new_act = true;
                    Console.WriteLine("new user found to add to economy, doing that now");

                    XElement xelm = new XElement("reserve");
                    XAttribute user = new XAttribute("user", arg.Id.ToString());
                    XAttribute moodval = new XAttribute("amount", "0");
                    XAttribute lastupdate = new XAttribute("lastupdate", DateTime.Now.ToString());

                    xelm.Add(user);
                    xelm.Add(moodval);
                    xelm.Add(lastupdate);

                    Mood.xdoc.Root.Add(xelm);
                    Mood.xdoc.Save("Economy.xml");
                }
            }

            public static bool AccountExists(IUser arg)
            {
                bool exists = false;

                Mood.xdoc = XDocument.Load("Economy.xml");

                var items = from i in Mood.xdoc.Descendants("reserve")
                            select new
                            {
                                user = i.Attribute("user")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        exists = true;
                    }
                }

                return exists;
            }

            public static double ReadAmount(IUser arg)
            {
                Mood.xdoc = XDocument.Load("Economy.xml");

                var items = from i in Mood.xdoc.Descendants("reserve")
                            select new
                            {
                                user = i.Attribute("user"),
                                amount = i.Attribute("amount")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        return Convert.ToDouble(item.amount.Value);
                    }
                }

                return 0;
            }

            public static string ReadLastUpdate(IUser arg)
            {
                Mood.xdoc = XDocument.Load("Economy.xml");

                var items = from i in Mood.xdoc.Descendants("reserve")
                            select new
                            {
                                user = i.Attribute("user"),
                                lastupdate = i.Attribute("lastupdate")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        return item.lastupdate.Value;
                    }
                }

                return "";
            }

            public static void Adjust(IUser arg, double adjust, bool changeUpdate = true)
            {
                Mood.xdoc = XDocument.Load("Economy.xml");

                var items = from i in Mood.xdoc.Descendants("reserve")
                            select new
                            {
                                user = i.Attribute("user"),
                                amount = i.Attribute("amount"),
                                lastupdate = i.Attribute("lastupdate")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        item.amount.Value = (Convert.ToDouble(item.amount.Value) + adjust).ToString();
                        if (changeUpdate) item.lastupdate.Value = DateTime.Now.ToString();
                    }
                }

                Mood.xdoc.Save("Economy.xml");
            }

            public static async Task Build(ICommandContext Context, string description, string title, string amt)
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Economy",
                    Color = Discord.Color.Green,
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
                                Name = title,
                                Value = amt,
                                IsInline = false
                            }
                        }
                };

                await Context.Channel.SendMessageAsync("", false, embedd.Build());
            }

            public static async Task Build_Transfer(ICommandContext Context, double amount, IUser transferTo, string _code)
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Economy",
                    Color = Discord.Color.Green,
                    Description = "Transfer: Complete Cost is `" + (System.Math.Round((_amount * 1.1) * 100) / 100) + "#`",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Transfer Amount to **" + transferTo.Username + "#" + transferTo.Discriminator + "**",
                                Value = "```" + amount + "#```",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Transfer Fee",
                                Value = "```" + (System.Math.Round((_amount * .1) * 100)/100).ToString() + "#```",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Transfer Confirmation Code",
                                Value = "Your confirmation is `" + _code + "`.\nUse `[>eco confirm` to confirm this transaction.",
                                IsInline = false
                            }
                        }
                };

                await Context.Channel.SendMessageAsync("", false, embedd.Build());
            }
        }
    }
}