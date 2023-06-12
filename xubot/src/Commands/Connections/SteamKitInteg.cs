using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SteamKit2;
using xubot.Attributes;
// ReSharper disable StringLiteralTypo

namespace xubot.Commands.Connections
{
    [Group("steam"), Summary("Steam API integration via SteamKit2.")]
    public class SteamKitInteg : ModuleBase
    {
        private static readonly dynamic SteamUserInterface = WebAPI.GetInterface("ISteamUser", Program.JsonKeys["keys"].Contents.steam.ToString());
        private static readonly dynamic PlayerServiceInterface = WebAPI.GetInterface("IPlayerService", Program.JsonKeys["keys"].Contents.steam.ToString());
        private static readonly dynamic SteamAppsInterface = WebAPI.GetInterface("ISteamApps", Program.JsonKeys["keys"].Contents.steam.ToString());
        private static readonly dynamic SteamNewsInterface = WebAPI.GetInterface("ISteamNews", Program.JsonKeys["keys"].Contents.steam.ToString());

        [Example("76561197960287930")]
        [Command("user", RunMode = RunMode.Async), Summary("Gets information about a Steam user based on their ID.")]
        public async Task User(ulong id)
        {
            using Util.WorkingBlock wb = new Util.WorkingBlock(Context);
            try
            {
                KeyValue ownedGames = PlayerServiceInterface.GetOwnedGames(steamid: id, include_appinfo: 1);
                KeyValue playerSummaries = SteamUserInterface.GetPlayerSummaries002(steamids: id);
                KeyValue playerLevel = PlayerServiceInterface.GetSteamLevel(steamid: id);

                playerSummaries = playerSummaries["players"].Children[0];

                decimal twoWeeks = 0;
                decimal forever = 0;

                string mostTimeIn = "";
                decimal mostTime = 0;

                string mostWeekIn = "";
                decimal mostWeek = 0;

                EmbedFieldBuilder mostWeekField = new() { Name = "Most Playtime (2 wks)", Value = "Has not played in last 2 weeks.", IsInline = true };
                EmbedFieldBuilder mostTimeField = new() { Name = "Most Playtime (forever)", Value = "Has not played since account creation (Wha...?)", IsInline = true };

                foreach (KeyValue game in ownedGames["games"].Children)
                {
                    forever += game["playtime_forever"].AsInteger();
                    twoWeeks += game["playtime_2weeks"].AsInteger();

                    if (game["playtime_forever"].AsInteger() > mostTime)
                    {
                        mostTime = game["playtime_forever"].AsInteger();
                        mostTimeIn = game["name"].AsString();
                    }

                    if (game["playtime_2weeks"].AsInteger() > mostWeek)
                    {
                        mostWeek = game["playtime_2weeks"].AsInteger();
                        mostWeekIn = game["name"].AsString();
                    }
                }

                if (!string.IsNullOrEmpty(mostWeekIn))
                {
                    mostWeekField.Value = $"In App\n**__{mostWeekIn}__**: {mostWeek:#,###} minutes\n{mostWeek / 60:#,###0.0} hours";
                }

                if (!string.IsNullOrEmpty(mostTimeIn))
                {
                    mostTimeField.Value = $"In App\n**__{mostTimeIn}__**: {mostTime:#,###} minutes\n{mostTime / 60:#,###0.0} hours";
                }

                ulong lastLogOffUnsigned = playerSummaries["lastlogoff"].AsUnsignedLong();
                DateTime lastLogOff = Util.UnixTimeStampToDateTime(lastLogOffUnsigned);

                ulong timeCreatedUnsigned = playerSummaries["timecreated"].AsUnsignedLong();
                DateTime timeCreated = Util.UnixTimeStampToDateTime(timeCreatedUnsigned);

                TimeSpan lastLogOffToNow = DateTime.Now - lastLogOff;
                TimeSpan createdToNow = DateTime.Now - timeCreated;

                string playing = "";
                if (playerSummaries["gameid"].AsInteger() != 0) playing = $"Currently playing **{ReturnAppName(playerSummaries["gameid"].AsInteger())}**";

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Steam User: {playerSummaries["personaname"].AsString()} ({id.ToString()})", "Data obtained Steam WebAPI using SteamKit2", Color.DarkBlue);
                embed.ThumbnailUrl = playerSummaries["avatarfull"].AsString();

                if (playerSummaries["communityvisibilitystate"].AsInteger(1) == 3 /* public, don't ask why */)
                {
                    embed.Fields = new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                            Name = "Current Stats",
                            Value = $"Currently __{GetStatus(playerSummaries["personastate"].AsInteger())}__\n" +
                                    $"Level **{playerLevel["player_level"].AsString()}**\n**" +
                                    $"{ownedGames["game_count"].AsString()}** products\n**" +
                                    $"{GetFriendSlots(playerLevel["player_level"].AsInteger())}** friend slots" +
                                    $"{playing}",
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Playtime (2 wks)",
                            Value = $"{twoWeeks:#,##0} minutes\n{twoWeeks / 60:#,###0.0} hours",
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Playtime (forever)",
                            Value = $"{forever:#,##0} minutes\n{forever / 60:#,###0.0} hours\n{forever / 1440:#,###0.00} days",
                            IsInline = true
                        },
                        mostWeekField,
                        mostTimeField,
                        new()
                        {
                            Name = "Last Logoff",
                            Value = $"{lastLogOff.ToShortDateString()} {lastLogOff.ToShortTimeString()}\n(" +
                                    $"{System.Math.Round(lastLogOffToNow.TotalHours*100/100).ToString(CultureInfo.CurrentCulture)} hours)",
                            IsInline = true
                        },
                        new()
                        {
                            Name = "Time Created",
                            Value = $"{timeCreated.ToShortDateString()} {timeCreated.ToShortTimeString()}\n(" +
                                    $"{createdToNow.TotalDays / 365:#,###.00} years)",
                            IsInline = true
                        }
                    };
                }
                else
                {
                    embed.Fields = new List<EmbedFieldBuilder>
                    {
                        new()
                        {
                            Name = "Current Stats",
                            Value = $"**This user's profile is private.**\nCurrently __{GetStatus(playerSummaries["personastate"].AsInteger())}__",
                            IsInline = false
                        },
                        new()
                        {
                            Name = "Last Logoff",
                            Value = $"{lastLogOff.ToShortDateString()} {lastLogOff.ToShortTimeString()}\n(" +
                                    $"{System.Math.Round(lastLogOffToNow.TotalHours*100/100).ToString(CultureInfo.CurrentCulture)} hours)",
                            IsInline = false
                        }
                    };
                }

