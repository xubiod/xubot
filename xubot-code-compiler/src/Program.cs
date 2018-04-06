using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jurassic;
using Newtonsoft.Json;
using NLua;

namespace xubot_code_compiler
{
    class Program
    {
        private static string TableToString(LuaTable t)
        {
            object[] keys = new object[t.Keys.Count];
            object[] values = new object[t.Values.Count];
            t.Keys.CopyTo(keys, 0);
            t.Values.CopyTo(values, 0);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < keys.Count(); i++)
            {
                builder.AppendLine($"{keys[i]} = {values[i]}");
            }

            return builder.ToString();
        }

        public static ScriptEngine jsEngine = new ScriptEngine();

        public static string _result = "";

        static void Main(string[] args)
        {
            if (args[0] == "lua")
            {
                string code = "";
                foreach (string arg in args)
                {
                    if (arg != "lua")
                    {
                        code = code + " " + arg;
                    }
                }

                if (code.Contains("luanet")) { Environment.Exit(0); }

                //thanks wamwoowam!!!
                using (Lua lua = new Lua())
                {
                    try
                    {
                        lua.DoString(@"os.execute = nil
                                        os.rename = nil
                                        os.remove = nil
                                        os.exit = nil
                                        io = nil
                                        import = nil
                                        require = nil");
                        var ret = lua.DoString(code, "eval")?
                            .Select(o => o is LuaTable ta ? TableToString(ta) : o);
                        _result = JsonConvert.SerializeObject(ret);
                    }
                    catch (Exception ex)
                    {
                        _result = ex.Message;
                    }
                }

                if (_result.Contains("null"))
                {
                    _result = "Code has null in it.";
                }

                string uri = Path.GetTempPath() + "InterpResult.xubot";
                if (File.Exists(uri)) File.Delete(uri);
                File.WriteAllText(uri, _result);

                Environment.Exit(0);
            }

            else if (args[0] == "js")
            {
                string code = "";
                foreach (string arg in args)
                {
                    if (arg != "js")
                    {
                        code = code + " " + arg;
                    }
                }
                
                try
                {
                    _result = jsEngine.Evaluate(code).ToString();

                    if (_result.Contains("null"))
                    {
                        _result = "Code has null in it.";
                    }
                }
                catch (Exception ex)
                {
                    _result = ex.Message;
                }

                string uri = Path.GetTempPath() + "InterpResult.xubot";
                if (File.Exists(uri)) File.Delete(uri);
                File.WriteAllText(uri, _result);

                Environment.Exit(0);
            }
        }
    }
}
