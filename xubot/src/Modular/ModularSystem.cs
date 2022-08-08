using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Discord.Commands;
using xubot.src.Attributes;
using XubotSharedModule;
using XubotSharedModule.DiscordThings;
using XubotSharedModule.Events;

namespace xubot.Modular
{
    public class ModularSystem
    {
        public class ModuleEntry
        {
            public string assemblyFileName;
            public Assembly assembly;
            public AssemblyLoadContext moduleContext;
            public string id;

            public IModuleEntrypoint startInstance;
            public List<ICommandModule> commandInstances;

            public ICommandContext context;

            public ModuleEntry(string assemblyFilename, string id)
            {
                assemblyFileName = assemblyFilename;
                this.id = id;

                Initialize();
            }

            private async void Initialize()
            {
                moduleContext = new AssemblyLoadContext(id, true);
                moduleContext.Unloading += ContextUnloaded;

                assembly = moduleContext.LoadFromAssemblyPath(assemblyFileName);

                startInstance = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IModuleEntrypoint))).Select(type => (IModuleEntrypoint)Activator.CreateInstance(type)).FirstOrDefault();
                commandInstances = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ICommandModule))).Select(type => (ICommandModule)Activator.CreateInstance(type)).ToList();

                Messages.OnMessageSend += SendModuleMessage; await Util.Log.QuickLog("Sub.MsgSend");
                Requestor.OnRequest += SendRequestedThing; await Util.Log.QuickLog("Sub.OnReq");
            }

            private object SendRequestedThing(Requestor.RequestType what, Requestor.RequestProperty want)
            {
                return Requestee.Get(context, what, want);
            }

            private Task SendModuleMessage(SendableMsg message)
            {
                if (context == null) return Task.CompletedTask;
                return ModularUtil.SendMessage(context, message);
            }

            public void Execute(ICommandContext context, string command, string[] parameters = null)
            {
                this.context = context;

                var temp = commandInstances.First(x => x.GetType().GetMethods().Count(x => (x.GetCustomAttribute<CmdNameAttribute>() ?? new CmdNameAttribute("")).Name == command) > 0);
                temp.GetType().GetMethods().First(x => x.GetCustomAttributes<CmdNameAttribute>().First().Name == command).Invoke(temp, new object[] { parameters }); //.Where(x => x is CmdNameAttribute).First() as CmdNameAttribute).name == command) //.GetMethods("Execute").Invoke(temp, new object[]{ parameters });
            }

            public async Task<string> Unload()
            {
                string msg = startInstance != null ? (startInstance.Unload() ?? "No unload message").ToString() : "No startInstance";

                await Util.Log.QuickLog($"Module unloading: {id}\nUnload msg: {msg}");

                Messages.OnMessageSend -= SendModuleMessage; await Util.Log.QuickLog("Unsub.MsgSend");
                Requestor.OnRequest -= SendRequestedThing; await Util.Log.QuickLog("Unsub.OnReq");

                moduleContext.Unload();
                startInstance = null;
                commandInstances.Clear();
                commandInstances = null;
                assembly = null;
                context = null;

                return msg;
            }

            private async void ContextUnloaded(AssemblyLoadContext obj)
            {
                await Util.Log.QuickLog($"Module unloaded: {id}");
                moduleContext = null;
            }

            public async Task<string> Reload()
            {
                if (startInstance != null) await Unload();

                Initialize();
                string msg = startInstance.Reload().ToString();
                await Util.Log.QuickLog($"Module reloaded: {id}\nReload msg: {msg}");
                return msg;
            }
        }

        public static Dictionary<string, ModuleEntry> Modules { get; private set; }

        public static void Initialize()
        {
            Modules = new Dictionary<string, ModuleEntry>();
            LoadFromDirectory();
        }

        public static async void LoadFromFile(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException("File does not exist");
            if (Path.GetExtension(filename).ToLower() != ".dll") return;

            string name = Path.GetFileNameWithoutExtension(filename).ToLower();
            //Assembly newModule = Assembly.LoadFrom(filename);

            Modules.Add(name, new ModuleEntry(filename, name));

            if (Modules[name].startInstance == null)
            {
                await Modules[name].Unload();
                Modules.Remove(name);
                return;
            }

            await Util.Log.QuickLog($"Module loaded: {name}\nLoad msg: {Modules[name].startInstance.Load()}");
        }

        public static async void LoadFromDirectory(string directory = "/Modules", bool isFull = false)
        {
            if (!Directory.Exists((isFull ? "" : Directory.GetCurrentDirectory()) + directory))
            {
                await Util.Log.QuickLog("there's no modules, skipping");
                return;
            }

            foreach (string filename in Directory.GetDirectories((isFull ? "" : Directory.GetCurrentDirectory()) + directory))
                LoadFromDirectory(filename, true);

            foreach (string filename in Directory.GetFiles((isFull ? "" : Directory.GetCurrentDirectory()) + directory))
                LoadFromFile(filename);
        }

        public static void Execute(ICommandContext context, string module, string command, string[] parameters = null)
        {
            Modules[module].Execute(context, command, parameters);
        }
    }
}
