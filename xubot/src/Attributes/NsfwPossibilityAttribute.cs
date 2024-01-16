using System;

namespace xubot.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class NsfwPossibilityAttribute : Attribute
{
    public string Warnings { get; }

    public NsfwPossibilityAttribute()
    {
        Warnings = "Unspecified";
    }

    public NsfwPossibilityAttribute(string warnings)
    {
        Warnings = warnings;
    }
}