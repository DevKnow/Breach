using System;
using System.Collections.Generic;

[Serializable]
public class EnemyData : ProgramData
{
    public List<EnemyModuleEntry> commands;
}

[Serializable]
public class EnemyModuleEntry
{
    public string commandId;
    public string[] patches;
}
