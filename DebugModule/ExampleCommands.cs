using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xubot.src.Attributes;
using XubotSharedModule;
using XubotSharedModule.DiscordThings;
using XubotSharedModule.Events;

namespace DebugModule
{
    public class ExampleCommands : ICommandModule
    {
        [CmdName("example")]
        public async Task SingleMessage(string[] parameters)
        {
            // new Message("Test success\nParam count " + parameters.Length)
            await Messages.Send(new SendableMsg("Test success\nParam count " + parameters.Length));
        }

        [CmdName("examplem")]
        public async Task MultipleMessages(string[] parameters)
        {
            // new Message("Test success\nParam count " + parameters.Length)
            await Messages.Send(new SendableMsg("Test success\nParam count " + parameters.Length));
            await Messages.Send(new SendableMsg("New message"));
            await Messages.Send(new SendableMsg("Oh wow another message"));
        }

        [CmdName("embed")]
        public async Task Embed(string[] parameters)
        {
            SendableMsg msg = new SendableMsg("", false, new Embed()
            {
                Title = "Embed title",
                Description = "Embed description",
                Fields = new List<EmbedField>()
                {
                    new EmbedField()
                    {
                        Name = "Param count",
                        Value = parameters.Length.ToString(),
                        IsInline = true
                    },

                    new EmbedField()
                    {
                        Name = "Other",
                        Value = "More test stuff",
                        IsInline = true
                    },

                    new EmbedField()
                    {
                        Name = "Other",
                        Value = "New test stuff",
                        IsInline = true
                    }
                }
            });

            await Messages.Send(msg);
        }

        [CmdName("request")]
        public async Task Request(string[] parameters)
        {
            string msg = "";

            msg += (string)Requestor.Request(Requestor.RequestType.User, Requestor.RequestProperty.Name) + "#";
            msg += (string)Requestor.Request(Requestor.RequestType.User, Requestor.RequestProperty.Discrim);
            msg += " (" + ((ulong)Requestor.Request(Requestor.RequestType.User, Requestor.RequestProperty.ID)).ToString() + ")";

            await Messages.Send(new SendableMsg(msg));
        }
    }
}
