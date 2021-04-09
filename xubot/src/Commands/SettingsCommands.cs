using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Commands
{
    [Group("settings")]
    public class SettingsCommands : ModuleBase
    {
        [Command("get")]
        public async Task Get(string key)
        {
            object value = Util.Settings.Get(key);

            await ReplyAsync($"`{key}` is currently set to `{value}`.");
        }

        public async Task SetOperation<T>(string key, T newValue)
        {
            Util.Settings.Set<T>(key, newValue);

            await ReplyAsync($"`{key}` has been set to `{newValue}`.\n**These changes will not persist until they are saved.**");
        }

        [Command("set"), RequireOwner]
        public async Task Set(string key, params string[] newValue)
        {
            string all = "";
            foreach (string _ in newValue)
                all += _ + (_ != newValue.Last() ? " " : "");

            await SetOperation<string>(key, all);
        }

        [Command("set"), RequireOwner]
        public async Task Set(string key, int newValue)
        {
            await SetOperation<int>(key, newValue);
        }

        [Command("set"), RequireOwner]
        public async Task Set(string key, bool newValue)
        {
            await SetOperation<bool>(key, newValue);
        }

        [Command("save"), RequireOwner]
        public async Task Save()
        {
            BotSettings.Global.Default.Save();

            await ReplyAsync($"All setting changes have been saved.");
        }

        [Command("reload"), RequireOwner]
        public async Task Reload()
        {
            BotSettings.Global.Default.Reload();

            await ReplyAsync($"All setting changes have been undone.");
        }

        [Command("reset"), RequireOwner]
        public async Task Reset()
        {
            BotSettings.Global.Default.Reset();

            await ReplyAsync($"All setting changes have been reset to default values.");
        }

        [Command("list")]
        public async Task List(int page = 1)
        {
            if (page < 1) page = 1;
            int itemsPerPage = BotSettings.Global.Default.EmbedListMaxLength;

            SettingsProperty[] collection = new SettingsProperty[BotSettings.Global.Default.Properties.Count];
            BotSettings.Global.Default.Properties.CopyTo(collection, 0);

            string items = "";

            int limit = System.Math.Min(collection.Length - ((page - 1) * itemsPerPage), itemsPerPage);

            int index;
            for (int i = 0; i < limit; i++)
            {
                index = i + (itemsPerPage * (page - 1));

                if (index > collection.Length - 1) { break; }

                items += $"{collection[index].Name}: {Util.Settings.Get(collection[index].Name)}\n";
            }

            if (items == "") items = "There's nothing here, I think you went out of bounds.";

            EmbedBuilder embedd = new EmbedBuilder
            {
                Title = "Settings list",
                Color = Discord.Color.DarkBlue,
                Description = $"Showing page #{page} out of {System.Math.Ceiling((float)collection.Length / itemsPerPage)} pages.\nShowing a few of the **{collection.Length}** settings.\n**Note: Only the bot owner can set these.**",
                ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),

                Footer = new EmbedFooterBuilder
                {
                    Text = Util.Globals.EmbedFooter,
                    IconUrl = Context.Client.CurrentUser.GetAvatarUrl()
                },
                Timestamp = DateTime.UtcNow,
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder
                    {
                        Name = "List",
                        Value = $"```\n{items}```" ,
                        IsInline = true
                    }
                }
            };
            await ReplyAsync("", false, embedd.Build());
        }
    }
}
