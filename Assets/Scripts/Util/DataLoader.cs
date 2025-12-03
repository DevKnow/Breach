using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class DataLoader
{
    private static Dictionary<string, ProgramData> _programData;
    private static Dictionary<string, CommandData> _commandData;
    private static Dictionary<string, PatchData> _patchData;
    private static Dictionary<string, EnemyData> _enemyData;
    private static Dictionary<BuffKeyword, BuffData> _buffData;
    private static Dictionary<int, PoolData> _poolData;
    private static Dictionary<int, BattleData> _battleData;

    public static void LoadAll()
    {
        _programData = LoadData<ProgramData>("Data/ProgramData");
        _commandData = LoadData<CommandData>("Data/CommandData");
        _patchData = LoadData<PatchData>("Data/PatchData");
        _enemyData = LoadEnemyData("Data/EnemyData");
        _buffData = LoadBuffData("Data/BuffData");
        _poolData = LoadPoolData("Data/PoolData");
        _battleData = LoadBattleData("Data/BattleData");
    }

    private static Dictionary<string, T> LoadData<T>(string path) where T : new()
    {
        var dict = new Dictionary<string, T>();
        var csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"{path}.csv not found in Resources/");
            return dict;
        }

        var lines = csv.text.Split('\n');
        if (lines.Length < 2)
            return dict;

        var headers = lines[0].Split(',');
        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = headers[i].Trim();
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var values = lines[i].Split(',');
            if (values.Length != headers.Length)
                continue;

            var data = new T();

            for (int j = 0; j < headers.Length; j++)
            {
                // _로 시작하는 컬럼은 스킵 (주석용)
                if (headers[j].StartsWith("_"))
                    continue;

                var field = typeof(T).GetField(headers[j]);
                if (field == null)
                    continue;

                var value = values[j].Trim();
                SetFieldValue(data, field, value);
            }

            var idField = typeof(T).GetField("id");
            if (idField != null)
            {
                var id = (string)idField.GetValue(data);
                if (!dict.TryAdd(id, data))
                {
                    Debug.LogError($"{typeof(T).Name} Registration Error: Already registered ID {id}");
                }
            }
        }

        return dict;
    }

    private static Dictionary<string, EnemyData> LoadEnemyData(string path)
    {
        var dict = new Dictionary<string, EnemyData>();
        var csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"{path}.csv not found in Resources/");
            return dict;
        }

        var lines = csv.text.Split('\n');
        if (lines.Length < 2)
            return dict;

        var headers = lines[0].Split(',');
        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = headers[i].Trim();
        }

        var idIdx = Array.IndexOf(headers, "id");
        var nameKoIdx = Array.IndexOf(headers, "nameKo");
        var nameEnIdx = Array.IndexOf(headers, "nameEn");
        var integrityIdx = Array.IndexOf(headers, "defaultMaxIntegrity");
        var payloadIdx = Array.IndexOf(headers, "payloadBonus");
        var commandIdx = Array.IndexOf(headers, "commands");
        var patchesIdx = Array.IndexOf(headers, "patches");

        EnemyData currentEnemy = null;

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var values = lines[i].Split(',');
            if (values.Length != headers.Length)
                continue;

            var id = values[idIdx].Trim();

            if (!string.IsNullOrEmpty(id))
            {
                currentEnemy = new EnemyData
                {
                    id = id,
                    nameKo = values[nameKoIdx].Trim(),
                    nameEn = values[nameEnIdx].Trim(),
                    defaultMaxIntegrity = int.TryParse(values[integrityIdx].Trim(), out var integrity) ? integrity : 0,
                    payloadBonus = int.TryParse(values[payloadIdx].Trim(), out var payload) ? payload : 0,
                    commands = new List<EnemyModuleEntry>()
                };

                if (!dict.TryAdd(id, currentEnemy))
                {
                    Debug.LogError($"EnemyData Registration Error: Already registered ID {id}");
                }
            }

            if (currentEnemy != null)
            {
                var commandId = values[commandIdx].Trim();
                if (!string.IsNullOrEmpty(commandId))
                {
                    var patchesValue = values[patchesIdx].Trim();
                    currentEnemy.commands.Add(new EnemyModuleEntry
                    {
                        commandId = commandId,
                        patches = string.IsNullOrEmpty(patchesValue) ? Array.Empty<string>() : patchesValue.Split('|')
                    });
                }
            }
        }

        return dict;
    }

    private static void SetFieldValue<T>(T data, FieldInfo field, string value)
    {
        var fieldType = field.FieldType;

        if (fieldType == typeof(int))
        {
            field.SetValue(data, int.TryParse(value, out var result) ? result : 0);
        }
        else if (fieldType == typeof(float))
        {
            field.SetValue(data, float.TryParse(value, out var result) ? result : 0f);
        }
        else if (fieldType == typeof(string))
        {
            field.SetValue(data, value);
        }
        else if (fieldType == typeof(string[]))
        {
            field.SetValue(data, string.IsNullOrEmpty(value) ? Array.Empty<string>() : value.Split('|'));
        }
        else if (fieldType.IsEnum)
        {
            if (Enum.TryParse(fieldType, value, true, out var result))
            {
                field.SetValue(data, result);
            }
        }
    }

    public static ProgramData GetProgramData(string id)
    {
        if (_programData == null) LoadAll();
        if (_programData.TryGetValue(id, out var data)) return data;
        Debug.LogError($"ProgramData not found: {id}");
        return null;
    }

    public static CommandData GetCommandData(string id)
    {
        if (_commandData == null) LoadAll();
        if (_commandData.TryGetValue(id, out var data)) return data;
        Debug.LogError($"CommandData not found: {id}");
        return null;
    }

    public static PatchData GetPatchData(string id)
    {
        if (_patchData == null) LoadAll();
        if (_patchData.TryGetValue(id, out var data)) return data;
        Debug.LogError($"PatchData not found: {id}");
        return null;
    }

    public static EnemyData GetEnemyData(string id)
    {
        if (_enemyData == null) LoadAll();
        if (_enemyData.TryGetValue(id, out var data)) return data;
        Debug.LogError($"EnemyData not found: {id}");
        return null;
    }

    public static BuffData GetBuff(BuffKeyword id)
    {
        if (_buffData == null) LoadAll();
        if (_buffData.TryGetValue(id, out var data)) return data;
        Debug.LogError($"BuffData not found: {id}");
        return null;
    }

    private static Dictionary<int, PoolData> LoadPoolData(string path)
    {
        var dict = new Dictionary<int, PoolData>();
        var csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"{path}.csv not found in Resources/");
            return dict;
        }

        var lines = csv.text.Split('\n');
        if (lines.Length < 2)
            return dict;

        var headers = lines[0].Split(',');
        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = headers[i].Trim();
        }

        int idIdx = Array.IndexOf(headers, "id");
        int commandsIdx = Array.IndexOf(headers, "commands");

        PoolData currentPool = null;

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var values = lines[i].Split(',');
            if (values.Length != headers.Length)
                continue;

            var idValue = values[idIdx].Trim();

            if (!string.IsNullOrEmpty(idValue))
            {
                if (int.TryParse(idValue, out var id))
                {
                    currentPool = new PoolData
                    {
                        id = id,
                        commands = new List<string>()
                    };

                    if (!dict.TryAdd(id, currentPool))
                    {
                        Debug.LogError($"PoolData Registration Error: Already registered ID {id}");
                    }
                }
            }

            if (currentPool != null)
            {
                var commandId = values[commandsIdx].Trim();
                if (!string.IsNullOrEmpty(commandId))
                {
                    currentPool.commands.Add(commandId);
                }
            }
        }

        return dict;
    }

    public static PoolData GetPool(int id)
    {
        if (_poolData == null) LoadAll();
        if (_poolData.TryGetValue(id, out var data)) return data;
        Debug.LogError($"PoolData not found: {id}");
        return null;
    }

    private static Dictionary<int, BattleData> LoadBattleData(string path)
    {
        var dict = new Dictionary<int, BattleData>();
        var csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"{path}.csv not found in Resources/");
            return dict;
        }

        var lines = csv.text.Split('\n');
        if (lines.Length < 2)
            return dict;

        var headers = lines[0].Split(',');
        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = headers[i].Trim();
        }

        int idIdx = Array.IndexOf(headers, "id");
        int enemiesIdx = Array.IndexOf(headers, "enemies");
        int roundLimitIdx = Array.IndexOf(headers, "roundLimit");

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var values = lines[i].Split(',');
            if (values.Length != headers.Length)
                continue;

            var idValue = values[idIdx].Trim();
            if (!int.TryParse(idValue, out var id))
                continue;

            var enemiesValue = values[enemiesIdx].Trim();
            var roundLimitValue = values[roundLimitIdx].Trim();

            var data = new BattleData
            {
                id = id,
                enemies = string.IsNullOrEmpty(enemiesValue) ? Array.Empty<string>() : enemiesValue.Split('|'),
                roundLimit = int.TryParse(roundLimitValue, out var roundLimit) ? roundLimit : 10
            };

            if (!dict.TryAdd(id, data))
            {
                Debug.LogError($"BattleData Registration Error: Already registered ID {id}");
            }
        }

        return dict;
    }

    public static BattleData GetBattleData(int id)
    {
        if (_battleData == null) LoadAll();
        if (_battleData.TryGetValue(id, out var data)) return data;
        Debug.LogError($"BattleData not found: {id}");
        return null;
    }

    private static Dictionary<BuffKeyword, BuffData> LoadBuffData(string path)
    {
        var dict = new Dictionary<BuffKeyword, BuffData>();
        var csv = Resources.Load<TextAsset>(path);

        if (csv == null)
        {
            Debug.LogError($"{path}.csv not found in Resources/");
            return dict;
        }

        var lines = csv.text.Split('\n');
        if (lines.Length < 2)
            return dict;

        var headers = lines[0].Split(',');
        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = headers[i].Trim();
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var values = lines[i].Split(',');
            if (values.Length != headers.Length)
                continue;

            var data = new BuffData();

            for (int j = 0; j < headers.Length; j++)
            {
                var field = typeof(BuffData).GetField(headers[j]);
                if (field == null)
                    continue;

                var value = values[j].Trim();
                SetFieldValue(data, field, value);
            }

            if (!dict.TryAdd(data.id, data))
            {
                Debug.LogError($"BuffData Registration Error: Already registered ID {data.id}");
            }
        }

        return dict;
    }
}
