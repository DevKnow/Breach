using System;
using System.Collections.Generic;

public class BuffStatus
{
    private List<BuffModel> _buffs = new();
    private List<BuffRequest> _pending = new();

    public BuffStatus(ProgramModel owner)
    {
    }

    public void AddBuff(BuffRequest request)
    {
        if (request.delay > 0)
        {
            _pending.Add(request);
            return;
        }

        ApplyBuff(request);
    }

    private void ApplyBuff(BuffRequest request)
    {
        var buff = GrabBuff(request);

        // 스택 누적
        buff.StackUp(request);
    }

    private BuffModel GrabBuff(BuffRequest request)
    {
        var buffID = request.id;

        // find the already existed buff
        for (int i = 0; i < _buffs.Count; i++)
        {
            if (_buffs[i].Data.id != buffID)
                continue;

            return _buffs[i];
        }

        // if can't find it, create new one.
        var data = DataLoader.GetBuff(buffID);
        var newBuff = BuffFactory.Create(data);
        _buffs.Add(newBuff);
        return newBuff;
    }

    public void BeforeRoundStart()
    {
        for (int i = _pending.Count - 1; i >= 0; i--)
        {
            var request = _pending[i];
            request.delay--;

            if (request.delay <= 0)
            {
                ApplyBuff(request);
                _pending.RemoveAt(i);
            }
            else
            {
                _pending[i] = request;
            }
        }
    }

    public void RemoveBuff(BuffKeyword buffID)
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i].Data.id == buffID)
            {
                _buffs.RemoveAt(i);
                return;
            }
        }
    }

    #region Event Funcs

    public void OnRoundStart(ProgramModel owner)
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i] is not IOnRoundStart buff)
                continue;

            buff.OnRoundStart(owner);
        }

        CheckDurationTrigger(DurationTrigger.OnRoundStart);
    }

    public void OnRoundEnd(ProgramModel owner)
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i] is not IOnRoundEnd buff)
                continue;

            buff.OnRoundEnd(owner);
        }

        CheckDurationTrigger(DurationTrigger.OnRoundEnd);
    }

    public void OnHit(ProgramModel owner, int damage)
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i] is not IOnHit buff)
                continue;

            buff.OnHit(owner, damage);
        }

        CheckDurationTrigger(DurationTrigger.OnHit);
    }

    public void OnAttack(ProgramModel owner, ProgramModel target)
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i] is not IOnAttack buff)
                continue;

            buff.OnAttack(owner, target);
        }

        CheckDurationTrigger(DurationTrigger.OnAttack);
    }

    public void OnMissAttack(ProgramModel owner, ProgramModel target)
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i] is not IOnMissAttack buff)
                continue;

            buff.OnMissAttack(owner, target);
        }

        CheckDurationTrigger(DurationTrigger.OnAttack);
    }

    #endregion

    #region Modifier

    public int GetPayloadAdder()
    {
        var sum = 0;
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i] is not IPayloadModifier buff)
                continue;

            buff.GetPayloadAdder();
        }

        return sum;
    }

    #endregion

    private void CheckDurationTrigger(DurationTrigger checkTrigger)
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i].DurationTrigger != checkTrigger)
                continue;

            _buffs[i].ReduceDuration();
            if (_buffs[i].IsExpired)
                _buffs.RemoveAt(i);
        }
    }
}
