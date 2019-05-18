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
            dynamic playerService = WebAPI.GetInterface("IPlayerService", Program.keys.steam.ToString());

            KeyValue ownedGames = playerService.GetOwnedGames( steamid: id );

            await ReplyAsync(ownedGames["game_count"].AsString());
        }
    }
}
