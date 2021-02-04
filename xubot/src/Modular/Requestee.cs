using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static XubotSharedModule.Events.Requestor;

namespace xubot.src.Modular
{
    public static class Requestee
    {
        public static object Get(ICommandContext context, RequestType what, RequestProperty wanted)
        {
            if (context == null) return null;

            switch (what)
            {
                case RequestType.Guild:
                    switch (wanted)
                    {
                        case RequestProperty.Name:       return context.Guild.Name; break;
                        case RequestProperty.ID:         return context.Guild.Id; break;
                        default:                         return null; break;
                    }

                case RequestType.Channel:
                    switch (wanted)
                    {
                        case RequestProperty.Name:       return context.Channel.Name; break;
                        case RequestProperty.ID:         return context.Channel.Id; break;
                        default:                         return null; break;
                    }

                case RequestType.User:
                    switch (wanted)
                    {
                        case RequestProperty.Name:       return context.User.Username; break;
                        case RequestProperty.ID:         return context.User.Id; break;
                        case RequestProperty.Discrim:    return context.User.Discriminator; break;
                        default:                         return null; break;
                    }

                case RequestType.Client:
                case RequestType.Self:
                    switch (wanted)
                    {
                        case RequestProperty.Name:       return Program.xuClient.CurrentUser.Username; break;
                        case RequestProperty.ID:         return Program.xuClient.CurrentUser.Id; break;
                        case RequestProperty.Discrim:    return Program.xuClient.CurrentUser.DiscriminatorValue; break;
                        default:                         return null;
                    }

                case RequestType.Message:
                    switch (wanted)
                    {
                        case RequestProperty.ID:         return context.Message.Id; break;
                        case RequestProperty.Content:    return context.Message.Content; break;
                        default:                         return null; break;
                    }

                default: break;
            }

            return null;
        }
    }
}
