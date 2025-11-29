[EffectType("EnemyClock")]
public class EnemyClockEffect : TargetedEffectAbility
{
    private int _amount;

    public override void Parse(string[] parts)
    {
        _amount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor, ProgramModel target)
    {
        target.AddCarriedClock(-_amount);
    }
}
