using Discord.Commands;
using NLua;
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
using System.Threading.Tasks;
using xubot.src.Attributes;
using XubotSharedModule;

namespace xubot.src.Modular
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
                this.assemblyFileName = assemblyFilename;
                this.id = id;

                Initialize();
            }

            private void Initialize()
            {
                this.moduleContext = new AssemblyLoadContext(id, true);
                moduleContext.Unloading += ContextUnloaded;

                assembly = this.moduleContext.LoadFromAssemblyPath(this.assemblyFileName);

                startInstance = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IModuleEntrypoint))).Select(type => { return (IModuleEntrypoint)Activator.CreateInstance(type); }).FirstOrDefault();
                commandInstances = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ICommandModule))).Select(type => { return (ICommandModule)Activator.CreateInstance(type); }).ToList();

                XubotSharedModule.Events.Messages.OnMessageSend += SendModuleMessage; Util.Log.QuickLog("Sub.MsgSend");
                XubotSharedModule.Events.Requestor.OnRequest += SendRequestedThing; Util.Log.QuickLog("Sub.OnReq");
            }

            private object SendRequestedThing(XubotSharedModule.Events.Requestor.RequestType what, XubotSharedModule.Events.Requestor.RequestProperty want)
            {
                return Requestee.Get(context, what, want);
            }

            private Task SendModuleMessage(XubotSharedModule.DiscordThings.SendableMsg message)
            {
                if (this.context == null) return Task.CompletedTask;
                return ModularUtil.SendMessage(this.context, message);
            }

            public void Execute(ICommandContext context, string command, string[] parameters = null)
            {
                this.context = context;

                var temp = commandInstances.First(x => x.GetType().GetMethods().Where(x => (x.GetCustomAttribute<CmdNameAttribute>() ?? new CmdNameAttribute("")).Name == command).Count() > 0);
                temp.GetType().GetMethods().Where(x => (x.GetCustomAttributes<CmdNameAttribute>().First().Name == command)).First().Invoke(temp, new object[] { parameters }); //.Where(x => x is CmdNameAttribute).First() as CmdNameAttribute).name == command) //.GetMethods("Execute").Invoke(temp, new object[]{ parameters });
            }

            public string Unload()
            {
                string msg = (startInstance != null) ? (startInstance.Unload() ?? "No unload message").ToString() : "No startInstance";

                Util.Log.QuickLog("Module unloading: " + id + "\nUnload msg: " + msg);

                XubotSharedModule.Events.Messages.OnMessageSend -= SendModuleMessage; Util.Log.QuickLog("Unsub.MsgSend");
                XubotSharedModule.Events.Requestor.OnRequest -= SendRequestedThing; Util.Log.QuickLog("Unsub.OnReq");

                moduleContext.Unload();
                startInstance = null;
                commandInstances.Clear();
                commandInstances = null;
                assembly = null;
                context = null;

                return msg;
            }

            private void ContextUnloaded(AssemblyLoadContext obj)
            {
                Util.Log.QuickLog("Module unloaded: " + id);
                moduleContext = null;
            }

            public string Reload()
            {
                if (startInstance != null) Unload();

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
            //Assembly newModule = Assembly.LoadFrom(filename);

            modules.Add(name, new ModuleEntry(filename, name));

            if (modules[name].startInstance == null)
            {
                modules[name].Unload();
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

        public static async Task Execute(ICommandContext context, string module, string command, string[] parameters = null)
        {
            Modular.ModularSystem.modules[module].Execute(context, command, parameters);
        }
    }
}
