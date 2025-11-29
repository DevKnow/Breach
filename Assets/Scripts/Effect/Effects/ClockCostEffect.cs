[EffectType("ClockCost")]
public class ClockCostEffect : NonTargetEffectAbility, IClockCostModifier
{
    private int _amount;

    public override void Parse(string[] parts)
    {
        // ClockCost:-1
        _amount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // passive modifier - no direct effect on apply
    }

    public int GetClockCostAdder()
    {
        return _amount;
    }
}
