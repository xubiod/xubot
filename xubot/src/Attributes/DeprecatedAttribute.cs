using System;

namespace xubot.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DeprecatedAttribute : Attribute
    {
        public DeprecatedAttribute() {}
    }
}
