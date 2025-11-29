using UnityEngine;

[EffectType("Deflect")]
public class DeflectEffect : NonTargetEffectAbility
{
    private int _chance;

    public override void Parse(string[] parts)
    {
        // Deflect:15
        _chance = int.Parse(parts[1]);
    }

    public override void Apply(ProgramModel actor)
    {
        // 피격 시 체크됨
    }

    public bool TryDeflect()
    {
        return Random.Range(0, 100) < _chance;
    }

    public int GetChance()
    {
        return _chance;
    }
}
