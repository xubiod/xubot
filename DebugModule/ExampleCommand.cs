using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule;
using XubotSharedModule.DiscordThings;
using XubotSharedModule.Events;

namespace DebugModule
{
    public class ExampleCommand : ICommandModule
    {
        public static string Name = "Example";
        public static string Summary = "ExampleSumm";
        public static string[] Aliases = { "ExampleAlias" };

        public async Task Execute(string[] parameters)
        {
            // new Message("Test success\nParam count " + parameters.Length)
            await Messages.Send(new Message("Test success\nParam count " + parameters.Length));
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
