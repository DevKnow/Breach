using System;

[Serializable]
public class CommandData
{
    public string id;
    public string nameKo = string.Empty;
    public string nameEn = string.Empty;
    public CommandType type = CommandType.None;
    public int minRange;
    public int maxRange;
    public int clockCost;
    public int penModifier;
    public int payload;
    public float criticalMultiplier;
    public string[] effects;

    public string GetName()
    {
        return SettingManager.Instance.Language switch
        {
            LANGUAGE.Ko => nameKo,
            LANGUAGE.En => nameEn,
            _ => nameKo,
        };
    }

    public string GetClock()
    {
        return clockCost.ToString();
    }

    public string GetRange()
    {
        return $"{minRange}-{maxRange}";
    }

    public string GetPenetrationBonus()
    {
        return penModifier.ToString();
    }

    public string GetPayload()
    {
        return payload.ToString();
    }

    public string GetCriticalMultiplier()
    {
        return $"x{criticalMultiplier}";
    }

    public string[] GetEffects()
    {
        return effects;
    }
}