                await ReplyAsync("", false, embed.Build());
            }
            catch (Exception ex)
            {
                await Util.Error.BuildErrorAsync(ex, Context);
            }
        }

        [Example("gabelogannewell")]
        [Command("user", RunMode = RunMode.Async), Summary("Gets information about a Steam user based on their vanity URL.")]
        public async Task User(string vanity)
        {
            KeyValue vanityUrl = SteamUserInterface.ResolveVanityURL(vanityurl: vanity);

            await User(vanityUrl["steamid"].AsUnsignedLong());
        }

        [Example("4000")]
        [Command("news", RunMode = RunMode.Async), Summary("Gets news for a game via it's ID.")]
        public async Task News(int appid, int cap = 5)
        {
            using Util.WorkingBlock wb = new Util.WorkingBlock(Context);
            try
            {
                KeyValue news = SteamNewsInterface.GetNewsForApp0002(appid: appid);

                news = news["newsitems"];
                int amount = System.Math.Min(System.Math.Min(news.Children.Count, cap), 5);

                List<EmbedFieldBuilder> articleDetails = new List<EmbedFieldBuilder>();

                for (int i = 0; i < amount; i++)
                {
                    Uri articleUri = new(news.Children[i]["url"].AsString() ?? string.Empty);

                    string label = "";
                    if (news.Children[i]["feedlabel"].AsString() != "Community Announcements") label = "\n*(" + news.Children[i]["feedlabel"].AsString() + ")*";

                    articleDetails.Add(new EmbedFieldBuilder
                    {
                        Name = news.Children[i]["title"].AsString() + label,
                        Value = "[Go to article (" + articleUri.Host + ")](" +
                                news.Children[i]["url"].AsString() + ")"
                    });
                }

                EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Latest {amount} news articles for the app: {ReturnAppName(appid)}", "Data obtained Steam WebAPI using SteamKit2", Color.DarkBlue);
                embed.Fields = articleDetails;

                await ReplyAsync("", false, embed.Build());
            }
            catch (Exception exp)
            {
                await Util.Error.BuildErrorAsync(exp, Context);
            }
        }

        [Example("Garry's Mod 3")]
        [Command("news", RunMode = RunMode.Async), Summary("Gets news for a game via it's name on Steam. (HAS TO BE EXACT)")]
        public async Task News(params string[] args)
        {
            string name = "";
            // int lastStringIndex = args.Length - 2;

            if (!int.TryParse(args.Last(), out var cap))
            {
                cap = 5;
                // lastStringIndex++;
            }

            int i;
            for (i = 0; i < args.Length - 1; i++)
            {
                name += args[i] + (i < args.Length - 2 ? " " : "");
            }

            await News(await ReturnAppId(name, Context), cap);
        }

        public static string ReturnAppName(int appId)
        {
            KeyValue appList = SteamAppsInterface.GetAppList2()["apps"];

            KeyValue app = appList.Children.Find(x => x["appid"].AsInteger() == appId);

            return app?["name"].AsString();
        }

        public async static Task<int> ReturnAppId(string appName, ICommandContext context)
        {
            try
            {
                KeyValue appList = SteamAppsInterface.GetAppList2()["apps"];
                KeyValue app = appList.Children.Find(x => x["name"].AsString() == appName);

                return app != null ? app["appid"].AsInteger() : 0;
            }
            catch (Exception exp)
            {
                await Util.Error.BuildErrorAsync(exp, context);
                return -1;
            }
        }

        public static string GetStatus(int input)
        {
            switch (input)
            {//The user's current status. 0 - Offline, 1 - Online, 2 - Busy, 3 - Away, 4 - Snooze, 5 - looking to trade, 6 - looking to play
                case 0: return "Offline";
                case 1: return "Online";
                case 2: return "Busy";
                case 3: return "Away";
                case 4: return "Snooze";
                case 5: return "Looking to Trade";
                case 6: return "Looking to Play";
                default: return "Something's wrong...";
            }
        }

        public static int GetFriendSlots(int level, bool considerFacebook = false)
        {
            // add 50 if facebook is considered
            return 250 + (considerFacebook ? 50 : 0) + level * 5;
        }
    }
}