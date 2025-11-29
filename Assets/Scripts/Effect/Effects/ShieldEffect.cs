[EffectType("Shield")]
public class ShieldEffect : NonTargetEffectAbility, IDefenseModifier
{
    private int _amount;

    public override void Parse(string[] parts)
    {
        _amount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor) { }

    public int GetDefenseModifier() => _amount;
}
