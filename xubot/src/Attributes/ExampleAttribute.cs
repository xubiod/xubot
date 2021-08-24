using System;

namespace xubot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ExampleAttribute : Attribute
    {
        public string ExampleParameters { get; }
        public bool AttachmentNeeded { get; }

        public ExampleAttribute()
        {
            ExampleParameters = "";
            AttachmentNeeded = false;
        }

        public ExampleAttribute(string exampleParams)
        {
            ExampleParameters = exampleParams;
            AttachmentNeeded = false;
        }

        public ExampleAttribute(bool attachmentNeeded)
        {
            ExampleParameters = "";
            AttachmentNeeded = attachmentNeeded;
        }

        public ExampleAttribute(string exampleParams, bool attachmentNeeded)
        {
            ExampleParameters = exampleParams;
            AttachmentNeeded = attachmentNeeded;
        }
    }
}
