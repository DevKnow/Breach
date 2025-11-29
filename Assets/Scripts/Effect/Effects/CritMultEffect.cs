[EffectType("CritMult")]
public class CritMultEffect : NonTargetEffectAbility, ICritMultModifier
{
    private float _amount;

    public override void Parse(string[] parts)
    {
        // CritMult:0.5
        _amount = float.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // passive modifier - no direct effect on apply
    }

    public float GetCritMultAdder()
    {
        return _amount;
    }
}
