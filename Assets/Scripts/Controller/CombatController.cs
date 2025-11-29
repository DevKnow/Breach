using UnityEngine;

public class CombatController
{
    public bool IsInRange(ProgramModel actor, ProgramModel target, CommandModel command)
    {
        var distance = CombatStatus.GetDistance(actor, target);
        return command.IsInRange(distance);
    }

    public bool TryExecuteAttack(ProgramModel actor, ProgramModel target, CommandModel command)
    {
        if (UseClock(actor, command))
        {
            ExecuteAttack(actor, target, command);
            return true;
        }

        return false;
    }

    public bool CheckSufficient(ProgramModel actor, CommandModel command)
    {
        return actor.IsSufficient(command);
    }

    public bool UseClock(ProgramModel actor, CommandModel command)
    {
        return actor.UseClock(command);
    }

    public void ExecuteAttack(ProgramModel actor, ProgramModel target, CommandModel command)
    {
        var diceCount = GameConstants.DICE_COUNT;
        var penModifier = command.GetPenModifier();

        var hitResult = DamageCalculator.RollHitCheck(
            penModifier,
            actor.Combat.Layer,
            target.Combat.Layer,
            diceCount
        );

        // apply command effects before attack
        command.ApplyEffects(actor, target);

        // execute attack
        actor.TryAttack(target, command, hitResult);

        // post-attack cleanup
        actor.OnEndTryAttack(actor);
    }
}
