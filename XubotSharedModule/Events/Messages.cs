using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule.DiscordThings;

namespace XubotSharedModule.Events
{
    public static class Messages
    {
        public delegate Task MessageHandler(SendableMsg message);
        public static event MessageHandler OnMessageSend;

        public static async Task Send(SendableMsg message)
        {
            OnMessageSend?.Invoke(message);
            return;
        }
    }
}
