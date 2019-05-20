using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SteamKit2;

namespace xubot_core.src
{
    [Group("steam"), Summary("Steam API integration via SteamKit2.")]
    public class SteamKitInteg : ModuleBase
    {
        static dynamic steamUserInterface = WebAPI.GetInterface("ISteamUser", Program.keys.steam.ToString());
        static dynamic playerServiceInterface = WebAPI.GetInterface("IPlayerService", Program.keys.steam.ToString());
        static dynamic steamAppsInterface = WebAPI.GetInterface("ISteamApps", Program.keys.steam.ToString());

        [Command("user", RunMode = RunMode.Async), Summary("Gets information about a Steam user based on their ID.")]
        public async Task User(ulong id)
        {
            try {
                KeyValue ownedGames = playerServiceInterface.GetOwnedGames(steamid: id);
                KeyValue playerSummaries = steamUserInterface.GetPlayerSummaries002(steamids: id);

                playerSummaries = playerSummaries["players"].Children[0];

                decimal twoWeeks = 0;
                decimal forever = 0;

                int mostTimeIn = 0;
                decimal mostTime = 0;

                int mostWeekIn = 0;
                decimal mostWeek = 0;

                EmbedFieldBuilder mostWeekField = new EmbedFieldBuilder { Name = "Most Playtime (last 2 weeks)", Value = "Has not played in last 2 weeks.", IsInline = true };
                EmbedFieldBuilder mostTimeField = new EmbedFieldBuilder { Name = "Most Playtime (forever)", Value = "Has not played since account creation (Wha...?)", IsInline = true };

                foreach (KeyValue game in ownedGames["games"].Children)
                {
                    forever += game["playtime_forever"].AsInteger(0);
                    twoWeeks += game["playtime_2weeks"].AsInteger(0);

                    if (game["playtime_forever"].AsInteger(0) > mostTime)
                    {
                        mostTime = game["playtime_forever"].AsInteger(0);
                        mostTimeIn = game["appid"].AsInteger();
                    }

                    if (game["playtime_2weeks"].AsInteger(0) > mostWeek)
                    {
                        mostWeek = game["playtime_2weeks"].AsInteger(0);
                        mostWeekIn = game["appid"].AsInteger();
                    }
                }

                if (mostWeekIn != 0)
                {
                    mostWeekField.Value = "In App **" + ReturnAppName(mostWeekIn) + "**: " + string.Format("{0:#,###0.0}", mostWeek) + " minutes\n" + string.Format("{0:#,###0.0}", mostWeek / 60) + " hours";
                }

                if (mostTimeIn != 0)
                {
                    mostTimeField.Value = "In App **" + ReturnAppName(mostTimeIn) + "**: " + string.Format("{0:#,###0.0}", mostTime) + " minutes\n" + string.Format("{0:#,###0.0}", mostTime / 60) + " hours";
                }

                ulong _lastLogOff = playerSummaries["lastlogoff"].AsUnsignedLong(0);
                DateTime lastLogOff = GeneralTools.UnixTimeStampToDateTime(_lastLogOff);

                ulong _timeCreated = playerSummaries["timecreated"].AsUnsignedLong(0);
                DateTime timeCreated = GeneralTools.UnixTimeStampToDateTime(_timeCreated);

                TimeSpan lastLogOffToNow = DateTime.Now - lastLogOff;
                TimeSpan createdToNow = DateTime.Now - timeCreated;

                EmbedBuilder embedd = new EmbedBuilder
                {
                    Title = "Steam User: " + playerSummaries["personaname"].AsString(),
                    Color = Discord.Color.DarkBlue,
                    Description = "Data obtained Steam WebAPI using SteamKit2",
                    ThumbnailUrl = playerSummaries["avatarfull"].AsString(),

                    Footer = new EmbedFooterBuilder
                    {
                        Text = "xubot :p"
                    },
                    Timestamp = DateTime.UtcNow,
                    Fields = new List<EmbedFieldBuilder>()
                            {
                                new EmbedFieldBuilder
                                {
                                    Name = "Owned Games",
                                    Value = ownedGames["game_count"].AsString() + " products",
                                    IsInline = false
                                },
                                new EmbedFieldBuilder
                                {
                                    Name = "Playtime (last 2 weeks)",
                                    Value = string.Format("{0:#,###0.0}", twoWeeks) + " minutes\n" + string.Format("{0:#,###0.0}", twoWeeks/60) + " hours",
                                    IsInline = true
                                },
                                new EmbedFieldBuilder
                                {
                                    Name = "Playtime (forever)",
                                    Value = string.Format("{0:#,###0.0}", forever) + " minutes\n" + string.Format("{0:#,###0.0}", forever/60) + " hours\n" + string.Format("{0:#,###0.0}", forever/1440) + " days",
                                    IsInline = true
                                },
                                mostWeekField
                                ,
                                mostTimeField
                                ,
                                new EmbedFieldBuilder
                                {
                                    Name = "Last Logoff",
                                    Value = lastLogOff.ToShortDateString() + " " + lastLogOff.ToShortTimeString() + "\n(" +
                                    System.Math.Round((lastLogOffToNow.TotalHours*100)/100).ToString() + " hours)",
                                    IsInline = true
                                },
                                new EmbedFieldBuilder
                                {
                                    Name = "Time Created",
                                    Value = timeCreated.ToShortDateString() + " " + timeCreated.ToShortTimeString() + "\n(" +
                                    (System.Math.Round(createdToNow.TotalDays/3.65)/100).ToString() + " years)",
                                    IsInline = true
                                }
                            }
                };

                await ReplyAsync("", false, embedd.Build());
            }
            catch (Exception ex)
            {
                await GeneralTools.CommHandler.BuildError(ex, Context);
            }
        }

        [Command("user", RunMode = RunMode.Async), Summary("Gets information about a Steam user based on their vanity URL.")]
        public async Task User(string vanity)
        {
            KeyValue vanityUrl = steamUserInterface.ResolveVanityURL(vanityurl: vanity);

            await User(vanityUrl["steamid"].AsUnsignedLong(0));
        }

        public static string ReturnAppName(int appID)
        {
            KeyValue appList = steamAppsInterface.GetAppList2()["apps"];

            KeyValue app = appList.Children.Find(x => x["appid"].AsInteger() == appID);

            return app["name"].AsString();
        }

        public static int ReturnAppID(string appName)
        {
            KeyValue appList = steamAppsInterface.GetAppList2()["apps"];

            KeyValue app = appList.Children.Find(x => x["name"].AsString() == appName);

            return app["appid"].AsInteger();
        }
    }
}
