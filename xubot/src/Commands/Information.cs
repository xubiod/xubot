using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Discord;
using Discord.Commands;
using xubot.Attributes;

namespace xubot.Commands
{
    public class Information : ModuleBase
    {
        private static readonly string[] DiscordColor = { "Blue", "Grey", "Green", "Yellow", "Red" };
        private static readonly string DiscordRegex = "(.+[^#])*(#{1}([0-9]{4})){1}";

        //INFORMATION ABOUT SERVER/CHANNEL/USER
        [Group("info"), Summary("Gets information about various things.")]
        public class Info : ModuleBase
        {
            [Command("server"), Alias("server-info", "si"), Summary("Gets information about the server.")]
            public async Task Serverinfo()
            {
                string verifyLvl = Context.Guild.VerificationLevel.ToString();
                string afkchannelid = Context.Guild.AFKChannelId.ToString();

                if (afkchannelid == "") { afkchannelid = "No AFK Channel"; }

                if (verifyLvl == "None") { verifyLvl = "None ._.\n**Unrestricted**"; }
                else if (verifyLvl == "Low") { verifyLvl = "Low >_>\n**Verified email**"; }
                else if (verifyLvl == "Medium") { verifyLvl = "Medium o_o\n**Verified email**, and **Account age 5min+**"; }
                else if (verifyLvl == "High") { verifyLvl = "(╯°□°）╯︵ ┻━┻ (High)\n**Email**, **5min+ old acct.**, **On server 10min+**"; }
                else if (verifyLvl == "Very High") { verifyLvl = "┻━┻ミヽ(ಠ益ಠ)ﾉ彡 ┻━┻ (Very High)\n**Verified phone**"; }
                else { verifyLvl = "Inconclusive"; }

                IGuildChannel welcomeChannel = await Context.Guild.GetChannelAsync(Context.Guild.SystemChannelId ?? 0);

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Information about: " + Context.Guild.Name, "Server information details", Color.Red);
                embed.ThumbnailUrl = Context.Guild.IconUrl;
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = "Name",
                        Value = $"**{Context.Guild.Name}**\n({Context.Guild.Id})",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Owner",
                        Value = $"{(await Context.Guild.GetUserAsync(Context.Guild.OwnerId)).Username}#{(await Context.Guild.GetUserAsync(Context.Guild.OwnerId)).Discriminator}\n<@{Context.Guild.OwnerId}>",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Verification Level",
                        Value = verifyLvl,
                        IsInline = false
                    },
                    new()
                    {
                        Name = "Created",
                        Value = Context.Guild.CreatedAt,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "AFK",
                        Value = $"ID: {afkchannelid}\nTimeout: {Context.Guild.AFKTimeout} seconds",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Miscellaneous",
                        Value = $"# of Roles: {Context.Guild.Roles.Count}\n# of Emotes: {Context.Guild.Emotes.Count}\nWelcomes go to: <#{welcomeChannel.Id}>\nDefault MSG Notifs.: {Context.Guild.DefaultMessageNotifications}",
                        IsInline = false
                    }
                };

                await ReplyAsync("", false, embed.Build());
            }

