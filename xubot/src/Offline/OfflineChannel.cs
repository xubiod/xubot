using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Offline
{
    class OfflineChannel : IDMChannel, IMessageChannel
    {
        public string Name => "OfflineConsole";

        public DateTimeOffset CreatedAt => DateTimeOffset.Now;

        public ulong Id => 0;

        public IUser Recipient => throw new NotImplementedException();

        public IReadOnlyCollection<IUser> Recipients => throw new NotImplementedException();

        public List<IMessage> Messages = new List<IMessage>();

        public Task DeleteMessageAsync(ulong messageId, RequestOptions options = null)
        {
            Messages.RemoveAll(x => x.Id == messageId);
            return Task.CompletedTask;
        }

        public Task DeleteMessageAsync(IMessage message, RequestOptions options = null)
        {
            return DeleteMessageAsync(message.Id, options);
        }

        public IDisposable EnterTypingState(RequestOptions options = null)
        {
            return null;
        }

        public Task<IMessage> GetMessageAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            return Task.FromResult<IMessage>(Messages.Find(x => x.Id == id));
        }

        public IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMessagesAsync(int limit = 100, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMessagesAsync(ulong fromMessageId, Direction dir, int limit = 100, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMessagesAsync(IMessage fromMessage, Direction dir, int limit = 100, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<IMessage>> GetPinnedMessagesAsync(RequestOptions options = null)
        {
            return null;
        }

        public Task<IUser> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            return null;
        }

        public IAsyncEnumerable<IReadOnlyCollection<IUser>> GetUsersAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            return null;
        }

        public Task<IUserMessage> SendFileAsync(string filePath, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
        {
            return SendMessageAsync(text, isTTS, embed, options, allowedMentions, messageReference);
        }

        public Task<IUserMessage> SendFileAsync(Stream stream, string filename, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
        {
            return SendMessageAsync(text, isTTS, embed, options, allowedMentions, messageReference);
        }

        public Task<IUserMessage> SendMessageAsync(string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
        {
            OfflineMessage new_msg = new OfflineMessage() {
                Content = text
            };

            Messages.Add(new_msg);

            return Task.FromResult<IUserMessage>(new_msg);
        }

        public Task TriggerTypingAsync(RequestOptions options = null)
        {
            return null;
        }

        public Task CloseAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}
