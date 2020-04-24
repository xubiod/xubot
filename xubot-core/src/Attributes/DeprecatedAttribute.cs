using SteamKit2.Unified.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace xubot_core.src.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DeprecatedAttribute : Attribute
    {
        public DeprecatedAttribute() {}
    }
}
