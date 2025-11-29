[EffectType("ModuleSlot")]
public class ModuleSlotEffect : NonTargetEffectAbility, IModuleSlotModifier
{
    private int _amount;

    public override void Parse(string[] parts)
    {
        // ModuleSlot:1
        _amount = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // passive modifier - no direct effect on apply
    }

    public int GetModuleSlotAdder()
    {
        return _amount;
    }
}
