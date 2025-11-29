using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatus : IReadOnlyCombatStatus
{
    public int Layer => _layer;
    private int _layer;
    public int Clock => _clock;
    private int _clock;
    public int RemainingClock => _clock - _usedClock;
    private int _usedClock;
    private int _carriedClock;

    public static int GetDistance(ProgramModel a, ProgramModel b)
    {
        return Mathf.Abs(a.Combat.Layer - b.Combat.Layer);
    }

    public void SetLayer(int layer)
    {
        _layer = layer;
    }

    public void SetClock(int clock)
    {
        _clock = _carriedClock + clock;
        _carriedClock = 0;
    }

    public void ResetUsedClock()
    {
        _usedClock = 0;
    }

    public void AddCarriedClock(int clock)
    {
        _carriedClock += clock;
    }

    public bool IsSufficient(int needCost)
    {
        return RemainingClock >= needCost;
    }

    public bool UseClock(int clock)
    {
        if (IsSufficient(clock))
        {
            _usedClock += clock;
            return true;
        }

        return false;
    }

}

public interface IReadOnlyCombatStatus
{
    int Layer { get; }
    int Clock { get; }
    int RemainingClock { get; }
}