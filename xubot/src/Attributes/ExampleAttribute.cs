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

        public ExampleAttribute(string exampleParams)
        {
            this.ExampleParameters = exampleParams;
            this.AttachmentNeeded = false;
        }

        public ExampleAttribute(bool attachmentNeeded)
        {
            this.ExampleParameters = "";
            this.AttachmentNeeded = attachmentNeeded;
        }

        public ExampleAttribute(string exampleParams, bool attachmentNeeded)
        {
            this.ExampleParameters = exampleParams;
            this.AttachmentNeeded = attachmentNeeded;
        }
    }
}
