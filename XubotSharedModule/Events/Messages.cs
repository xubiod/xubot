using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule.DiscordThings;

namespace XubotSharedModule.Events
{
    public static class Messages
    {
        public delegate Task MessageHandler(Message message);
        public static event MessageHandler OnMessageSend;

        public static async Task Send(Message message)
        {
            await OnMessageSend?.Invoke(message);
        }
    }
}
