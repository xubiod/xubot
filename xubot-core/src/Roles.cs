using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing;

using DColor = Discord.Color;
using SColor = System.Drawing.Color;
using xubot_core.src.Attributes;

namespace xubot_core.src
{
    [Group("pronoun"), Summary("Stuff relating to the working albeit kinda defunct pronoun system."), Attributes.Deprecated]
    public class Roles : ModuleBase
    {
        public static XDocument xdoc = new XDocument();

        [Command("add"), RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Add(string pro)
        {
            await Util.Error.Deprecated(Context);

            RoleTools.Pronoun.AddRefresh(Context.Message.Author);
            RoleTools.Pronoun.Set(Context.Message.Author, pro);

            string role_name = "prefers " + pro;
            IRole role;

            if (Context.Guild.Roles.Any(x => x.Name == role_name))
            {
                role = Context.Guild.Roles.First(x => x.Name == role_name);
            }
            else
            {
                role = await Context.Guild.CreateRoleAsync(role_name);
            }

            /*foreach (var _R in (Context.Message.Author as IGuildUser).RoleIds)
            {
                IRole _role = Context.Guild.GetRole(_R);

                if (_role.Name.Contains("prefers"))
                {
                    await (Context.Message.Author as IGuildUser).RemoveRoleAsync(_role);
                }
            }*/

            await (Context.Message.Author as IGuildUser).AddRoleAsync(role);
            await ReplyAsync("Pronoun set, and role made or set.");
        }

        [Command("remove"), RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Remove(string pro)
        {
            await Util.Error.Deprecated(Context);

            RoleTools.Pronoun.AddRefresh(Context.Message.Author);
            RoleTools.Pronoun.Set(Context.Message.Author, pro);

            string role_name = "prefers " + pro;

            foreach (var _R in (Context.Message.Author as IGuildUser).RoleIds)
            {
                IRole _role = Context.Guild.GetRole(_R);

                if (_role.Name.Contains(role_name))
                {
                    await (Context.Message.Author as IGuildUser).RemoveRoleAsync(_role);
                }
            }

            await ReplyAsync("Pronoun removed.");
        }

        [Command("replace"), RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Replace(string pro)
        {
            await Util.Error.Deprecated(Context);

            RoleTools.Pronoun.AddRefresh(Context.Message.Author);
            RoleTools.Pronoun.Set(Context.Message.Author, pro);

            string role_name = "prefers " + pro;
            IRole role;

            if (Context.Guild.Roles.Any(x => x.Name == role_name))
            {
                role = Context.Guild.Roles.First(x => x.Name == role_name);
            }
            else
            {
                role = await Context.Guild.CreateRoleAsync(role_name);
            }

            foreach (var _R in (Context.Message.Author as IGuildUser).RoleIds)
            {
                IRole _role = Context.Guild.GetRole(_R);

                if (_role.Name.Contains("prefers"))
                {
                    await (Context.Message.Author as IGuildUser).RemoveRoleAsync(_role);
                }
            }

            await (Context.Message.Author as IGuildUser).AddRoleAsync(role);
            await ReplyAsync("Pronoun replaced (all pronoun roles have been removed).");
        }
    }

    [Group("identity"), Summary("Stuff relating to the working albeit kinda defunct identification system."), Attributes.Deprecated]
    public class Identity : ModuleBase
    {
        public static XDocument xdoc = new XDocument();

        [Command("set"), RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Replace(string first, string second, string third)
        {
            await Util.Error.Deprecated(Context);

            //RoleTools.Identity.AddRefresh(Context.Message.Author);

            //RoleTools.Identity.Set(Context.Message.Author, first + second + third);

            string role_name = "identifies as " + RoleTools.Identity.Simplify(first) + " " + second + " " + third;
            IRole role;

            if (Context.Guild.Roles.Any(x => x.Name == role_name))
            {
                role = Context.Guild.Roles.First(x => x.Name == role_name);
            }
            else
            {
                role = await Context.Guild.CreateRoleAsync(role_name);
            }

            foreach (var _R in (Context.Message.Author as IGuildUser).RoleIds)
            {
                IRole _role = Context.Guild.GetRole(_R);

                if (_role.Name.Contains("identifies as "))
                {
                    await (Context.Message.Author as IGuildUser).RemoveRoleAsync(_role);
                }
            }

            await (Context.Message.Author as IGuildUser).AddRoleAsync(role);
            await ReplyAsync("Identity added/replaced.");
        }

        [Command("remove"), RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task Remove()
        {
            await Util.Error.Deprecated(Context);

            foreach (var _R in (Context.Message.Author as IGuildUser).RoleIds)
            {
                IRole _role = Context.Guild.GetRole(_R);

                if (_role.Name.Contains("identifies as "))
                {
                    await (Context.Message.Author as IGuildUser).RemoveRoleAsync(_role);
                }
            }

            //await (Context.Message.Author as IGuildUser).AddRoleAsync(role);
            await ReplyAsync("Identity removed.");
        }
    }

    public class RoleTools
    {
        public class Pronoun
        {
            public static void AddRefresh(IUser arg)
            {
                bool exists = false;

                Roles.xdoc = XDocument.Load("Pronouns.xml");

                var items = from i in Roles.xdoc.Descendants("pronoun")
                            select new
                            {
                                user = i.Attribute("id"),
                                preferred = i.Attribute("preferred")
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
                    Console.WriteLine("new user found to add to pronouns, doing that now");

                    XElement xelm = new XElement("pronoun");
                    XAttribute user = new XAttribute("id", arg.Id.ToString());
                    XAttribute prefer = new XAttribute("preferred", "not set!");

                    xelm.Add(user);
                    xelm.Add(prefer);

                    Roles.xdoc.Root.Add(xelm);
                    Roles.xdoc.Save("Pronouns.xml");
                }
            }
            public static string Read(IUser arg)
            {
                Roles.xdoc = XDocument.Load("Pronouns.xml");

                var items = from i in Roles.xdoc.Descendants("pronoun")
                            select new
                            {
                                user = i.Attribute("id"),
                                preferred = i.Attribute("preferred")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        return item.preferred.Value;
                    }
                }

                return "not set!";
            }
            public static void Set(IUser arg, string newVal)
            {
                Roles.xdoc = XDocument.Load("Pronouns.xml");

                var items = from i in Roles.xdoc.Descendants("pronoun")
                            select new
                            {
                                user = i.Attribute("id"),
                                preferred = i.Attribute("preferred")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        item.preferred.Value = newVal;
                    }
                }

                Roles.xdoc.Save("Pronouns.xml");
            }
            public static async Task Build(ICommandContext Context, IUser arg, string prefer)
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Preferred Pronoun",
                    Color = Discord.Color.Red,
                    Description = arg.Username + "#" + arg.Discriminator + " 's Preferred Pronoun",

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Please use:",
                                Value = "**" + prefer + "**",
                                IsInline = false
                            }
                        }
                };

                await Context.Channel.SendMessageAsync("", false, embedd.Build());
            }
        }

