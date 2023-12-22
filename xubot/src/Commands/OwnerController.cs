using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace xubot.Commands
{
    [Group("su"), Summary("A couple of owner only commands."), RequireOwner, RequireContext(ContextType.DM)]
    public class OwnerController : ModuleBase
    {
        [Command("list-directory")]
        public async Task ListDirectory(string where)
        {
            var files = Directory.GetFiles(where);

            var result = "";

            foreach (var file in files)
            {
                result += file + "\n";
            }

            if (result.Length > 1018)
            {
                result = result.Substring(0, 1015) + "...";
            }

            var embed = Util.Embed.GetDefaultEmbed(Context, "Superuser - Directory", where, Color.DarkOrange);
            embed.Fields =
            [
                new()
                {
                    Name = "Result",
                    Value = $"```{result}```",
                    IsInline = true
                }
            ];
            await ReplyAsync("", false, embed.Build());
        }

        //[Command("update")]
        //public async Task UpdateBot(/* location for update script */)
        //{
        //    // run update script/application
        //    // then kill xubot
        //    // with a msg
        //}
    }
}
