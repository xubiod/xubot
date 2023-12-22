using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace xubot.Commands
{
    [Group("settings")]
    public class SettingsCommands : ModuleBase
    {
        [Command("get"), Alias("return")]
        public async Task Get(string key)
        {
            var value = Util.Settings.Get(key);

            await ReplyAsync($"`{key}` is currently set to `{value}`.");
        }

        public async Task SetOperation<T>(string key, T newValue)
        {
            Util.Settings.Set(key, newValue);

            await ReplyAsync($"`{key}` has been set to `{newValue}`.\n**These changes will not persist until they are saved.**");
        }

        [Command("set"), RequireOwner]
        public async Task Set(string key, params string[] newValue)
        {
            var all = "";
            foreach (var _ in newValue)
                all += _ + (_ != newValue.Last() ? " " : "");

            await SetOperation(key, all);
        }

        [Command("set"), RequireOwner]
        public async Task Set(string key, int newValue)
        {
            await SetOperation(key, newValue);
        }

        [Command("set"), RequireOwner]
        public async Task Set(string key, bool newValue)
        {
            await SetOperation(key, newValue);
        }

        [Command("save"), RequireOwner]
        public async Task Save()
        {
            src.BotSettings.Global.Default.Save();

            await ReplyAsync("All setting changes have been saved.");
        }

        [Command("reload"), Alias("undo", "unset"), RequireOwner]
        public async Task Reload()
        {
            src.BotSettings.Global.Default.Reload();

            await ReplyAsync("All setting changes have been undone.");
        }

        [Command("reset"), Alias("default"), RequireOwner]
        public async Task Reset()
        {
            src.BotSettings.Global.Default.Reset();

            await ReplyAsync("All setting changes have been reset to default values.");
        }

        [Command("list")]
        public async Task List(int page = 1)
        {
            if (page < 1) page = 1;
            var itemsPerPage = src.BotSettings.Global.Default.EmbedListMaxLength;

            var collection = new SettingsProperty[src.BotSettings.Global.Default.Properties.Count];
            src.BotSettings.Global.Default.Properties.CopyTo(collection, 0);

            var items = "";

            var limit = System.Math.Min(collection.Length - (page - 1) * itemsPerPage, itemsPerPage);

            int index;
            for (var i = 0; i < limit; i++)
            {
                index = i + itemsPerPage * (page - 1);

                if (index > collection.Length - 1) { break; }

                items += $"{collection[index].Name}: ({Util.String.SimplifyTypes(Util.Settings.Get(collection[index].Name).GetType().ToString())}) {Util.Settings.Get(collection[index].Name)}\n";
            }

            if (string.IsNullOrWhiteSpace(items)) items = "There's nothing here, I think you went out of bounds.";

            var embed = Util.Embed.GetDefaultEmbed(
                Context, "Settings list", $"Showing page #{page} out of {System.Math.Ceiling((float)collection.Length / itemsPerPage)} pages.\n" +
                $"Showing a few of the **{collection.Length}** settings.\n**Note: Only the bot owner can set these.**", Color.DarkBlue
            );

            embed.Fields = new List<EmbedFieldBuilder>
            {
                new()
                {
                    Name = "List",
                    Value = $"```\n{items}```" ,
                    IsInline = true
                }
            };

            await ReplyAsync("", false, embed.Build());
        }
    }
}
