using Discord.Commands;
using static XubotSharedModule.Events.Requestor;

namespace xubot.Modular
{
    public static class Requestee
    {
        public static object Get(ICommandContext context, RequestType what, RequestProperty wanted)
        {
            if (context == null) return null;

            return what switch
            {
                RequestType.Guild => GuildRequest(context, wanted),
                RequestType.Channel => ChannelRequest(context, wanted),
                RequestType.User => UserRequest(context, wanted),
                RequestType.Client or RequestType.Self => ClientRequest(context, wanted),
                RequestType.Message => MessageRequest(context, wanted),
                _ => null,
            };
        }

        private static object GuildRequest(ICommandContext context, RequestProperty wanted)
        {
            return wanted switch
            {
                RequestProperty.Name => context.Guild.Name,
                RequestProperty.ID => context.Guild.Id,
                _ => null,
            };
        }

        private static object ChannelRequest(ICommandContext context, RequestProperty wanted)
        {
            return wanted switch
            {
                RequestProperty.Name => context.Channel.Name,
                RequestProperty.ID => context.Channel.Id,
                _ => null,
            };
        }

        private static object UserRequest(ICommandContext context, RequestProperty wanted)
        {
            return wanted switch
            {
                RequestProperty.Name => context.User.Username,
                RequestProperty.ID => context.User.Id,
                RequestProperty.Discrim => context.User.Discriminator,
                _ => null,
            };
        }

        private static object ClientRequest(ICommandContext context, RequestProperty wanted)
        {
            return wanted switch
            {
                RequestProperty.Name => Program.XuClient.CurrentUser.Username,
                RequestProperty.ID => Program.XuClient.CurrentUser.Id,
                RequestProperty.Discrim => Program.XuClient.CurrentUser.DiscriminatorValue,
                _ => null,
            };
        }

        private static object MessageRequest(ICommandContext context, RequestProperty wanted)
        {
            return wanted switch
            {
                RequestProperty.ID => context.Message.Id,
                RequestProperty.Content => context.Message.Content,
                _ => null,
            };
        }
    }
}
