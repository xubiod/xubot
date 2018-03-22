using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace xubot.src
{
    public class Economy : ModuleBase
    {
        public static bool _new_act = false;
        //public static ICommandContext _Context;

        [Command("collect")]
        public async Task collect()
        {
            //await ReplyAsync(DateTime.Now.ToOADate().ToString());
            //_Context = Context;
            try
            {
                EconomyTools.AddOrRefreshAsync(Context.Message.Author);
                //string _lastupd = EconomyTools.ReadLastUpdate(Context.Message.Author);
                double _amount = 5;
                string _new_acct = "(This is a new account!)";
                if (!_new_act)
                {
                    _new_acct = "";
                    DateTime last_upd = DateTime.Parse(EconomyTools.ReadLastUpdate(Context.Message.Author));
                    double hr_since_up = DateTime.Now.ToOADate() - last_upd.ToOADate();

                    _amount = Math.Round(hr_since_up * 10000)/100;
                }
                    _new_act = false;
                EconomyTools.Adjust(Context.Message.Author, _amount);
                double _new = EconomyTools.ReadAmount(Context.Message.Author);

                    _new_act = false;

                    await EconomyTools.Build(Context, _new_acct, "Collected " + _amount + "#.", "Your balance is now " + _new + "#.");
                
            } catch (Exception exp)
            {
                await GeneralTools.CommHandler.BuildError(exp, Context);
            }
        }

        [Command("balance")]
        public async Task balance()
        {
            if (EconomyTools.AccountExists(Context.Message.Author))
            {
                await EconomyTools.Build(Context, "Fake money! Woo!", "Amount in your account", EconomyTools.ReadAmount(Context.Message.Author).ToString() + "#");
            } else
            {
                await EconomyTools.Build(Context, "", "Problem!", "You don't have an account.");
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

            public static void Adjust(IUser arg, double adjust)
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
                        item.lastupdate.Value = DateTime.Now.ToString();
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

                await Context.Channel.SendMessageAsync("", false, embedd);
            }

        }
    }
}
