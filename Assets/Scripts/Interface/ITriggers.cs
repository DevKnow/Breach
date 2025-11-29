public interface IOnRoundStart
{
    void OnRoundStart(ProgramModel owner);
}

public interface IOnRoundEnd
{
    void OnRoundEnd(ProgramModel owner);
}

public interface IOnHit
{
    void OnHit(ProgramModel owner, int damage);
}

public interface IOnAttack
{
    void OnAttack(ProgramModel owner, ProgramModel target);
}

public interface IOnMissAttack
{
    void OnMissAttack(ProgramModel owner, ProgramModel target);
}

public interface IOnEndTryAttack
{
    void OnEndTryAttack(ProgramModel owner);
}

public interface IRevival
{
    bool TryRevive();
}

public interface IDefenseModifier
{
    int GetDefenseModifier();
}

public interface IPayloadModifier
{
    int GetPayloadAdder();
}

public interface IPenetrationModifier
{
    int GetPenetrationAdder();
}

public interface IMaxIntegrityModifier
{
    int GetMaxIntegrityAdder();

    float GetMaxIntegrityMultiplier();
}

public interface ICritMultModifier
{
    float GetCritMultAdder();
}

public interface IClockCostModifier
{
    int GetClockCostAdder();
}

public interface IModuleSlotModifier
{
    int GetModuleSlotAdder();
}

public interface IMultiHitModifier
{
    int GetHitCount();
}

public interface IPriority
{
}
