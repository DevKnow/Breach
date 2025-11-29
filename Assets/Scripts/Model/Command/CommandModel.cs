using System.Collections.Generic;

public class CommandModel
{
    public CommandData Data => _data;
    private CommandData _data;

    private List<EffectAbility> _effects = new();

    public CommandModel(CommandData data)
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

    #region Getters

    public int GetClockCost(int modifier = 0)
    {
        return _data.clockCost + modifier;
    }

    public int GetPayload(int modifier = 0)
    {
        return _data.payload + modifier;
    }

    public float GetCritMultiplier(float modifier = 0f)
    {
        return _data.criticalMultiplier + modifier;
    }

    public int GetPenModifier(int modifier = 0)
    {
        return _data.penModifier + modifier;
    }

    public bool IsInRange(int distance)
    {
        return distance >= _data.minRange && distance <= _data.maxRange;
    }

    #endregion

    #region Execute

    public void ApplyEffects(ProgramModel actor, ProgramModel target)
    {
        for (int i = 0, iMax = _effects.Count; i < iMax; i++)
        {
            var effect = _effects[i];

            if (effect is TargetedEffectAbility targeted)
            {
                targeted.Apply(actor, target);
            }
            else if (effect is NonTargetEffectAbility nonTarget)
            {
                nonTarget.Apply(actor);
            }
        }
    }

    #endregion
}
