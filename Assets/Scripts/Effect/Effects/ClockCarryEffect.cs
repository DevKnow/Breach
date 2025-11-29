using UnityEngine;

[EffectType("ClockCarry")]
public class ClockCarryEffect : NonTargetEffectAbility
{
    private int _max;

    public override void Parse(string[] parts)
    {
        // ClockCarry:2
        _max = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // 라운드 종료 시 호출됨
        int carry = Mathf.Min(actor.Combat.RemainingClock, _max);
        actor.AddCarriedClock(carry);
    }

    public int GetMax()
    {
        return _max;
    }
}
