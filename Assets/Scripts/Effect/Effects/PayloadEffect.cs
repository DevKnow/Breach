using UnityEngine;

[EffectType("Payload")]
public class PayloadEffect : NonTargetEffectAbility, IPayloadModifier
{
    private int _baseAmount;
    private bool _distanceModified;
    private int _minValue;

    public override void Parse(string[] parts)
    {
        // Payload:2 또는 Payload:4:-Distance:0
        _baseAmount = int.Parse(parts[1]);

        if (parts.Length > 2 && parts[2] == "-Distance")
        {
            _distanceModified = true;
            _minValue = parts.Length > 3 ? int.Parse(parts[3]) : 0;
        }
    }

    public override void Apply(ProgramModel actor)
    {
        // passive modifier - no direct effect on apply
    }

    public int GetPayloadAdder()
    {
        return _baseAmount;
    }

    public int GetAmount(int distance)
    {
        if (_distanceModified)
        {
            return Mathf.Max(_baseAmount - distance, _minValue);
        }
        return _baseAmount;
    }
}