            [Command("channel"), Alias("channel-info", "ci"), Summary("Gets information about the current channel")]
            public async Task Channelinfo()
            {
                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Information about: " + Context.Channel.Name, "Channel information details", Color.Red);
                embed.ThumbnailUrl = Context.Guild.IconUrl;
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = "ID",
                        Value = Context.Channel.Id,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Name",
                        Value = Context.Channel.Name,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Created on",
                        Value = Context.Channel.CreatedAt,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Amount of Pinned Messages",
                        Value = $"{(await Context.Channel.GetPinnedMessagesAsync()).Count}/50",
                        IsInline = true
                    },
                    new()
                    {
                        Name = "NSFW?",
                        Value = await Util.IsChannelNsfw(Context),
                        IsInline = true
                    }
                };

                await ReplyAsync("", false, embed.Build());
            }

            [Example("198146693672337409")]
            [Command("user", RunMode = RunMode.Async), Alias("user-info", "ui"), Summary("Gets information about the user that sent the command.")]
            public async Task User(ulong id = ulong.MaxValue)
            {
                try
                {
                    //throw new SpecialException.IHaveNoFuckingIdeaException();

                    IUser user0 = Context.Message.Author;

                    if (id != ulong.MaxValue) { user0 = Program.XuClient.GetUser(id); }

                    if (user0 == null) { await ReplyAsync("You either messed up the ID, or I don't share a server with this person."); return; }

                    string act = user0.Activity == null ? "Nothing." : user0.Activity.Type + " " + user0.Activity.Name + " " + user0.Activity.Details;

                    EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Information about: " + user0, "User information details", Color.Red);
                    embed.ThumbnailUrl = user0.GetAvatarUrl();
                    embed.Fields = new List<EmbedFieldBuilder>()
                    {
                        new()
                        {
                            Name = "ID",
                            Value = user0.Id,
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Status",
                            Value = user0.Status,
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Automation",
                            Value = user0.Id != 198146693672337409 ? user0.IsBot || user0.IsWebhook ? "Yes (" + (user0.IsBot ? "bot" : "") + (user0.IsWebhook ? "webhook" : "") + ")" : "No (Human probably)" : "Indeterminate",
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Current Activity",
                            Value = act,
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Created on",
                            Value = user0.CreatedAt,
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Random Fact(s)",
                            Value = "Default Icon Color: **" + DiscordColor[user0.DiscriminatorValue % 5] + "**",
                            IsInline = true
                        }
                    };

                    if (Context.Guild != null)
                    {
                        IGuildUser user1 = await Context.Guild.GetUserAsync(user0.Id);

                        string roleList = "";

                        foreach (var role in user1.RoleIds) { roleList += Context.Guild.GetRole(role).Mention + " "; }

                        List<EmbedFieldBuilder> guildData = new List<EmbedFieldBuilder>(){
                            new()
                            {
                                Name = "Deafened?",
                                Value = (user1.IsDeafened ? "Yes" : "No") + (user1.IsSelfDeafened ? " (self)" : ""),
                                IsInline = true
                            },
                            new()
                            {
                                Name = "Muted?",
                                Value = (user1.IsMuted ? "Yes" : "No") + (user1.IsSelfMuted ? " (self)" : ""),
                                IsInline = true
                            },
                            new()
                            {
                                Name = "Joined server on",
                                Value = user1.JoinedAt,
                                IsInline = true
                            },
                            new()
                            {
                                Name = "Nickname",
                                Value = user1.Nickname,
                                IsInline = true
                            },
                            new()
                            {
                                Name = "Has " + user1.RoleIds.Count + "roles:",
                                Value = roleList,
                                IsInline = true
                            },
                        };

                        foreach (EmbedFieldBuilder item in guildData) { embed.Fields.Add(item); }

                        guildData.Clear();
                    }

                    await ReplyAsync("", false, embed.Build());
                }
                catch (Exception e)
                {
                    await Util.Error.BuildError(e, Context);
                }
            }

            [Example("xubot#4220")]
            [Command("user", RunMode = RunMode.Async), Alias("user-info", "ui"), Summary("Gets information about the user that sent the command.")]
            public async Task User(params string[] username)
            {
                string complete = username.Length == 1 ? username[0] : "";
                if (username.Length > 1) foreach (string part in username) { complete += part + (username.Last() != part ? " " : ""); }

                if (Regex.Match(complete, DiscordRegex).Success)
                {
                    var _ = Program.XuClient.GetUser(complete.Split("#")[0], complete.Split("#")[1]);

                    if (_ == null)
                    {
                        await ReplyAsync("It appears that's an user that doesn't exist, or I don't share a server with them. Either check, or use their ID.");
                        return;
                    }

                    User(_.Id);
                }
                else
                {
                    await ReplyAsync("I have determined that username isn't correct.");
                }
            }

            [Command("host"), Summary("Gets data about the machine running xubot, and xubot itself.")]
            public async Task HostMachine()
            {
                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Runtime Information", "Details of the bot and OS", new Color(194, 24, 91));
                embed.Fields = new List<EmbedFieldBuilder>()
                {
                    new()
                    {
                        Name = ".NET Installation",
                        Value = RuntimeInformation.FrameworkDescription + "\n" + RuntimeInformation.ProcessArchitecture,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "OS Description",
                        Value = RuntimeInformation.OSDescription + "\n" + RuntimeInformation.OSArchitecture,
                        IsInline = true
                    },
                    new()
                    {
                        Name = "Runtime Environment Version",
                        Value = RuntimeEnvironment.GetSystemVersion(),
                        IsInline = true
                    }
                };

                await ReplyAsync("", false, embed.Build());
            }
        }

        //INFORMATION ABOUT XUBOT
        [Command("about"), Summary("Returns data about the bot.")]
        public async Task About()
        {
            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "About Xubot", $"Version {ThisAssembly.Git.BaseTag}", Color.Orange);
            embed.Fields = new List<EmbedFieldBuilder>()
            {
                new()
                {
                    Name = "APIs",
                    Value = String.Join("", Program.JsonKeys["apis"].Contents.apis),
                    IsInline = false
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Command("credits"), Summary("Returns people that inspired or helped produce this bot.")]
        public async Task Credits()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Xubot Development Credits", $"Version {ThisAssembly.Git.BaseTag}", Color.Orange);
            embed.Fields = new List<EmbedFieldBuilder>()
            {
                new()
                {
                    Name = "Credits",
                    Value = String.Join("", Program.JsonKeys["apis"].Contents.credits),
                    IsInline = false
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Command("version"), Summary("Returns the current build via the latest commit.")]
        public async Task VersionCmd()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Xubot Version", "The specifics.", Color.Orange);
            embed.Fields = new List<EmbedFieldBuilder>()
            {
                new()
                {
                    Name = "Version",
                    Value = $"**{ThisAssembly.Git.BaseTag}**\n{ThisAssembly.Git.Tag}",
                    IsInline = true
                },
                new()
                {
                    Name = "Build Commit",
                    Value = $"On {ThisAssembly.Git.Branch}:\n{ThisAssembly.Git.Sha}\n\nCommited at {ThisAssembly.Git.CommitDate}",
                    IsInline = false
                },
                new()
                {
                    Name = "Remote Repository",
                    Value = $"{ThisAssembly.Git.RepositoryUrl}",
                    IsInline = true
                }
            };

            await ReplyAsync("", false, embed.Build());
        }

        [Command("donate"), Summary("Returns a link to donate to the developer.")]
        public async Task Donate()
        {
            await ReplyAsync("To donate to the creator of this bot, please visit:\n" + Program.JsonKeys["keys"].Contents.donate_link);
        }

        [Command("privacy-policy")]
        public async Task Pp()
        {
            File.WriteAllText(Path.GetTempPath() + "pripol.txt", Properties.Resources.PrivacyPolicy);
            await Context.Channel.SendFileAsync(Path.GetTempPath() + "pripol.txt");
        }
    }
}
