using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule;
using XubotSharedModule.DiscordThings;
using XubotSharedModule.Events;

namespace DebugModule
{
    class ExampleEmbed : CommandModule
    {
        public static string Name = "Embed";
        public static string Summary = "Test embed";
        public static string[] Aliases = { "ExampleAlias" };

        public async Task Execute(string[] parameters)
        {
            Message msg = new Message("", false, new Embed() {
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

        public string GetName()
        {
            return Name;
        }

        public string GetSummary()
        {
            return Summary;
        }

        public string[] GetAliases()
        {
            return Aliases;
        }
    }
}
