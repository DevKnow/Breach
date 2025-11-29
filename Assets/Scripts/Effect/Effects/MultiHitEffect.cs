[EffectType("MultiHit")]
public class MultiHitEffect : NonTargetEffectAbility, IMultiHitModifier
{
    private int _hitCount;

    public override void Parse(string[] parts)
    {
        // MultiHit:3
        _hitCount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // hit count is checked during attack execution
    }

    public int GetHitCount()
    {
        return _hitCount;
    }
}
