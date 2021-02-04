using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace XubotSharedModule.DiscordThings
{
    public class Embed
    {
        // modeled after Discord.Net

        public string Title;
        public string Description;
        public string ThumbnailURL;
        public Color Color;
        public List<DiscordThings.EmbedField> Fields;
    }
}
