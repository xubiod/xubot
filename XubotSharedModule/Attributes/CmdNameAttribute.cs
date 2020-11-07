using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CmdNameAttribute : Attribute
    {
        public string Name { get; }

        public CmdNameAttribute(string name)
        {
            this.Name = name;
        }

        public string GetName()
        {
            return Name;
        }
    }
}
