using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace xubot.Offline
{
    internal class OfflineMessage : IUserMessage
    {
        public IUserMessage ReferencedMessage { get; set; }

        public MessageType Type { get; set; }

        public MessageSource Source { get; set; }

        public bool IsTTS => false;

        public bool IsPinned { get; set; }

        public bool IsSuppressed { get; set; }

        public bool MentionedEveryone => false;

        public string Content { get; set; }

        public DateTimeOffset Timestamp => throw new NotImplementedException();

        public DateTimeOffset? EditedTimestamp => throw new NotImplementedException();

        public IMessageChannel Channel => OfflineHandlers.DefaultOfflineChannel;

        public IUser Author => OfflineHandlers.DefaultOfflineUser;
        public IThreadChannel Thread { get; }

        public IReadOnlyCollection<IAttachment> Attachments => throw new NotImplementedException();

        public List<IEmbed> EmbedsWritable = new();
        public IReadOnlyCollection<IEmbed> Embeds => EmbedsWritable;

        public IReadOnlyCollection<ITag> Tags => throw new NotImplementedException();

        public IReadOnlyCollection<ulong> MentionedChannelIds => throw new NotImplementedException();

        public IReadOnlyCollection<ulong> MentionedRoleIds => throw new NotImplementedException();

        public IReadOnlyCollection<ulong> MentionedUserIds => throw new NotImplementedException();

        public MessageActivity Activity => throw new NotImplementedException();

        public MessageApplication Application => throw new NotImplementedException();

        public MessageReference Reference => throw new NotImplementedException();

        public Dictionary<IEmote, ReactionMetadata> ReactionsWritable = new();
        public IReadOnlyDictionary<IEmote, ReactionMetadata> Reactions => ReactionsWritable;

        public MessageFlags? Flags => throw new NotImplementedException();

        public DateTimeOffset CreatedAt => DateTimeOffset.Now;

        public ulong Id { get; }

        public IReadOnlyCollection<ISticker> Stickers => throw new NotImplementedException();

        public string CleanContent => throw new NotImplementedException();

        public IReadOnlyCollection<IMessageComponent> Components => throw new NotImplementedException();

        IReadOnlyCollection<IStickerItem> IMessage.Stickers => throw new NotImplementedException();

        public IMessageInteraction Interaction => throw new NotImplementedException();
        public MessageRoleSubscriptionData RoleSubscriptionData { get; }

        public OfflineMessage()
        {
            Id = OfflineHandlers.DefaultOfflineChannel.LastMessageId++;
        }

        public Task AddReactionAsync(IEmote emote, RequestOptions options = null)
        {
            ReactionsWritable.Add(emote, new ReactionMetadata());
            return Task.CompletedTask;
        }

        public Task CrosspostAsync(RequestOptions options = null)
        {
            throw new InvalidOperationException();
        }

        public Task DeleteAsync(RequestOptions options = null)
        {
            return Task.CompletedTask;
        }

        public IAsyncEnumerable<IReadOnlyCollection<IUser>> GetReactionUsersAsync(IEmote emoji, int limit, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task ModifyAsync(Action<MessageProperties> func, RequestOptions options = null)
        {
            MessageProperties messageProperties = new MessageProperties
            {
                Content = Content
            };

            func(messageProperties);

            if (messageProperties.Content.IsSpecified) Content = messageProperties.Content.Value;

            return Task.CompletedTask;
        }

        public Task ModifySuppressionAsync(bool suppressEmbeds, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task PinAsync(RequestOptions options = null)
        {
            IsPinned = true;
            return Task.CompletedTask;
        }

        public Task RemoveAllReactionsAsync(RequestOptions options = null)
        {
            ReactionsWritable.Clear();
            return Task.CompletedTask;
        }

        public Task RemoveAllReactionsForEmoteAsync(IEmote emote, RequestOptions options = null)
        {
            ReactionsWritable.Remove(emote);
            return Task.CompletedTask;
        }

        public Task RemoveReactionAsync(IEmote emote, IUser user, RequestOptions options = null)
        {
            return RemoveAllReactionsForEmoteAsync(emote, options);
        }

        public Task RemoveReactionAsync(IEmote emote, ulong userId, RequestOptions options = null)
        {
            return RemoveAllReactionsForEmoteAsync(emote, options);
        }

        public string Resolve(TagHandling userHandling = TagHandling.Name, TagHandling channelHandling = TagHandling.Name, TagHandling roleHandling = TagHandling.Name, TagHandling everyoneHandling = TagHandling.Ignore, TagHandling emojiHandling = TagHandling.Name)
        {
            string emit = Content;

            foreach (IEmbed embed in Embeds)
            {
                if (embed != null)
                {
                    emit += $"[RESP.]\n--- {embed.Title} ---\n{embed.Description}\n";

                    foreach (EmbedField field in embed.Fields)
                    {
                        emit += $"\n- {field.Name} -\n{field.Value}";
                    }
                    emit += $"\n{embed.Footer}";
                }
            }

            return emit;
        }

        public Task UnpinAsync(RequestOptions options = null)
        {
            IsPinned = false;
            return Task.CompletedTask;
        }
    }
}
