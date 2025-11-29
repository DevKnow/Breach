public abstract class EffectCondition
{
    public abstract bool Check(ProgramModel actor, ProgramModel target);
    public abstract void Parse(string conditionString);
}
