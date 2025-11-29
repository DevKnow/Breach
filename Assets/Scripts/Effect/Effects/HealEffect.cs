[EffectType("Heal")]
public class HealEffect : TargetedEffectAbility
{
    private int _amount;

    public override void Parse(string[] parts)
    {
        _amount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor, ProgramModel target)
    {
        target.TryHeal(_amount, out _);
    }
}
