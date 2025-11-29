using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 전체를 관리하는 매니저
/// GameManager에서 생성/호출됨
/// </summary>
public class BattleManager : MonoBehaviour
{
    private RoundController _roundController;
    private List<ProgramModel> _allAgents = new();

    public RoundController RoundController => _roundController;

    // 전투 결과 이벤트
    public event Action OnBattleVictory;
    public event Action OnBattleDefeat;

    #region Initialization

    private void Awake()
    {
        _roundController = new RoundController();
    }

    /// <summary>
    /// 전투 시작
    /// </summary>
    public void StartBattle(List<ProgramModel> playerAgents, List<ProgramModel> enemyAgents)
    {
        _allAgents.Clear();
        _allAgents.AddRange(playerAgents);
        _allAgents.AddRange(enemyAgents);

        // 이벤트 구독
        _roundController.OnPhaseChanged += HandlePhaseChanged;
        _roundController.OnTurnStarted += HandleTurnStarted;

        _roundController.StartBattle(_allAgents);
    }

    public void EndBattle()
    {
        // 이벤트 구독 해제
        _roundController.OnPhaseChanged -= HandlePhaseChanged;
        _roundController.OnTurnStarted -= HandleTurnStarted;

        _roundController.EndBattle();
        _allAgents.Clear();
    }

    #endregion

    #region Event Handlers

    private void HandlePhaseChanged(RoundPhase phase)
    {
        Debug.Log($"[Battle] Phase: {phase}");

        // 매 페이즈마다 승패 체크
        CheckBattleEnd();

        // 적 턴이면 자동 진행
        if (phase == RoundPhase.ENEMY_TURN)
        {
            ProcessEnemyTurn();
        }
    }

    private void ProcessEnemyTurn()
    {
        var actor = _roundController.CurrentActor;
        if (actor == null)
            return;

        // TODO: 적 AI 로직 (공격 대상 선택, 스킬 사용 등)
        Debug.Log($"[Battle] Enemy {actor.GetID()} is thinking...");

        // 일단 턴 종료
        _roundController.NextTurn();
    }

    private void HandleTurnStarted(ProgramModel actor)
    {
        if (actor == null)
            return;

        Debug.Log($"[Battle] Turn: {actor.GetID()} (Layer {_roundController.CurrentLayer})");
    }

    #endregion

    #region Battle Logic

    private void CheckBattleEnd()
    {
        bool hasPlayer = false;
        bool hasEnemy = false;

        for (int i = 0, iMax = _allAgents.Count; i < iMax; i++)
        {
            if (_allAgents[i].CurrentIntegrity <= 0)
                continue;

            if (_allAgents[i].Team == Team.PLAYER)
                hasPlayer = true;
            else
                hasEnemy = true;
        }

        if (!hasEnemy)
        {
            OnBattleVictory?.Invoke();
            EndBattle();
        }
        else if (!hasPlayer)
        {
            OnBattleDefeat?.Invoke();
            EndBattle();
        }
    }

    /// <summary>
    /// 플레이어 공격 실행
    /// </summary>
    public bool TryPlayerAttack(ProgramModel target, CommandModel command)
    {
        if (_roundController.Phase != RoundPhase.TURN_ACTION)
            return false;

        var actor = _roundController.CurrentActor;
        if (actor == null || actor.Team != Team.PLAYER)
            return false;

        return _roundController.Combat.TryExecuteAttack(actor, target, command);
    }

    /// <summary>
    /// 현재 살아있는 적 목록
    /// </summary>
    public List<ProgramModel> GetAliveEnemies()
    {
        var result = new List<ProgramModel>();

        for (int i = 0, iMax = _allAgents.Count; i < iMax; i++)
        {
            if (_allAgents[i].Team == Team.ENEMY && _allAgents[i].CurrentIntegrity > 0)
                result.Add(_allAgents[i]);
        }

        return result;
    }

    /// <summary>
    /// 현재 살아있는 아군 목록
    /// </summary>
    public List<ProgramModel> GetAliveAllies()
    {
        var result = new List<ProgramModel>();

        for (int i = 0, iMax = _allAgents.Count; i < iMax; i++)
        {
            if (_allAgents[i].Team == Team.PLAYER && _allAgents[i].CurrentIntegrity > 0)
                result.Add(_allAgents[i]);
        }

        return result;
    }

    #endregion
}
