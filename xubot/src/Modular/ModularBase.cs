using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace xubot.src.Modular
{
    public class ModularBase
    {
        public static Dictionary<string, Assembly> modules { get; private set; }

        public static void Initialize()
        {
            modules = new Dictionary<string, Assembly>();
            LoadFromDirectory();
        }

        public static void LoadFromFile(string filename)
        {
            ///TODO actually make this
        }

        public static void LoadFromDirectory(string directory = "/Modules")
        {
            foreach (string filename in Directory.GetFiles(Directory.GetCurrentDirectory() + directory))
                LoadFromFile(filename);
        }

        public static object Execute(string id, string method, object[] parameters = null)
        {
            /// TODO shit
            return null;
        }
    }
}
