using System.Collections.Generic;

public class ModuleModel
{
    private ModuleData _data;
    private List<CommandModel> _commands = new();
    private List<PatchModel> _installedPatches = new();

    public ModuleData Data => _data;
    public IReadOnlyList<CommandModel> Commands => _commands;

    public ModuleModel(ModuleData data)
    {
        _data = data;
        ParseCommands();
    }

    private void ParseCommands()
    {
        if (_data.commands == null)
            return;

        for (int i = 0, iMax = _data.commands.Length; i < iMax; i++)
        {
            var commandData = DataLoader.GetCommandData(_data.commands[i]);
            if (commandData != null)
                _commands.Add(new CommandModel(commandData));
        }
    }

    #region Patch Funcs

    public void InstallPatch(PatchModel patch)
    {
        _installedPatches.Add(patch);
    }

    public void InstallPatch(string patchId)
    {
        var data = DataLoader.GetPatchData(patchId);
        if (data != null)
            _installedPatches.Add(new PatchModel(data));
    }

    public bool UninstallPatch(PatchModel patch)
    {
        return _installedPatches.Remove(patch);
    }

    #endregion

    #region Modifier

    public float GetMaxIntegrityMultiplier()
    {
        var sum = 0f;
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            sum += _installedPatches[i].GetMaxIntegrityMultiplier();
        }
        return sum;
    }

    public int GetMaxIntegrityAdder()
    {
        var sum = _data.integrityBonus;
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            sum += _installedPatches[i].GetMaxIntegrityAdder();
        }
        return sum;
    }

    public int GetPayloadAdder()
    {
        var sum = _data.payloadBonus;
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            sum += _installedPatches[i].GetPayloadAdder();
        }
        return sum;
    }

    public float GetCritMultAdder()
    {
        var sum = 0f;
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            sum += _installedPatches[i].GetCritMultAdder();
        }
        return sum;
    }

    public int GetClockCostAdder()
    {
        var sum = 0;
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            sum += _installedPatches[i].GetClockCostAdder();
        }
        return sum;
    }

    public int GetModuleSlotAdder()
    {
        var sum = 0;
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            sum += _installedPatches[i].GetModuleSlotAdder();
        }
        return sum;
    }

    #endregion

    #region Effect

    public bool TryRevive()
    {
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            if (_installedPatches[i].TryRevive())
                return true;
        }
        return false;
    }

    public void OnEndTryAttack(ProgramModel owner)
    {
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            _installedPatches[i].OnEndTryAttack(owner);
        }
    }

    public bool HasPriority()
    {
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            if (_installedPatches[i].HasPriority())
                return true;
        }
        return false;
    }

    #endregion
}
