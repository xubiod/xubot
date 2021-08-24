﻿using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xubot.src.Offline
{
    class OfflineChannel : IMessageChannel, IDMChannel
    {
        public string Name => "OfflineConsole";

        public DateTimeOffset CreatedAt => DateTimeOffset.Now;

        public ulong Id => 0;

        public ulong LastMessageId = 0;

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
            IReadOnlyCollection<IMessage> results = Messages.GetRange(0, limit);
            return (IAsyncEnumerable<IReadOnlyCollection<IMessage>>)results.GetEnumerator();
        }

        public IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMessagesAsync(ulong fromMessageId, Direction dir, int limit = 100, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            int index = Messages.FindIndex(x => x.Id == fromMessageId);

            if (dir == Direction.Before)
            {
                index = Math.Max(0, index - limit);
            }
            else if (dir == Direction.Around)
            {
                index = Math.Max(0, index - (int)(limit / 2));
            }

            IReadOnlyCollection<IMessage> results = Messages.GetRange(index, limit);
            return (IAsyncEnumerable<IReadOnlyCollection<IMessage>>)results.GetEnumerator();
        }

        public IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMessagesAsync(IMessage fromMessage, Direction dir, int limit = 100, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            return GetMessagesAsync(fromMessage.Id, dir, limit, mode, options);
        }

        public Task<IReadOnlyCollection<IMessage>> GetPinnedMessagesAsync(RequestOptions options = null)
        {
            return null;
        }

        public Task<IUser> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            return Task.FromResult(OfflineHandlers.DefaultOfflineUser as IUser);
        }

        public IAsyncEnumerable<IReadOnlyCollection<IUser>> GetUsersAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            return null;
        }

        public Task<IUserMessage> SendFileAsync(string filePath, string text = null, bool isTts = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
        {
            return SendMessageAsync(text, isTts, embed, options, allowedMentions, messageReference);
        }

        public Task<IUserMessage> SendFileAsync(Stream stream, string filename, string text = null, bool isTts = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
        {
            return SendMessageAsync(text, isTts, embed, options, allowedMentions, messageReference);
        }

        public Task<IUserMessage> SendMessageAsync(string text = null, bool isTts = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
        {
            OfflineMessage newMsg = new OfflineMessage() {
                Content = text
            };

            newMsg.EmbedsWritable.Add(embed);

            Messages.Add(newMsg);
            Console.WriteLine($"{newMsg.Resolve()}\n");

            return Task.FromResult<IUserMessage>(newMsg);
        }

        public Task TriggerTypingAsync(RequestOptions options = null)
        {
            return null;
        }

        public Task CloseAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IUserMessage> ModifyMessageAsync(ulong messageId, Action<MessageProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}
