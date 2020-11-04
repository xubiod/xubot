using ProtoBuf;
using Reddit.Things;
using SteamKit2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using XubotSharedModule;

namespace xubot.src.Modular
{
    public class ModularSystem
    {
        public class ModuleEntry
        {
            public Assembly assemblyToLoad;
            public Assembly assemblyLoaded;
            public AssemblyLoadContext moduleContext;
            public string id;

            public ModuleEntrypoint startInstance;
            public List<CommandModule> commandInstances;

            public ModuleEntry(Assembly assembly, string id)
            {
                this.assemblyToLoad = assembly;
                this.id = id;

                Initialize();
            }

            private void Initialize()
            {
                this.moduleContext = new AssemblyLoadContext(id, true);
                moduleContext.Unloading += ContextUnloaded;

                assemblyLoaded = this.moduleContext.LoadFromAssemblyName(assemblyToLoad.GetName());

                startInstance = assemblyLoaded.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ModuleEntrypoint))).Select(type => { return (ModuleEntrypoint)Activator.CreateInstance(type); }).FirstOrDefault();
                commandInstances = assemblyLoaded.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(CommandModule))).Select(type => { return (CommandModule)Activator.CreateInstance(type); }).ToList();
            }

            public XubotSharedModule.DiscordThings.Message Execute(string command, string[] parameters = null)
            {
                return commandInstances.First(x => x.GetName() == command).Execute(parameters);
            }

            public string Unload()
            {
                string msg = startInstance.Unload().ToString();

                Util.Log.QuickLog("Module unloading: " + id + "\nUnload msg: " + msg);
                moduleContext.Unload();
                startInstance = null;
                commandInstances.Clear();

                return msg;
            }

            private void ContextUnloaded(AssemblyLoadContext obj)
            {
                Util.Log.QuickLog("Module unloaded: " + id);
            }

            public string Reload()
            {
                Initialize();
                string msg = startInstance.Reload().ToString();
                Util.Log.QuickLog("Module reloaded: " + id + "\nReload msg: " + msg);
                return msg;
            }
        }

        public static Dictionary<string, ModuleEntry> modules { get; private set; }

        public static void Initialize()
        {
            modules = new Dictionary<string, ModuleEntry>();
            LoadFromDirectory();
        }

        public static void LoadFromFile(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException("File does not exist");
            if (Path.GetExtension(filename).ToLower() != ".dll") return;

            string name = Path.GetFileNameWithoutExtension(filename).ToLower();
            Assembly newModule = Assembly.LoadFrom(filename);

            modules.Add(name, new ModuleEntry(newModule, name));

            if (modules[name].startInstance == null)
            {
                modules.Remove(name);
                return;
            }

            Util.Log.QuickLog("Module loaded: " + name + "\nLoad msg: " + modules[name].startInstance.Load().ToString());
        }

        public static void LoadFromDirectory(string directory = "/Modules", bool isFull = false)
        {
            foreach (string filename in Directory.GetDirectories((isFull ? "" : Directory.GetCurrentDirectory()) + directory))
                LoadFromDirectory(filename, true);

            foreach (string filename in Directory.GetFiles((isFull ? "" : Directory.GetCurrentDirectory()) + directory))
                LoadFromFile(filename);
        }

        public static void Execute(string id, string method, object[] parameters = null)
        {
            //modules[id].instance.
        }
    }
}
