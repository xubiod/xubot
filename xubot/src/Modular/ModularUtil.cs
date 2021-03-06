﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule;

namespace xubot.src.Modular
{
    public static class ModularUtil
    {
        public static Discord.Embed Convert(XubotSharedModule.DiscordThings.Embed from, ICommandContext context)
        {
            if (from == null) return null;

            EmbedBuilder to = new EmbedBuilder()
            {
                Title = from.Title,
                Description = from.Description,
                ThumbnailUrl = context.Client.CurrentUser.GetAvatarUrl(),

                Footer = new EmbedFooterBuilder()
                {
                    Text = Util.Globals.EmbedFooter,
                    IconUrl = context.Client.CurrentUser.GetAvatarUrl()
                },
                Timestamp = DateTime.Now,
                Fields = from.Fields.Select(x => {
                    return new EmbedFieldBuilder() {
                        Name = x.Name,
                        Value = x.Value,
                        IsInline = x.IsInline
                    };
                }).ToList()
            };

            return to.Build();
        }

        public static async Task<IUserMessage> SendMessage(ICommandContext context, XubotSharedModule.DiscordThings.SendableMsg message)
        {
            if (message.Filepath != null)
            {
                return await context.Channel.SendFileAsync(message.Filepath, message.Text, message.isTTS, Convert(message.MsgEmbed, context), null, message.Spoilered);
            }
            else
            {
                return await context.Channel.SendMessageAsync(message.Text, message.isTTS, Convert(message.MsgEmbed, context), null);
            }
        }
    }
}
