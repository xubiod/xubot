using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CmdSummaryAttribute : Attribute
    {
        public string Summary { get; }

        public CmdSummaryAttribute(string summary)
        {
            this.Summary = summary;
        }

        public string GetSummary()
        {
            return Summary;
        }
    }
}
