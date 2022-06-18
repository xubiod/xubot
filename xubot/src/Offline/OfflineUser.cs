using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;

namespace xubot.Offline
{
    internal class OfflineUser : ISelfUser
    {
        public string AvatarId => null;

        public string Discriminator => "0000";

        public ushort DiscriminatorValue => 0000;

        public bool IsBot => false;

        public bool IsWebhook => false;

        public string Username => "OfflineUser";

        public UserProperties? PublicFlags => null;

        public DateTimeOffset CreatedAt => DateTimeOffset.MinValue;

        public ulong Id => 0;

        public string Mention => "<@OfflineUser>";

        public IActivity Activity => null;

        public UserStatus Status => UserStatus.Online;

        public List<ClientType> DefaultClients = new() { ClientType.Desktop };
        public IImmutableSet<ClientType> ActiveClients => (IImmutableSet<ClientType>)DefaultClients;

        public List<IActivity> DefaultActivities = null;
        public IImmutableList<IActivity> Activities => throw new NotImplementedException();

        public string Email => "";

        public bool IsVerified => true;

        public bool IsMfaEnabled => true;

        public UserProperties Flags => throw new NotImplementedException();

        public PremiumType PremiumType => PremiumType.None;

        public string Locale => throw new NotImplementedException();

        IReadOnlyCollection<ClientType> IPresence.ActiveClients => throw new NotImplementedException();

        IReadOnlyCollection<IActivity> IPresence.Activities => throw new NotImplementedException();

        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            return "";
        }

        public string GetDefaultAvatarUrl()
        {
            return "";
        }

        public Task<IDMChannel> GetOrCreateDMChannelAsync(RequestOptions options = null)
        {
            return (Task<IDMChannel>)(OfflineHandlers.DefaultOfflineChannel as IDMChannel);
        }

        public Task ModifyAsync(Action<SelfUserProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}
