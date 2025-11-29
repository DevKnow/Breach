using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class BuffFactory
{
    private static Dictionary<BuffKeyword, Type> _buffTypes;
    private static Dictionary<string, BuffKeyword> _buffKeywords;

    public static void Initialize()
    {
        _buffTypes = new Dictionary<BuffKeyword, Type>();
        _buffKeywords = new Dictionary<string, BuffKeyword>();

        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<BuffTypeAttribute>();
            if (attr != null)
            {
                if (!_buffTypes.TryAdd(attr.Id, type))
                {
                    Debug.LogError($"BuffType '{attr.Id}' already registered");
                }
            }
        }
    }

    public static BuffModel Create(BuffData data)
    {
        if (_buffTypes == null)
        {
            Initialize();
        }

        if (!_buffTypes.TryGetValue(data.id, out Type buffType))
        {
            Debug.LogError($"Unknown buff type: {data.id}");
            return null;
        }

        var buff = (BuffModel)Activator.CreateInstance(buffType);
        buff.Initialize(data);
        return buff;
    }

    public static BuffKeyword FindKeyword(string buffId)
    {
        if (_buffKeywords == null)
        {
            Initialize();
        }

        if (_buffKeywords.TryGetValue(buffId, out var keyword))
        {
            return keyword;
        }

        if(System.Enum.TryParse(buffId, false, out keyword))
        {
            _buffKeywords.TryAdd(buffId, keyword);
            return keyword;
        }

        return BuffKeyword.Error;
    }
}
