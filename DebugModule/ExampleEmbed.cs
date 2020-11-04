using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XubotSharedModule;
using XubotSharedModule.DiscordThings;

namespace DebugModule
{
    class ExampleEmbed : CommandModule
    {
        public static string Name = "Embed";
        public static string Summary = "Test embed";
        public static string[] Aliases = { "ExampleAlias" };

        public Message Execute(string[] parameters)
        {
            return new Message("", false, new Embed() {
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
                    }
                }
            });
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
