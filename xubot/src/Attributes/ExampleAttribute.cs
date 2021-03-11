using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ExampleAttribute : Attribute
    {
        public string ExampleParameters { get; }

        public ExampleAttribute()
        {
            this.ExampleParameters = "";
        }

        public ExampleAttribute(string example_params)
        {
            this.ExampleParameters = example_params;
        }

        public string GetExampleUsage()
        {
            return ExampleParameters;
        }
    }
}
