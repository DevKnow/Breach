using System.Collections.Generic;
using UnityEngine;

public abstract class ProgramModel
{
    private readonly string _id;

    /// <summary>
    /// current hp
    /// </summary>
    public int CurrentIntegrity => _currentIntegrity;
    private int _currentIntegrity;

    /// <summary>
    /// temp hp (absorbs damage first)
    /// </summary>
    public int TempIntegrity => _tempIntegrity;
    private int _tempIntegrity;

    /// <summary>
    /// current maxhp
    /// </summary>
    private int _defaultMaxIntegrity;
    private int _payloadBonus;

    public int MaxIntegrity
    {
        get
        {
            return Mathf.CeilToInt(
                _defaultMaxIntegrity * (1f + GetMaxIntegrityMultiplier())
                ) + GetMaxIntegrityAdder();
        }
    }

    private List<ModuleModel> _installedModules = new();

    public IReadOnlyCombatStatus Combat => _combat;
    private CombatStatus _combat = new ();

    private BuffStatus _buff;

    public bool IsRevealed => _isRevealed;
    private bool _isRevealed;

    public Team Team => _team;
    protected readonly Team _team;

    public ProgramModel(ProgramData data, Team team)
    {
        _id = data.id;
        _team = team;
        _defaultMaxIntegrity = data.defaultMaxIntegrity;
        _payloadBonus = data.payloadBonus;
        _currentIntegrity = MaxIntegrity;
        _buff = new BuffStatus(this);
    }

    #region Module Funcs

    public void InstallModule(ModuleModel module)
    {
        _installedModules.Add(module);
    }

    public void InstallCommand(string commandId)
    {
        var data = DataLoader.GetCommandData(commandId);
        if (data != null)
            _installedModules.Add(new ModuleModel(data));
    }

    public bool UninstallModule(ModuleModel module)
    {
        return _installedModules.Remove(module);
    }

    public void ClearModules()
    {
        _installedModules.Clear();
    }

    #endregion

    #region Get Funcs

    public string GetID()
    {
        return _id;
    }

    public List<CommandModel> GetAllCommands()
    {
        var commands = new List<CommandModel>();

        for (int i = 0, iMax = _installedModules.Count; i < iMax; i++)
        {
            // TODO
        }

        return commands;
    }

    /// <summary>
    /// max hp multiplier
    /// </summary>
    public float GetMaxIntegrityMultiplier()
    {
        var sum = 0f;
        for(int i=0, iMax = _installedModules.Count; i<iMax; i++)
        {
            sum += _installedModules[i].GetMaxIntegrityMultiplier();
        }

        return sum;
    }

    /// <summary>
    /// max hp adder
    /// </summary>
    public int GetMaxIntegrityAdder()
    {
        var sum = 0;
        for (int i = 0, iMax = _installedModules.Count; i < iMax; i++)
        {
            sum += _installedModules[i].GetMaxIntegrityAdder();
        }

        return sum;
    }

    /// <summary>
    /// attack power modifier
    /// </summary>
    public int GetPayloadAdder()
    {
        var sum = _payloadBonus;
        sum += _buff.GetPayloadAdder();
        for (int i = 0, iMax = _installedModules.Count; i < iMax; i++)
        {
            // TODO
        }

        return sum;
    }

    public float GetCritMultAdder()
    {
        var sum = 0f;
        for (int i = 0, iMax = _installedModules.Count; i < iMax; i++)
        {
            sum += _installedModules[i].GetCritMultAdder();
        }

        return sum;
    }

    public int GetClockCostAdder()
    {
        var sum = 0;
        for (int i = 0, iMax = _installedModules.Count; i < iMax; i++)
        {
            sum += _installedModules[i].GetClockCostAdder();
        }

        return sum;
    }

    public bool IsSufficient(CommandModel command)
    {
        var costAdder = GetClockCostAdder();
        var cost = command.GetClockCost(costAdder);

        return _combat.IsSufficient(cost);
    }

    #endregion

    #region Set/Change/Add Funcs

    public void ChangeLayer(int layer)
    {
        _combat.SetLayer(layer);
    }

    public void AddCarriedClock(int carry)
    {
        _combat.AddCarriedClock(carry);
    }

