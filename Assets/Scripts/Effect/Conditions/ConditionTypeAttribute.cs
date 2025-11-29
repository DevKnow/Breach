using System;

[AttributeUsage(AttributeTargets.Class)]
public class ConditionTypeAttribute : Attribute
{
    public string TypeName { get; }

    public ConditionTypeAttribute(string typeName)
    {
        TypeName = typeName;
    }
}
