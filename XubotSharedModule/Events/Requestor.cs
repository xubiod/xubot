using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule.DiscordThings;

namespace XubotSharedModule.Events
{
    public static class Requestor
    {
        public enum RequestType
        {
            Guild, Channel, User, Client, Message, Self
        }

        public enum RequestProperty
        {
            Name, ID, Discrim, Content
        }

        public delegate object RequestHandler(RequestType what, RequestProperty want);
        public static event RequestHandler OnRequest;

        public static object Request(RequestType what, RequestProperty want)
        {
            object r;
            r = OnRequest?.Invoke(what, want);
            return r;
        }
    }
}
