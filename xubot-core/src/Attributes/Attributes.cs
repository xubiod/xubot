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

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NSFWPossibiltyAttribute : Attribute
    {
        public string warnings { get; }

        public NSFWPossibiltyAttribute()
        {
            this.warnings = "Unspecified";
        }

        public NSFWPossibiltyAttribute(string warnings)
        {
            this.warnings = warnings;
        }

        public string GetWarnings()
        {
            return warnings;
        }
    }
}
