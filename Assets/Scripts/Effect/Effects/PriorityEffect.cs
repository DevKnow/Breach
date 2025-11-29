[EffectType("Priority")]
public class PriorityEffect : NonTargetEffectAbility
{
    public override void Parse(string[] parts)
    {
        // Priority (파라미터 없음)
    }

    public override void Apply(ProgramModel actor)
    {
        // 턴 순서 결정 시 체크됨
    }
}
