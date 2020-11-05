using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule;
using XubotSharedModule.DiscordThings;
using XubotSharedModule.Events;

namespace DebugModule
{
    public class ExampleMultiMsgCommand : CommandModule
    {
        public static string Name = "ExampleM";
        public static string Summary = "ExampleSumm";
        public static string[] Aliases = { "ExampleAlias" };

        public async Task Execute(string[] parameters)
        {
            // new Message("Test success\nParam count " + parameters.Length)
            await Messages.Send(new Message("Test success\nParam count " + parameters.Length));
            await Messages.Send(new Message("New message"));
            await Messages.Send(new Message("Oh wow another message"));
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
