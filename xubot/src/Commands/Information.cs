using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xubot.src.Commands
{
    public class Information : ModuleBase
    {
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

                if (verifyLvl == "None") { verifyLvl = "None ._."; }
                else if (verifyLvl == "Low") { verifyLvl = "Low >_>"; }
                else if (verifyLvl == "Medium") { verifyLvl = "Medium o_o"; }
                else if (verifyLvl == "High") { verifyLvl = "(╯°□°）╯︵ ┻━┻ (High)"; }
                else if (verifyLvl == "Very High") { verifyLvl = "┻━┻ミヽ(ಠ益ಠ)ﾉ彡 ┻━┻ (Very High)"; }
                else { verifyLvl = "Unconclusive"; }

                IGuildChannel welcomeChannel = await Context.Guild.GetChannelAsync(Context.Guild.SystemChannelId ?? 0);

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Information about: " + Context.Guild.Name,
                    Color = Discord.Color.Red,
                    Description = "Server information details",
                    ThumbnailUrl = Context.Guild.IconUrl,

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "ID",
                                Value = Context.Guild.Id,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Name",
                                Value = Context.Guild.Name,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Owner",
                                Value = (await Context.Guild.GetUserAsync(Context.Guild.OwnerId)).Username + "#" + (await Context.Guild.GetUserAsync(Context.Guild.OwnerId)).Discriminator,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Verification Level",
                                Value = verifyLvl,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "AFK Timeout",
                                Value = Context.Guild.AFKTimeout.ToString() + " seconds",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "AFK Channel ID",
                                Value = afkchannelid,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Welcomes go to this channel:",
                                Value = "<#" + welcomeChannel.Id + ">",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Created",
                                Value = Context.Guild.CreatedAt,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Default MSG Notifications",
                                Value = Context.Guild.DefaultMessageNotifications,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Amount of Roles",
                                Value = Context.Guild.Roles.Count,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Amount of Custom Emoji",
                                Value = Context.Guild.Emotes.Count,
                                IsInline = true
                            }
                        }
                };
                await ReplyAsync("", false, embedd.Build());
            }

            [Command("channel"), Alias("channel-info", "ci"), Summary("Gets information about the current channel")]
            public async Task Channelinfo()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Information about: " + Context.Channel.Name,
                    Color = Discord.Color.Red,
                    Description = "Channel information details",
                    ThumbnailUrl = Context.Guild.IconUrl,

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p",
                        IconUrl = Program.xuClient.CurrentUser.GetAvatarUrl()
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "ID",
                                Value = Context.Channel.Id,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Name",
                                Value = Context.Channel.Name,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Created on",
                                Value = Context.Channel.CreatedAt,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Amount of Pinned Messages",
                                Value = (await Context.Channel.GetPinnedMessagesAsync()).Count.ToString() + "/50",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "NSFW?",
                                Value = (await Util.IsChannelNSFW(Context)),
                                IsInline = true
                            }
                        }
                };
                await ReplyAsync("", false, embedd.Build());
            }

            [Command("user", RunMode = RunMode.Async), Alias("user-info", "ui"), Summary("Gets information about the user that sent the command.")]
            public async Task User(ulong id = 0)
            {
                try
                {
                    //throw new SpecialException.IHaveNoFuckingIdeaException();

                    Discord.IUser _user0 = Context.Message.Author;
                    IGuildUser _user1 = await Context.Guild.GetUserAsync(_user0.Id);

                    if (id == 0)
                    {
                        _user0 = Context.Message.Author;
                        _user1 = await Context.Guild.GetUserAsync(_user0.Id);
                    }
                    else
                    {
                        _user0 = Program.xuClient.GetUser(id);
                        _user1 = await Context.Guild.GetUserAsync(_user0.Id);
                    }

                    string _role_list = "";

                    foreach (var role in _user1.RoleIds)
                    {
                        var _role = Context.Guild.GetRole(role);

                        _role_list += _role.Mention + " ";
                    }

                    string act = "";

                    if (_user0.Activity == null)
                    {
                        act = "Nothing.";
                    }
                    else
                    {
                        act = _user0.Activity.Type + " " + _user0.Activity.Name;
                    }

                    EmbedBuilder embedd = new EmbedBuilder
                    {
                        Title = "Information about: " + _user0,
                        Color = Discord.Color.Red,
                        Description = "User information details",
                        ThumbnailUrl = _user0.GetAvatarUrl(),

                        Footer = new EmbedFooterBuilder
                        {
                            Text = "xubot :p"
                        },
                        Timestamp = DateTime.UtcNow,
                        Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "ID",
                                Value = _user0.Id,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Status",
                                Value = _user0.Status,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Bot?",
                                Value = _user0.IsBot,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Webhook?",
                                Value = _user0.IsWebhook,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Current Activity",
                                Value = act,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Deafened?",
                                Value = _user1.IsDeafened,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Self Deafened?",
                                Value = _user1.IsSelfDeafened,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Muted?",
                                Value = _user1.IsMuted,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Self Muted?",
                                Value = _user1.IsSelfMuted,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Joined server on",
                                Value = _user1.JoinedAt,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Nickname",
                                Value = _user1.Nickname,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Has " + (_user1.RoleIds.Count) + "roles:",
                                Value = _role_list,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Created on",
                                Value = _user0.CreatedAt,
                                IsInline = true
                            }
                        }
                    };
                    await ReplyAsync("", false, embedd.Build());
                }
                catch (Exception e)
                {
                    await Util.Error.BuildError(e, Context);
                }
            }

            [Command("host"), Summary("Gets data about the machine running xubot, and xubot itself.")]
            public async Task HostMachine()
            {
                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Runtime Information",
                    Color = new Color(194, 24, 91),
                    Description = "Details of the bot and OS",
                    ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                    {
                         new EmbedFieldBuilder
                         {
                             Name = ".NET Installation",
                             Value = RuntimeInformation.FrameworkDescription + "\n" + RuntimeInformation.ProcessArchitecture.ToString(),
                             IsInline = true
                         },
                         new EmbedFieldBuilder
                         {
                             Name = "OS Description",
                             Value = RuntimeInformation.OSDescription + "\n" + RuntimeInformation.OSArchitecture.ToString(),
                             IsInline = true
                         },
                         new EmbedFieldBuilder
                         {
                             Name = "Runtime Environment Version",
                             Value = RuntimeEnvironment.GetSystemVersion(),
                             IsInline = true
                         }
                    }
                };

                await ReplyAsync("", false, embedd.Build());
            }
        }

        //INFORMATION ABOUT XUBOT
        [Command("about"), Summary("Returns data about the bot.")]
        public async Task About()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "About Xubot",
                Color = Discord.Color.Orange,
                Description = "Version " + ThisAssembly.Git.BaseTag,
                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "APIs",
                                Value = String.Join("", Program.JSONKeys["apis"].Contents.apis),
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Command("credits"), Summary("Returns people that inspired or helped produce this bot.")]
        public async Task Credits()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Xubot Development Credits",
                Color = Discord.Color.Orange,
                Description = "Version " + ThisAssembly.Git.BaseTag,

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Credits",
                                Value = String.Join("", Program.JSONKeys["apis"].Contents.credits),
                                IsInline = false
                            }
                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Command("version"), Summary("Returns the current build via the latest commit.")]
        public async Task VersionCMD()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Xubot Version",
                Color = Discord.Color.Orange,
                Description = "The specifics.",

                Footer = new EmbedFooterBuilder
                {
                    Text = "xubot :p"
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Version",
                                Value = ThisAssembly.Git.BaseTag,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Specific Build",
                                Value = ThisAssembly.Git.Tag,
                                IsInline = true
                            },
                            //https://github.com/xubot-team/xubot/commit/2064085bc0fd33a591036f67b686d0366d1591c5
                            new EmbedFieldBuilder
                            {
                                Name = "Build Commit",
                                Value = ThisAssembly.Git.Commit,
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Link to Latest Change",
                                Value = "https://github.com/xubot-team/xubot/commit/" + ThisAssembly.Git.Sha,
                                IsInline = true
                            }
                        }
            };

            await ReplyAsync("", false, embedd.Build());
        }

        [Command("donate"), Summary("Returns a link to donate to the developer.")]
        public async Task Donate()
        {
            await ReplyAsync("To donate to the creator of this bot, please visit:\n" + Program.JSONKeys["keys"].Contents.donate_link);
        }

        [Command("privacy-policy")]
        public async Task PP()
        {
            File.WriteAllText(Path.GetTempPath() + "pripol.txt", Properties.Resources.PrivacyPolicy);
            await Context.Channel.SendFileAsync(Path.GetTempPath() + "pripol.txt");
        }
    }
}
