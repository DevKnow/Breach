using System;
using System.Collections.Generic;

[Serializable]
public class EnemyData : ProgramData
{
    public List<EnemyModuleEntry> modules;
}

[Serializable]
public class EnemyModuleEntry
{
    public string moduleId;
    public string[] patches;
}
