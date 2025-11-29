using System;

[AttributeUsage(AttributeTargets.Class)]
public class EffectTypeAttribute : Attribute
{
    public string TypeName { get; }

    public EffectTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
