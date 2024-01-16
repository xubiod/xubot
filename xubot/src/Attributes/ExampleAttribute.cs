using System;

namespace xubot.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class ExampleAttribute(string exampleParams, bool attachmentNeeded) : Attribute
{
    public string ExampleParameters { get; } = exampleParams;
    public bool AttachmentNeeded { get; } = attachmentNeeded;

    public ExampleAttribute(string exampleParams = "") : this(exampleParams, false)
    {
    }

    public ExampleAttribute(bool attachmentNeeded) : this("", attachmentNeeded)
    {
    }
}