using UnityEngine;

/// <summary>
/// Base class for all command effects in combat
/// </summary>
public abstract class CommandEffect
{
    /// <summary>
    /// Execute non-target effect
    /// </summary>
    public virtual void Execute(ProgramModel user)
    {
        
    }

    /// <summary>
    /// Execute targetable effect
    /// </summary>
    public virtual void Execute(ProgramModel user, ProgramModel target)
    {
        
    }
}