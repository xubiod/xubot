using System;
using System.Collections.Generic;
using System.Text;

namespace XubotSharedModule
{
    public interface ModuleEntrypoint
    {
        /// <summary>
        /// Gets called on the first load of the module.
        /// </summary>
        /// <returns>An object that is printed to console.</returns>
        public object Load();

        /// <summary>
        /// Gets called when the module is unloaded dynamically. This is not called on program close.
        /// </summary>
        /// <returns>An object that is printed to console</returns>
        public object Unload();

        /// <summary>
        /// Gets called on subsequent loads of the module (2nd, 3rd, etc.).
        /// </summary>
        /// <returns>An object that is printed to console and to the bot output</returns>
        public object Reload();
    }
}
