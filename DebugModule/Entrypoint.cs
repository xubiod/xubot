﻿using System;
using XubotSharedModule;

namespace DebugModule
{
    public class Entrypoint : IModuleEntrypoint
    {
        public object Load()
        {
            return "Success on load";
        }

        public object Unload()
        {
            return "Success on unload";
        }

        public object Reload()
        {
            return "Success on reload";
        }
    }
}
