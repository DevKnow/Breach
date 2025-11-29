using System;

[Serializable]
public class CommandData
{
    public string id;
    public string nameKo;
    public string nameEn;
    public CommandType type;
    public int minRange;
    public int maxRange;
    public int clockCost;
    public int penModifier;
    public int payload;
    public float criticalMultiplier;
    public string[] effects;
}
