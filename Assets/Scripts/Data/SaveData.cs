using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // 해금
    public List<string> unlockedPrograms = new();
    public List<string> unlockedCommands = new();

    // 발견 (소스코드 획득, 해금 가능)
    public List<string> discoveredPrograms = new();
    public List<string> discoveredCommands = new();

    // 해금용 재화
    public int cache;

    // 런 기록
    public List<RunRecord> runHistory = new();
}

[Serializable]
public class RunRecord
{
    public string programId;
    public int reachedSector;
    public bool cleared;
    public int earnedChips;
    public List<BattleRecord> battles = new();
}

[Serializable]
public class BattleRecord
{
    public int battleId;
    public int sector;
    public int clearedRound;  // -1이면 실패
}
