using System;
using XubotSharedModule;

namespace DebugModule
{
    public class Entrypoint : StartModule
    {
        public object Load()
        {
            return "Success on load";
        }
    }
}
