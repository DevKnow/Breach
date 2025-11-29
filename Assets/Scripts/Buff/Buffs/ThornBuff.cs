[BuffType(BuffKeyword.Thorn)]
public class ThornBuff : BuffModel, IOnRoundEnd
{
    public override DurationTrigger DurationTrigger => DurationTrigger.OnRoundEnd;

    public void OnRoundEnd(ProgramModel owner)
    {
        owner.TakeDamage(_stack);
    }
}
