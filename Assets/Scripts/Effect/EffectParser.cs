using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class EffectParser
{
    private static Dictionary<string, Type> _effectTypes;

    public static void Initialize()
    {
        _effectTypes = new Dictionary<string, Type>();

        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<EffectTypeAttribute>();
            if (attr != null)
            {
                if (!_effectTypes.TryAdd(attr.TypeName, type))
                {
                    Debug.LogError($"EffectType '{attr.TypeName}' already registered");
                }
            }
        }
    }

    public static EffectAbility Parse(string effectString)
    {
        if (_effectTypes == null)
        {
            Initialize();
        }

        var parts = effectString.Split(':');
        var typeName = parts[0];

        if (_effectTypes.TryGetValue(typeName, out var type))
        {
            var effect = (EffectAbility)Activator.CreateInstance(type);
            effect.Parse(parts);
            return effect;
        }

        Debug.LogError($"Unknown effect type: {typeName}");
        return null;
    }
}
