using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;

namespace xubot.Offline;

internal class OfflineClient : IDiscordClient
{
    public ConnectionState ConnectionState => ConnectionState.Connected;

    public ISelfUser CurrentUser => OfflineHandlers.DefaultOfflineUser;

    public TokenType TokenType => TokenType.Bot;

    public Task<IReadOnlyCollection<IApplicationCommand>> BulkOverwriteGlobalApplicationCommand(ApplicationCommandProperties[] properties, RequestOptions options = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<IApplicationCommand>> GetGlobalApplicationCommandsAsync(bool withLocalizations = false, string locale = null,
        RequestOptions options = null)
    {
        throw new NotImplementedException();
    }

    public Task<IApplicationCommand> CreateGlobalApplicationCommand(ApplicationCommandProperties properties, RequestOptions options = null)
    {
        throw new NotImplementedException();
    }

    public Task<IGuild> CreateGuildAsync(string name, IVoiceRegion region, Stream jpegIcon = null, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public void Dispose()
    {
        throw new InvalidOperationException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IApplication> GetApplicationInfoAsync(RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<BotGateway> GetBotGatewayAsync(RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IChannel> GetChannelAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IReadOnlyCollection<IConnection>> GetConnectionsAsync(RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IReadOnlyCollection<IDMChannel>> GetDMChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IApplicationCommand> GetGlobalApplicationCommandAsync(ulong id, RequestOptions options = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<IApplicationCommand>> GetGlobalApplicationCommandsAsync(RequestOptions options = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<IGroupChannel>> GetGroupChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IGuild> GetGuildAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IReadOnlyCollection<IGuild>> GetGuildsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IInvite> GetInviteAsync(string inviteId, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IReadOnlyCollection<IPrivateChannel>> GetPrivateChannelsAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<int> GetRecommendedShardCountAsync(RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IUser> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IUser> GetUserAsync(string username, string discriminator, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IVoiceRegion> GetVoiceRegionAsync(string id, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IReadOnlyCollection<IVoiceRegion>> GetVoiceRegionsAsync(RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task<IWebhook> GetWebhookAsync(ulong id, RequestOptions options = null)
    {
        throw new InvalidOperationException();
    }

    public Task StartAsync()
    {
        throw new InvalidOperationException();
    }

    public Task StopAsync()
    {
        throw new InvalidOperationException();
    }
}