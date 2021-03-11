using System;
using System.Collections.Generic;
using System.Text;

namespace xubot.src.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ExampleAttribute : Attribute
    {
        public string ExampleParameters { get; }
        public bool AttachmentNeeded { get; }

        public ExampleAttribute()
        {
            this.ExampleParameters = "";
            this.AttachmentNeeded = false;
        }

        public ExampleAttribute(string example_params)
        {
            this.ExampleParameters = example_params;
            this.AttachmentNeeded = false;
        }

        public ExampleAttribute(bool attachment_needed)
        {
            this.ExampleParameters = "";
            this.AttachmentNeeded = attachment_needed;
        }

        public ExampleAttribute(string example_params, bool attachment_needed)
        {
            this.ExampleParameters = example_params;
            this.AttachmentNeeded = attachment_needed;
        }
    }
}
