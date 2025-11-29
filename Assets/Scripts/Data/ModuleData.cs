using System;

[Serializable]
public class ModuleData
{
    public string id;
    public string nameKo;
    public string nameEn;
    public ModuleType type;
    public int integrityBonus;
    public int payloadBonus;
    public string[] commands;
    public Rarity rarity;
}
