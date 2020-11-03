using System;
using XubotSharedModule;

namespace DebugModule
{
    public class Entrypoint : BotModule
    {
        public object Load()
        {
            return "Success on load";
        }
    }
}
