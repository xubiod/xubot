using System;
using System.Collections.Generic;
using System.Text;
using XubotSharedModule;
using XubotSharedModule.DiscordThings;

namespace DebugModule
{
    public class ExampleCommand : CommandModule
    {
        public static string Name = "Example";
        public static string Summary = "ExampleSumm";
        public static string[] Aliases = { "ExampleAlias" };

        public Message Execute(string[] parameters)
        {
            return new Message("Test success\nParam count " + parameters.Length);
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
