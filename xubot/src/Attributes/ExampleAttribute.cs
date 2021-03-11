using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ExampleAttribute : Attribute
    {
        public string ExampleCmdUsage { get; }

        public ExampleAttribute(string examplecmd)
        {
            this.ExampleCmdUsage = examplecmd;
        }

        public string GetExampleUsage()
        {
            return ExampleCmdUsage;
        }
    }
}
