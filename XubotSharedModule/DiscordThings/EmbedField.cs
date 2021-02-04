using System;
using System.Collections.Generic;
using System.Text;

namespace XubotSharedModule.DiscordThings
{
    public class EmbedField
    {
        // modeled after Discord.Net

        public string Name;
        public string Value;
        public bool IsInline = false;
    }
}
