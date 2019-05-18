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
        [Command("user"), Summary("Gets information about a Steam user based on their ID.")]
        public async Task User(string id)
        {
            try {
                dynamic playerService = WebAPI.GetInterface("IPlayerService", Program.keys.steam.ToString());
                dynamic steamUser = WebAPI.GetInterface("ISteamUser", Program.keys.steam.ToString());

                KeyValue ownedGames = playerService.GetOwnedGames(steamid: id);
                KeyValue playerSummaries = steamUser.GetPlayerSummaries002(steamids: id);

                playerSummaries = playerSummaries["players"].Children[0];

                int twoWeeks = 0;
                int forever = 0;

                string mostTimeIn = "";
                int mostTime = 0;

                foreach (KeyValue game in ownedGames["games"].Children)
                {
                    forever += game["playtime_forever"].AsInteger(0);
                    twoWeeks += game["playtime_2weeks"].AsInteger(0);

                    if (game["playtime_forever"].AsInteger(0) > mostTime)
                    {
                        mostTime = game["playtime_forever"].AsInteger(0);
                        mostTimeIn = game["appid"].AsString();
                    }
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
                                    Value = string.Format("{0:#,###0}", twoWeeks) + " minutes\n" + string.Format("{0:#,###0}", twoWeeks/60) + " hours",
                                    IsInline = true
                                },
                                new EmbedFieldBuilder
                                {
                                    Name = "Playtime (forever)",
                                    Value = string.Format("{0:#,###0}", forever) + " minutes\n" + string.Format("{0:#,###0}", forever/60) + " hours\n" + string.Format("{0:#,###0}", forever/1440) + " days",
                                    IsInline = true
                                },
                                new EmbedFieldBuilder
                                {
                                    Name = "Most Playtime",
                                    Value = "In App " + mostTimeIn + ": " + string.Format("{0:#,###0}", mostTime) + " minutes\n" + string.Format("{0:#,###0}", mostTime/60) + " hours",
                                    IsInline = true
                                },
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
    }
}
