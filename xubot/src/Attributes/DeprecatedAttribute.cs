using System;

namespace xubot.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DeprecatedAttribute : Attribute
    {
    }
}
