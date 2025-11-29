using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles hit calculation and damage computation for combat.
/// </summary>
public static class DamageCalculator
{
    /// <summary>
    /// Rolls dice and check whether the attack hits.
    /// </summary>
    /// <param name="penModifier">penetration modifier from command and etc.</param>
    /// <param name="attackerLayer">attacker's layer position</param>
    /// <param name="defenderLayer">defender's layer position</param>
    /// <param name="rollCount">Number of dice to roll. Minimum is 2 (values below 2 are clamped)</param>
    /// <param name="keepHighest">When rolling more than 2 dice, determines whether to keep the highest or lowest 2 results.</param>
    /// <param name="randomFunc">Optional random function for testing purposes.</param>
    public static HitResult RollHitCheck(int penModifier, int attackerLayer, int defenderLayer, int rollCount, Func<int, int, int> randomFunc = null)
    {
        var diceResult = RollDiceForHitCheck(rollCount, randomFunc);
        var targetNumber = CalculateTargetNumberForHitCheck(attackerLayer, defenderLayer);
        var resultType = GetResultOfHitCheck(diceResult, targetNumber, penModifier);

        return new HitResult(resultType, diceResult, penModifier);
    }

    /// <summary>
    /// Calculate target number with attacklayer and defender layer
    /// </summary>
    private static int CalculateTargetNumberForHitCheck(int attackerLayer, int defenderLayer)
    {
        var defaultTarget = GameConstants.DEFAULT_TARGET;
        var acc = CalculateAcc(attackerLayer);
        var dodge = CalculateDodge(defenderLayer);
        var distance = Mathf.Abs(attackerLayer - defenderLayer);
        var divide = GameConstants.DIVIDE_NUMBER;
        var distancePenalty = (distance + 1) / divide;
        return defaultTarget + acc - dodge - distancePenalty;
    }

    /// <summary>
    /// accuracy correction
    /// </summary>
    private static int CalculateAcc(int attackerLayer)
    {
        // 1,2 => 2
        // 3,4 => 1
        // 5,6 => 0
        return 2 - (attackerLayer - 1) / 2;
    }

    /// <summary>
    /// dodge correction
    /// </summary>
    private static int CalculateDodge(int defenderLayer)
    {
        // 1,2 => 0
        // 3,4 => 1
        // 5,6 => 2
        return (defenderLayer - 1) / 2;
    }

    /// <summary>
    /// Roll dice for hit calculation
    /// </summary>
    /// <param name="rollCount">Number of dice to roll. Minimum is 2 (values below 2 are clamped)</param>
    /// <param name="keepHighest">When rolling more than 2 dice, determines whether to keep the highest or lowest 2 results.</param>
    /// <param name="randomFunc">Optional random function for testing purposes.</param>
    private static int RollDiceForHitCheck(int rollCount, Func<int, int, int> randomFunc)
    {
        if (rollCount < 2)
            rollCount = 2;

        randomFunc ??= UnityEngine.Random.Range;
        int sides = GameConstants.DICE_SIDES + 1;

        // if dice count is more than 2
        if(rollCount > 2)
        {
            // make a list for sorting
            var rolls = new List<int>(rollCount);
            for (int i = 0; i < rollCount; i++)
            {
                rolls.Add(randomFunc(1, sides));
            }

            rolls.Sort();

            return rolls[0] + rolls[1];
        }

        return randomFunc(1, sides) + randomFunc(1, sides);
    }

    private static HitResultType GetResultOfHitCheck(int diceResult, int targetNumber, int modifier)
    {
        return diceResult switch
        {
            // only use dice results for checking critical or fumble
            GameConstants.CRITICAL => HitResultType.Critical,
            GameConstants.FUMBLE => HitResultType.Fumble,

            // if it's not critical or fumble, add modifier to dice result for hit check
            _ when diceResult + modifier <= targetNumber => HitResultType.Hit,
            _ => HitResultType.Miss
        };
    }

    public static int CalculateDamage(int commandPayload, int payloadModifier, float critMultiplier, bool isCrit)
    {
        if (isCrit)
        {
            return Mathf.FloorToInt(commandPayload * critMultiplier) + payloadModifier;
        }

        return commandPayload + payloadModifier;
    }

    /// <summary>
    /// Roll 2d6 and return sum.
    /// </summary>
    public static int RollDice()
    {
        int sides = GameConstants.DICE_SIDES + 1;
        return UnityEngine.Random.Range(1, sides) + UnityEngine.Random.Range(1, sides);
    }
}

public struct HitResult
{
    public HitResultType type;
    public int diceResult;
    public int modifier;

    public HitResult(HitResultType type, int diceResult, int modifier)
    {
        this.type = type;
        this.diceResult = diceResult;
        this.modifier = modifier;
    }
}
