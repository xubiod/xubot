﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Commands
{
    [Group("su"), Summary("A couple of owner only commands."), RequireOwner, RequireContext(ContextType.DM)]
    public class OwnerController : ModuleBase
    {
        [Command("list-directory")]
        public async Task ListDirectory(string where)
        {
            string[] files = Directory.GetFiles(where);

            string result = "";

            foreach (string file in files)
            {
                result += file + "\n";
            }

            if (result.Length > 1018)
            {
                result = result.Substring(0, 1015) + "...";
            }

            EmbedBuilder embed = Util.Embed.GetDefaultEmbed(Context, "Superuser - Directory", where, Discord.Color.DarkOrange);
            embed.Fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder
                {
                    Name = "Result",
                    Value = $"```{result}```",
                    IsInline = true
                }
            };
            await ReplyAsync("", false, embed.Build());
        }

        [Command("update")]
        public async Task UpdateBot(/* location for update script */)
        {
            // run update script/application
            // then kill xubot
            // with a msg
        }
    }
}
