using System;
using System.Collections.Generic;
using System.Text;
using XubotSharedModule.DiscordThings;

namespace XubotSharedModule
{
    public interface CommandModule
    {
        public string GetName();
        public string GetSummary();
        public string[] GetAliases();
        public Message Execute(string[] parameters);
    }
}
