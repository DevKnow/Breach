[EffectType("Regen")]
public class RegenEffect : NonTargetEffectAbility
{
    private int _amount;

    public override void Parse(string[] parts)
    {
        // Regen:3
        _amount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // 라운드 종료 시 호출됨
        actor.TryHeal(_amount, out _);
    }

    public int GetAmount()
    {
        return _amount;
    }
}
