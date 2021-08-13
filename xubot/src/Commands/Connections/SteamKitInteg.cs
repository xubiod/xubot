using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SteamKit2;
using xubot.src.Attributes;

namespace xubot.src.Commands.Connections
{
    [Group("steam"), Summary("Steam API integration via SteamKit2.")]
    public class SteamKitInteg : ModuleBase
    {
        static dynamic steamUserInterface = WebAPI.GetInterface("ISteamUser", Program.JSONKeys["keys"].Contents.steam.ToString());
        static dynamic playerServiceInterface = WebAPI.GetInterface("IPlayerService", Program.JSONKeys["keys"].Contents.steam.ToString());
        static dynamic steamAppsInterface = WebAPI.GetInterface("ISteamApps", Program.JSONKeys["keys"].Contents.steam.ToString());
        static dynamic steamNewsInterface = WebAPI.GetInterface("ISteamNews", Program.JSONKeys["keys"].Contents.steam.ToString());

        [Example("76561197960287930")]
        [Command("user", RunMode = RunMode.Async), Summary("Gets information about a Steam user based on their ID.")]
        public async Task User(ulong id)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
            {
                try
                {
                    KeyValue ownedGames = playerServiceInterface.GetOwnedGames(steamid: id, include_appinfo: 1);
                    KeyValue playerSummaries = steamUserInterface.GetPlayerSummaries002(steamids: id);
                    KeyValue playerLevel = playerServiceInterface.GetSteamLevel(steamid: id);

                    playerSummaries = playerSummaries["players"].Children[0];

                    decimal twoWeeks = 0;
                    decimal forever = 0;

                    string mostTimeIn = "";
                    decimal mostTime = 0;

                    string mostWeekIn = "";
                    decimal mostWeek = 0;

                    EmbedFieldBuilder mostWeekField = new EmbedFieldBuilder { Name = "Most Playtime (2 wks)", Value = "Has not played in last 2 weeks.", IsInline = true };
                    EmbedFieldBuilder mostTimeField = new EmbedFieldBuilder { Name = "Most Playtime (forever)", Value = "Has not played since account creation (Wha...?)", IsInline = true };

                    foreach (KeyValue game in ownedGames["games"].Children)
                    {
                        forever += game["playtime_forever"].AsInteger(0);
                        twoWeeks += game["playtime_2weeks"].AsInteger(0);

                        if (game["playtime_forever"].AsInteger(0) > mostTime)
                        {
                            mostTime = game["playtime_forever"].AsInteger(0);
                            mostTimeIn = game["name"].AsString();
                        }

                        if (game["playtime_2weeks"].AsInteger(0) > mostWeek)
                        {
                            mostWeek = game["playtime_2weeks"].AsInteger(0);
                            mostWeekIn = game["name"].AsString();
                        }
                    }

                    if (mostWeekIn != "")
                    {
                        mostWeekField.Value = $"In App\n**__{mostWeekIn}__**: {string.Format("{0:#,###}", mostWeek)} minutes\n{string.Format("{0:#,###0.0}", mostWeek / 60)} hours";
                    }

                    if (mostTimeIn != "")
                    {
                        mostTimeField.Value = $"In App\n**__{mostTimeIn}__**: {string.Format("{0:#,###}", mostTime)} minutes\n{string.Format("{0:#,###0.0}", mostTime / 60)} hours";
                    }

                    ulong _lastLogOff = playerSummaries["lastlogoff"].AsUnsignedLong(0);
                    DateTime lastLogOff = Util.UnixTimeStampToDateTime(_lastLogOff);

                    ulong _timeCreated = playerSummaries["timecreated"].AsUnsignedLong(0);
                    DateTime timeCreated = Util.UnixTimeStampToDateTime(_timeCreated);

                    TimeSpan lastLogOffToNow = DateTime.Now - lastLogOff;
                    TimeSpan createdToNow = DateTime.Now - timeCreated;

                    string playing = "";
                    if (playerSummaries["gameid"].AsInteger(0) != 0) playing = $"Currently playing **{ReturnAppName(playerSummaries["gameid"].AsInteger())}**";

                    EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Steam User: {playerSummaries["personaname"].AsString()} ({id.ToString()})", "Data obtained Steam WebAPI using SteamKit2", Discord.Color.DarkBlue);
                    embed.ThumbnailUrl = playerSummaries["avatarfull"].AsString();

                    if (playerSummaries["communityvisibilitystate"].AsInteger(1) == 3 /* public, don't ask why */)
                    {
                        embed.Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Current Stats",
                                Value = $"Currently __{GetStatus(playerSummaries["personastate"].AsInteger())}__\n" +
                                        $"Level **{playerLevel["player_level"].AsString()}**\n**" +
                                        $"{ownedGames["game_count"].AsString()}** products\n**" +
                                        $"{GetFriendSlots(playerLevel["player_level"].AsInteger())}** friend slots" +
                                        $"{playing}",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Playtime (2 wks)",
                                Value = $"{string.Format("{0:#,##0}", twoWeeks)} minutes\n{string.Format("{0:#,###0.0}", twoWeeks/60)} hours",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Playtime (forever)",
                                Value = $"{string.Format("{0:#,##0}", forever)} minutes\n{string.Format("{0:#,###0.0}", forever/60)} hours\n{string.Format("{0:#,###0.00}", forever/1440)} days",
                                IsInline = true
                            },
                            mostWeekField,
                            mostTimeField,
                            new EmbedFieldBuilder
                            {
                                Name = "Last Logoff",
                                Value = $"{lastLogOff.ToShortDateString()} {lastLogOff.ToShortTimeString()}\n(" +
                                $"{System.Math.Round((lastLogOffToNow.TotalHours*100)/100).ToString()} hours)",
                                IsInline = true
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Time Created",
                                Value = $"{timeCreated.ToShortDateString()} {timeCreated.ToShortTimeString()}\n(" +
                                $"{string.Format("{0:#,###.00}", (createdToNow.TotalDays/365))} years)",
                                IsInline = true
                            }
                        };
                    }
                    else
                    {
                        embed.Fields = new List<EmbedFieldBuilder>()
                        {
                            new EmbedFieldBuilder
                            {
                                Name = "Current Stats",
                                Value = $"**This user's profile is private.**\nCurrently __{GetStatus(playerSummaries["personastate"].AsInteger())}__",
                                IsInline = false
                            },
                            new EmbedFieldBuilder
                            {
                                Name = "Last Logoff",
                                Value = $"{lastLogOff.ToShortDateString()} {lastLogOff.ToShortTimeString()}\n(" +
                                $"{System.Math.Round((lastLogOffToNow.TotalHours*100)/100).ToString()} hours)",
                                IsInline = false
                            }
                        };
                    }

                    await ReplyAsync("", false, embed.Build());
                }
                catch (Exception ex)
                {
                    await Util.Error.BuildError(ex, Context);
                }
            }
        }

        [Example("gabelogannewell")]
        [Command("user", RunMode = RunMode.Async), Summary("Gets information about a Steam user based on their vanity URL.")]
        public async Task User(string vanity)
        {
            KeyValue vanityUrl = steamUserInterface.ResolveVanityURL(vanityurl: vanity);

            await User(vanityUrl["steamid"].AsUnsignedLong(0));
        }

        [Example("4000")]
        [Command("news", RunMode = RunMode.Async), Summary("Gets news for a game via it's ID.")]
        public async Task News(int appid, int cap = 5)
        {
            using (Util.WorkingBlock wb = new Util.WorkingBlock(Context))
            {
                try
                {
                    KeyValue news = steamNewsInterface.GetNewsForApp0002(appid: appid);

                    news = news["newsitems"];
                    int amount = System.Math.Min(System.Math.Min(news.Children.Count, cap), 5);

                    List<EmbedFieldBuilder> article_details = new List<EmbedFieldBuilder>();

                    for (int i = 0; i < amount; i++)
                    {
                        Uri article_URI = new Uri(news.Children[i]["url"].AsString());

                        string label = "";
                        if (news.Children[i]["feedlabel"].AsString() != "Community Announcements") label = "\n*(" + news.Children[i]["feedlabel"].AsString() + ")*";

                        article_details.Add(new EmbedFieldBuilder
                        {
                            Name = news.Children[i]["title"].AsString() + label,
                            Value = "[Go to article (" + article_URI.Host + ")](" +
                                    news.Children[i]["url"].AsString() + ")"
                        });
                    }

                    EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, $"Latest {amount} news articles for the app: {ReturnAppName(appid)}", "Data obtained Steam WebAPI using SteamKit2", Discord.Color.DarkBlue);

                    await ReplyAsync("", false, embed.Build());
                }
                catch (Exception exp)
                {
                    await Util.Error.BuildError(exp, Context);
                }
            }
        }

        [Example("Garry's Mod 3")]
        [Command("news", RunMode = RunMode.Async), Summary("Gets news for a game via it's name on Steam. (HAS TO BE EXACT)")]
        public async Task News(params string[] args)
        {
            string name = "";
            int cap;
            int last_string_index = args.Length - 2;

            if (!int.TryParse(args.Last(), out cap))
            {
                cap = 5;
                last_string_index++;
            }

            int i;
            for (i = 0; i < args.Length - 1; i++)
            {
                name += args[i] + (i < args.Length - 2 ? " " : "");
            }

            await News(ReturnAppID(name, Context), cap);
        }

        public static string ReturnAppName(int appID)
        {
            KeyValue appList = steamAppsInterface.GetAppList2()["apps"];

            KeyValue app = appList.Children.Find(x => x["appid"].AsInteger() == appID);

            return app["name"].AsString();
        }

        public static int ReturnAppID(string appName, ICommandContext context)
        {
            try
            {
                KeyValue appList = steamAppsInterface.GetAppList2()["apps"];
                KeyValue app = appList.Children.Find(x => x["name"].AsString() == appName);

                return app["appid"].AsInteger();
            }
            catch (Exception exp)
            {
                Util.Error.BuildError(exp, context);
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
            return 250 + (considerFacebook ? 50 : 0) + (level * 5);
        }
    }
}
