using System.Collections.Generic;

public class ModuleModel
{
    private CommandModel _command;
    private List<PatchModel> _installedPatches = new();

    public CommandModel Command => _command;
    public IReadOnlyList<PatchModel> InstalledPatches => _installedPatches;

    public ModuleModel(CommandData commandData)
    {
        _command = new CommandModel(commandData);
    }

    public ModuleModel(CommandModel command)
    {
        _command = command;
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
        var sum = 0;
        for (int i = 0, iMax = _installedPatches.Count; i < iMax; i++)
        {
            sum += _installedPatches[i].GetMaxIntegrityAdder();
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
