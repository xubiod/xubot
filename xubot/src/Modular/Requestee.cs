using Discord.Commands;
using static XubotSharedModule.Events.Requestor;

namespace xubot.Modular
{
    public static class Requestee
    {
        public static object Get(ICommandContext context, RequestType what, RequestProperty wanted)
        {
            if (context == null) return null;

            switch (what)
            {
                case RequestType.Guild:
                    return GuildRequest(context, wanted);

                case RequestType.Channel:
                    return ChannelRequest(context, wanted);

                case RequestType.User:
                    return UserRequest(context, wanted);

                case RequestType.Client:
                case RequestType.Self:
                    return ClientRequest(context, wanted);

                case RequestType.Message:
                    return MessageRequest(context, wanted);
            }

            return null;
        }

        private static object GuildRequest(ICommandContext context, RequestProperty wanted)
        {
            switch (wanted)
            {
                case RequestProperty.Name: return context.Guild.Name; break;
                case RequestProperty.ID: return context.Guild.Id; break;
                default: return null; break;
            }
        }

        private static object ChannelRequest(ICommandContext context, RequestProperty wanted)
        {
            switch (wanted)
            {
                case RequestProperty.Name: return context.Channel.Name; break;
                case RequestProperty.ID: return context.Channel.Id; break;
                default: return null; break;
            }
        }

        private static object UserRequest(ICommandContext context, RequestProperty wanted)
        {
            switch (wanted)
            {
                case RequestProperty.Name: return context.User.Username; break;
                case RequestProperty.ID: return context.User.Id; break;
                case RequestProperty.Discrim: return context.User.Discriminator; break;
                default: return null; break;
            }
        }

        private static object ClientRequest(ICommandContext context, RequestProperty wanted)
        {
            switch (wanted)
            {
                case RequestProperty.Name: return Program.XuClient.CurrentUser.Username; break;
                case RequestProperty.ID: return Program.XuClient.CurrentUser.Id; break;
                case RequestProperty.Discrim: return Program.XuClient.CurrentUser.DiscriminatorValue; break;
                default: return null;
            }
        }

        private static object MessageRequest(ICommandContext context, RequestProperty wanted)
        {
            switch (wanted)
            {
                case RequestProperty.ID: return context.Message.Id; break;
                case RequestProperty.Content: return context.Message.Content; break;
                default: return null; break;
            }
        }
    }
}
