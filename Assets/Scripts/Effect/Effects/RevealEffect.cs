[EffectType("Reveal")]
public class RevealEffect : TargetedEffectAbility
{
    public override void Parse(string[] parts)
    {
        // Reveal (파라미터 없음)
    }

    public override void Apply(ProgramModel actor, ProgramModel target)
    {
        target.SetRevealed(true);
    }
}
