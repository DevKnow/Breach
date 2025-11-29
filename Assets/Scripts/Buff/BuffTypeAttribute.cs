using System;

[AttributeUsage(AttributeTargets.Class)]
public class BuffTypeAttribute : Attribute
{
    public BuffKeyword Id { get; }

    public BuffTypeAttribute(BuffKeyword id)
    {
        Id = id;
    }
}
