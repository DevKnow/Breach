[EffectType("Penetration")]
public class PenetrationEffect : TargetedEffectAbility, IPenetrationModifier, IOnEndTryAttack
{
    private int _amount;
    private bool _check = false;

    public override void Parse(string[] parts)
    {
        _amount = int.Parse(parts[1]);
        _condition = parts.Length > 2 ? ConditionParser.Parse(parts[2]) : null;
    }

    public int GetPenetrationAdder()
    {
        if (_check)
        {
            return _amount;
        }

        return 0;
    }

    public bool CheckCondition(ProgramModel actor, ProgramModel target)
    {
        if (_condition == null)
            return true;

        return _condition.Check(actor, target);
    }

    public override void Apply(ProgramModel actor, ProgramModel target)
    {
        if (!CheckCondition(actor, target))
            return;

        _check = true;
    }

    public void OnEndTryAttack(ProgramModel owner)
    {
        _check = false;
    }
}
