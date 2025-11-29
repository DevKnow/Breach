[ConditionType("Distance")]
public class DistanceCondition : EffectCondition
{
    private CompareType _compare;
    private int _value;

    public override void Parse(string remaining)
    {
        CompareHelper.TryParseCondition(remaining, out _compare, out _value);
    }

    public override bool Check(ProgramModel actor, ProgramModel target)
    {
        int distance = CombatStatus.GetDistance(actor, target);
        return CompareHelper.Compare(distance, _value, _compare);
    }
}
