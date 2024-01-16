using System;

namespace xubot.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class NsfwPossibilityAttribute(string warnings = "Unspecified") : Attribute
{
    public string Warnings { get; } = warnings;
}