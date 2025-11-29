using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_FILE_NAME = "save.json";

    private SaveData _saveData;

    // 기본 해금 프로그램
    private readonly string[] DEFAULT_UNLOCKED_PROGRAMS = { "proc_ghost", "proc_daemon" };

    private string SavePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    private void Awake()
    {
        Load();
    }

    #region Save/Load

    public void Save()
    {
        // SaveData를 JSON 문자열로 변환
        var json = JsonUtility.ToJson(_saveData, true);

        // 파일에 쓰기
        File.WriteAllText(SavePath, json);

        Debug.Log($"[SaveManager] Saved to {SavePath}");
    }

    public void Load()
    {
        // 파일 존재 여부 체크
        if (!File.Exists(SavePath))
        {
            Debug.Log("[SaveManager] No save file found, creating new save");
            CreateNewSave();
            return;
        }

        // 파일에서 JSON 읽기
        var json = File.ReadAllText(SavePath);

        // JSON을 SaveData로 파싱
        _saveData = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("[SaveManager] Loaded save file");
    }

    private void CreateNewSave()
    {
        _saveData = new SaveData();

        // 기본 프로그램 해금
        foreach (var programId in DEFAULT_UNLOCKED_PROGRAMS)
        {
            _saveData.unlockedPrograms.Add(programId);
        }

        Save();
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("[SaveManager] Save file deleted");
        }

        CreateNewSave();
    }

    #endregion

    #region Program Unlock

    public bool IsProgramUnlocked(string programId)
    {
        return _saveData.unlockedPrograms.Contains(programId);
    }

    public bool IsProgramDiscovered(string programId)
    {
        return _saveData.discoveredPrograms.Contains(programId);
    }

    public void DiscoverProgram(string programId)
    {
        if (_saveData.discoveredPrograms.Contains(programId))
            return;

        _saveData.discoveredPrograms.Add(programId);
        Save();
    }

    public void UnlockProgram(string programId)
    {
        if (_saveData.unlockedPrograms.Contains(programId))
            return;

        _saveData.unlockedPrograms.Add(programId);
        Save();
    }

    public List<string> GetUnlockedPrograms()
    {
        return _saveData.unlockedPrograms;
    }

    public List<string> GetDiscoveredPrograms()
    {
        return _saveData.discoveredPrograms;
    }

    #endregion

    #region Module Unlock

    public bool IsModuleUnlocked(string moduleId)
    {
        return _saveData.unlockedModules.Contains(moduleId);
    }

    public bool IsModuleDiscovered(string moduleId)
    {
        return _saveData.discoveredModules.Contains(moduleId);
    }

    public void DiscoverModule(string moduleId)
    {
        if (_saveData.discoveredModules.Contains(moduleId))
            return;

        _saveData.discoveredModules.Add(moduleId);
        Save();
    }

    public void UnlockModule(string moduleId)
    {
        if (_saveData.unlockedModules.Contains(moduleId))
            return;

        _saveData.unlockedModules.Add(moduleId);
        Save();
    }

    public List<string> GetUnlockedModules()
    {
        return _saveData.unlockedModules;
    }

    public List<string> GetDiscoveredModules()
    {
        return _saveData.discoveredModules;
    }

    #endregion

    #region Cache (해금용 재화)

    public int GetCache()
    {
        return _saveData.cache;
    }

    public void AddCache(int amount)
    {
        _saveData.cache += amount;
        Save();
    }

    public bool SpendCache(int amount)
    {
        if (_saveData.cache < amount)
            return false;

        _saveData.cache -= amount;
        Save();
        return true;
    }

    #endregion

    #region Run History

    public void AddRunRecord(RunRecord record)
    {
        _saveData.runHistory.Add(record);
        Save();
    }

    public List<RunRecord> GetRunHistory()
    {
        return _saveData.runHistory;
    }

    #endregion
}
