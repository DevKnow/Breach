using System.Collections.Generic;

public class PatchModel
{
    private PatchData _data;
    private List<EffectAbility> _effects = new();

    public PatchData Data => _data;

    public PatchModel(PatchData data)
    {
        _data = data;
        ParseEffects();
    }

    private void ParseEffects()
    {
        if (_data.effects == null)
            return;

        for (int i = 0, iMax = _data.effects.Length; i < iMax; i++)
        {
            var effect = EffectParser.Parse(_data.effects[i]);
            if (effect != null)
                _effects.Add(effect);
        }
    }

    #region modifier

    public int GetPayloadAdder()
    {
        int sum = 0;
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is not IPayloadModifier effect)
                continue;

            sum += effect.GetPayloadAdder();
        }
        return sum;
    }

    public int GetMaxIntegrityAdder()
    {
        int sum = 0;
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is not IMaxIntegrityModifier effect)
                continue;

            sum += effect.GetMaxIntegrityAdder();
        }
        return sum;
    }

    public float GetMaxIntegrityMultiplier()
    {
        var sum = 0f;
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is not IMaxIntegrityModifier effect)
                continue;

            sum += effect.GetMaxIntegrityMultiplier();
        }
        return sum;
    }

    public float GetCritMultAdder()
    {
        var sum = 0f;
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is not ICritMultModifier effect)
                continue;

            sum += effect.GetCritMultAdder();
        }
        return sum;
    }

    public int GetClockCostAdder()
    {
        int sum = 0;
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is not IClockCostModifier effect)
                continue;

            sum += effect.GetClockCostAdder();
        }
        return sum;
    }

    public int GetModuleSlotAdder()
    {
        int sum = 0;
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is not IModuleSlotModifier effect)
                continue;

            sum += effect.GetModuleSlotAdder();
        }
        return sum;
    }

    #endregion

    #region Effect

    public virtual bool TryRevive()
    {
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is not IRevival effect)
                continue;

            if (effect.TryRevive())
                return true;
        }

        return false;
    }

    public void OnEndTryAttack(ProgramModel owner)
    {
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is IOnEndTryAttack effect)
                effect.OnEndTryAttack(owner);
        }
    }

    public bool HasPriority()
    {
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            if (_effects[i] is IPriority)
                return true;
        }
        return false;
    }

    #endregion
}