    public bool UseClock(CommandModel command)
    {
        var costAdder = GetClockCostAdder();
        var cost = command.GetClockCost(costAdder);

        return _combat.UseClock(cost);
    }

    /// <summary>
    /// reset data after round end
    /// </summary>
    public void ResetRound()
    {
        _combat.ResetUsedClock();
    }

    #endregion

    #region Event Funcs

    /// <summary>
    /// set data for this round
    /// </summary>
    public void PrepareRound(int selectedLayer)
    {
        _combat.SetLayer(selectedLayer);
        _combat.SetClock(selectedLayer);
    }

    public void OnRoundStart()
    {
        _buff.OnRoundStart(this);
    }

    public void OnRoundEnd()
    {
        _buff.OnRoundEnd(this);
    }

    public void BeforeRoundStart()
    {
        _buff.BeforeRoundStart();
        ResetRound();
    }

    public void OnAttack(ProgramModel target)
    {
        _buff.OnAttack(this, target);
    }

    public void OnMissAttack(ProgramModel target)
    {
        _buff.OnMissAttack(this, target);
    }

    public void OnHit(int damage)
    {
        _buff.OnHit(this, damage);
    }

    public void Die()
    {

    }

    #endregion

    #region Effect Funcs

    public bool TryHeal(int healAmount, out int overHeal)
    {
        var before = _currentIntegrity;
        var diff = MaxIntegrity - _currentIntegrity;

        if (diff < healAmount)
        {
            overHeal = healAmount - diff;
        }
        else
        {
            overHeal = 0;
        }

        _currentIntegrity += healAmount - overHeal;


        return _currentIntegrity > before;
    }

    public bool TryRevive()
    {
        for (int i=0, iMax = _installedModules.Count; i<iMax; i++)
        {
            if (_installedModules[i].TryRevive())
            {
                return true;
            }
        }

        return false;
    }

    public void Revive(int reviveWithHp = 1)
    {
        _currentIntegrity = reviveWithHp;
    }

    public void AddTempIntegrity(int amount)
    {
        // keep higher value (no stacking)
        if (amount > _tempIntegrity)
            _tempIntegrity = amount;
    }

    public void SetRevealed(bool revealed)
    {
        _isRevealed = revealed;
    }

    public void AddBuff(BuffRequest request)
    {
        _buff.AddBuff(request);
    }

    #endregion

    #region Combat Funcs

    public void TryAttack(ProgramModel target, CommandModel command, HitResult hitResult)
    {
        if (hitResult.type == HitResultType.Miss || hitResult.type == HitResultType.Fumble)
        {
            OnMissAttack(target);
            return;
        }

        var payload = command.GetPayload(GetPayloadAdder());
        var critMultiplier = command.GetCritMultiplier(GetCritMultAdder());

        var damage = DamageCalculator.CalculateDamage(payload, 0, critMultiplier, hitResult.type == HitResultType.Critical);

        target.TakeDamage(damage);
        OnAttack(target);
    }

    public void TakeDamage(int damage)
    {
        var originalDamage = damage;

        // backup absorbs damage first
        if (damage > 0 && _tempIntegrity > 0)
        {
            int absorbed = Mathf.Min(_tempIntegrity, damage);
            _tempIntegrity -= absorbed;
            damage -= absorbed;
        }

        if (damage <= 0)
        {
            OnHit(originalDamage);
            return;
        }

        _currentIntegrity -= damage;
        if (_currentIntegrity < 0)
            _currentIntegrity = 0;

        if (_currentIntegrity > 0)
        {
            OnHit(originalDamage);
            return;
        }

        // if integrity is still 0
        // check revival is available
        if (TryRevive())
        {
            return;
        }

        // or die
        Die();
    }

    public void OnEndTryAttack(ProgramModel owner)
    {
        for (int i = 0, iMax = _installedModules.Count; i < iMax; i++)
        {
            _installedModules[i].OnEndTryAttack(owner);
        }
    }

    public bool HasPriority()
    {
        for (int i = 0, iMax = _installedModules.Count; i < iMax; i++)
        {
            if (_installedModules[i].HasPriority())
                return true;
        }
        return false;
    }

    #endregion
}
