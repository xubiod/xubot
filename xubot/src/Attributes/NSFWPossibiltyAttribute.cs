using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NSFWPossibiltyAttribute : Attribute
    {
        public string Warnings { get; }

        public NSFWPossibiltyAttribute()
        {
            this.Warnings = "Unspecified";
        }

        public NSFWPossibiltyAttribute(string warnings)
        {
            this.Warnings = warnings;
        }

        public string GetWarnings()
        {
            return Warnings;
        }
    }
}
