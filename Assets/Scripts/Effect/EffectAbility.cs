public abstract class EffectAbility
{
    protected EffectCondition _condition = null;

    public abstract void Parse(string[] args);
}

public abstract class NonTargetEffectAbility : EffectAbility
{
    public abstract void Apply(ProgramModel actor);
}

public abstract class TargetedEffectAbility : EffectAbility
{
    public abstract void Apply(ProgramModel actor, ProgramModel target);
}
