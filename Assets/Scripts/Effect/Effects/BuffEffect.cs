using UnityEngine;

[EffectType("Buff")]
public class BuffEffect : TargetedEffectAbility
{
    private BuffKeyword _buffId;
    private int _stack;
    private int _duration;
    private int _delay;

    public override void Parse(string[] parts)
    {
        // Debuff:Thorn:2:3 → 즉시 적용
        // Debuff:Thorn:2:3:1 → 1턴 후 적용
        if (parts.Length < 4)
        {
            Debug.LogError("Debuff: not enough args");
            return;
        }

        _buffId = BuffFactory.FindKeyword(parts[1]);
        _stack = int.Parse(parts[2]);
        _duration = int.Parse(parts[3]);
        _delay = parts.Length > 4 ? int.Parse(parts[4]) : 0;
    }

    public override void Apply(ProgramModel actor, ProgramModel target)
    {
        if (_buffId == BuffKeyword.Error)
            return;

        var request = new BuffRequest(_buffId, _stack, _duration, _delay);
        target.AddBuff(request);
    }
}
