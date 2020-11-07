using System;
using System.Collections.Generic;
using System.Text;

namespace XubotSharedModule.DiscordThings
{
    public class SendableMsg
    {
        // modeled after Discord.Net

        public string Text;
        public bool isTTS = false;
        public Embed MsgEmbed = null;
        public string Filepath = null;
        public bool Spoilered = false;

        public SendableMsg(string text, bool isTTS = false, Embed embed = null)
        {
            this.Text = text;
            this.isTTS = isTTS;
            this.MsgEmbed = embed;
        }

        public SendableMsg FileMessage(string filepath, string text = null, bool isTTS = false, Embed embed = null, bool isSpoiler = false)
        {
            this.Filepath = filepath;
            this.Spoilered = isSpoiler;

            this.Text = text;
            this.isTTS = isTTS;
            this.MsgEmbed = embed;

            return this;
        }
    }
}
