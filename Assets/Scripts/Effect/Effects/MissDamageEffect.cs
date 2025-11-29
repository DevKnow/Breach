[EffectType("MissDamage")]
public class MissDamageEffect : TargetedEffectAbility
{
    private int _damage;

    public override void Parse(string[] parts)
    {
        _damage = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor, ProgramModel target)
    {
        // 빗나갔을 때 호출됨
        target.TakeDamage(_damage);
    }

    public int Damage => _damage;
}
