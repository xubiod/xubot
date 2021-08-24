using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NsfwPossibiltyAttribute : Attribute
    {
        public string Warnings { get; }

        public NsfwPossibiltyAttribute()
        {
            this.Warnings = "Unspecified";
        }

        public NsfwPossibiltyAttribute(string warnings)
        {
            this.Warnings = warnings;
        }

        public string GetWarnings()
        {
            return Warnings;
        }
    }
}