        public class Identity
        {
            public static void AddRefresh(IUser arg)
            {
                bool exists = false;

                Roles.xdoc = XDocument.Load("Identity.xml");

                var items = from i in Roles.xdoc.Descendants("identity")
                            select new
                            {
                                user = i.Attribute("id"),
                                preferred = i.Attribute("identify")
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
                    Console.WriteLine("new user found to add to identity, doing that now");

                    XElement xelm = new XElement("identity");
                    XAttribute user = new XAttribute("id", arg.Id.ToString());
                    XAttribute prefer = new XAttribute("identify", "not set!");

                    xelm.Add(user);
                    xelm.Add(prefer);

                    Roles.xdoc.Root.Add(xelm);
                    Roles.xdoc.Save("Identity.xml");
                }
            }
            public static string Read(IUser arg)
            {
                Roles.xdoc = XDocument.Load("Identity.xml");

                var items = from i in Roles.xdoc.Descendants("identity")
                            select new
                            {
                                user = i.Attribute("id"),
                                preferred = i.Attribute("identify")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        return item.preferred.Value;
                    }
                }

                return "not set!";
            }
            public static void Set(IUser arg, string newVal)
            {
                Roles.xdoc = XDocument.Load("Identity.xml");

                var items = from i in Roles.xdoc.Descendants("identity")
                            select new
                            {
                                user = i.Attribute("id"),
                                preferred = i.Attribute("preferred")
                            };

                foreach (var item in items)
                {
                    if (item.user.Value == arg.Id.ToString())
                    {
                        item.preferred.Value = newVal;
                    }
                }

                Roles.xdoc.Save("Identity.xml");
            }
            public static string Simplify(string input)
            {
                if (input.Contains("cis")) { return "cis"; }
                else if (input.Contains("trans")) { return "trans"; }
                else { return input; }
            }
        }
    }
}
