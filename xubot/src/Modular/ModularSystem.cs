using ProtoBuf;
using Reddit.Things;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using XubotSharedModule;

namespace xubot.src.Modular
{
    public class ModularSystem
    {
        public class ModuleEntry
        {
            public StartModule instance;

            public ModuleEntry(Assembly assembly)
            {
                instance = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(StartModule))).Select(type => { return (StartModule)Activator.CreateInstance(type); }).FirstOrDefault();
            }

            public void Reload()
            {
                /// TODO unfuck
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

            modules.Add(name, new ModuleEntry(newModule));

            if (modules[name].instance == null)
            {
                modules.Remove(name);
                return;
            }

            Util.Log.QuickLog("Module loaded: " + name + "\nLoad msg: " + modules[name].instance.Load().ToString());
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
