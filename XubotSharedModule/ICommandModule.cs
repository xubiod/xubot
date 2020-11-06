using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XubotSharedModule.DiscordThings;

namespace XubotSharedModule
{
    public interface ICommandModule
    {
        /// <summary>
        /// Returns the name of the command, used to call it through Discord.
        /// </summary>
        /// <returns>The command name as a string</returns>
        public string GetName();

        /// <summary>
        /// Returns the summary of the command, used to describe it in help functions.
        /// </summary>
        /// <returns>The summary of the command as a string</returns>
        public string GetSummary();

        /// <summary>
        /// Gets the aliases of the command, used as shorthands.
        /// </summary>
        /// <returns>The aliases of the command as a single dimension array of strings</returns>
        public string[] GetAliases();

        /// <summary>
        /// Gets called when the command is executed.
        /// </summary>
        /// <param name="parameters">A single dimensional array of strings</param>
        /// <returns>A Message object the bot uses to send a messages</returns>
        public Task Execute(string[] parameters);
    }
}
