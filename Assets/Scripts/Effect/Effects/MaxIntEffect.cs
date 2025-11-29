[EffectType("MaxInt")]
public class MaxIntEffect : NonTargetEffectAbility, IMaxIntegrityModifier
{
    private int _amount;

    public override void Parse(string[] parts)
    {
        // MaxInt:10
        _amount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // passive modifier - no direct effect on apply
    }

    public int GetMaxIntegrityAdder()
    {
        return _amount;
    }

    public float GetMaxIntegrityMultiplier()
    {
        return 0f;
    }
}
