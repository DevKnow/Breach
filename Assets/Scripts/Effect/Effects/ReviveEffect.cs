[EffectType("Revive")]
public class ReviveEffect : NonTargetEffectAbility
{
    private int _integrity;
    private int _maxUses;
    private int _usedCount;

    public override void Parse(string[] parts)
    {
        // Revive:10:1
        _integrity = int.Parse(parts[1]);
        _maxUses = int.Parse(parts[2]);
        _usedCount = 0;
    }

    public override void Apply(ProgramModel actor)
    {
        // 사망 시 체크됨
    }

    public bool TryRevive(ProgramModel actor)
    {
        if (_usedCount >= _maxUses) return false;

        _usedCount++;
        actor.Revive(_integrity);
        return true;
    }

    public bool CanRevive()
    {
        return _usedCount < _maxUses;
    }
}
