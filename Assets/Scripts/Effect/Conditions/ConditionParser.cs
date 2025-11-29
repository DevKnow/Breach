using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ConditionParser
{
    private static Dictionary<string, Type> _conditionTypes;

    public static void Initialize()
    {
        _conditionTypes = new Dictionary<string, Type>();

        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<ConditionTypeAttribute>();
            if (attr != null)
            {
                if (!_conditionTypes.TryAdd(attr.TypeName, type))
                {
                    Debug.LogError($"ConditionType '{attr.TypeName}' already registered");
                }
            }
        }
    }

    public static EffectCondition Parse(string conditionString)
    {
        if (string.IsNullOrEmpty(conditionString))
            return null;

        if (_conditionTypes == null)
        {
            Initialize();
        }

        int operatorIndex = CompareHelper.FindOperatorIndex(conditionString);
        if (operatorIndex <= 0)
        {
            Debug.LogError($"Invalid condition format: {conditionString}");
            return null;
        }

        string typeName = conditionString.Substring(0, operatorIndex);
        if (!_conditionTypes.TryGetValue(typeName, out Type conditionType))
        {
            Debug.LogError($"Unknown condition type: {typeName}");
            return null;
        }

        var condition = (EffectCondition)Activator.CreateInstance(conditionType);
        condition.Parse(conditionString.Substring(operatorIndex));
        return condition;
    }
}
