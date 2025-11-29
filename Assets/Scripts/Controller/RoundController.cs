using System;
using System.Collections.Generic;

public class RoundController
{
    private CombatController _combat = new();

    private List<ProgramModel> _participants = new();
    private List<ProgramModel> _currentLayerAgents = new();
    private int _currentRound;
    private int _currentLayer;
    private int _currentTurnIndex;
    private RoundPhase _phase = RoundPhase.IDLE;

    public CombatController Combat => _combat;
    public int CurrentRound => _currentRound;
    public int CurrentLayer => _currentLayer;
    public RoundPhase Phase => _phase;
    public ProgramModel CurrentActor => _currentLayerAgents.Count > 0 ? _currentLayerAgents[_currentTurnIndex] : null;

    // Events
    public event Action<RoundPhase> OnPhaseChanged;
    public event Action<ProgramModel> OnTurnStarted;

    private void SetPhase(RoundPhase phase)
    {
        _phase = phase;
        OnPhaseChanged?.Invoke(_phase);
    }

    #region Battle Flow

    public void StartBattle(List<ProgramModel> participants)
    {
        _participants = participants;
        _currentRound = 0;

        StartRound();
    }

    public void StartRound()
    {
        SetPhase(RoundPhase.PROCESSING);

        // 1. BeforeRoundStart for all
        for (int i = 0, iMax = _participants.Count; i < iMax; i++)
        {
            _participants[i].BeforeRoundStart();
        }

        // 2. increment round
        _currentRound++;

        // 3. enemies decide & prepare layer
        for (int i = 0, iMax = _participants.Count; i < iMax; i++)
        {
            if (_participants[i] is not EnemyProgramModel enemy)
                continue;

            enemy.DecideEnemyLayer();
        }

        // 4. wait for player input
        SetPhase(RoundPhase.LAYER_SELECT);
    }

    public void OnPlayerSelectLayer(int layer)
    {
        SetPhase(RoundPhase.PROCESSING);

        // 5. player prepare round
        for (int i = 0, iMax = _participants.Count; i < iMax; i++)
        {
            if (_participants[i].Team is not Team.PLAYER)
                continue;

            _participants[i].PrepareRound(layer);
        }

        // 6. OnRoundStart for all
        for (int i = 0, iMax = _participants.Count; i < iMax; i++)
        {
            _participants[i].OnRoundStart();
        }

        // 7. start turn processing
        _currentLayer = GameConstants.MaxLayer + 1;
        ProcessNextLayer();
    }

    private void ProcessNextLayer()
    {
        _currentLayer--;
        _currentTurnIndex = 0;
        _currentLayerAgents.Clear();

        if (_currentLayer < GameConstants.MinLayer)
        {
            EndRound();
            return;
        }

        // get agents in current layer
        for (int i = 0, iMax = _participants.Count; i < iMax; i++)
        {
            if (_participants[i].Combat.Layer == _currentLayer)
                _currentLayerAgents.Add(_participants[i]);
        }

        // no agents in this layer, move to next
        if (_currentLayerAgents.Count == 0)
        {
            ProcessNextLayer();
            return;
        }

        // sort agents in same layer
        SortCurrentLayerAgents();

        // start first turn in this layer
        StartCurrentTurn();
    }

    private void StartCurrentTurn()
    {
        var actor = CurrentActor;
        OnTurnStarted?.Invoke(actor);

        if (actor.Team == Team.PLAYER)
        {
            SetPhase(RoundPhase.TURN_ACTION);
        }
        else
        {
            SetPhase(RoundPhase.ENEMY_TURN);
            // TODO: AI action
        }
    }

    private void SortCurrentLayerAgents()
    {
        if (_currentLayerAgents.Count <= 1)
            return;

        // assign roll values first to avoid re-rolling during sort
        var rollValues = new Dictionary<ProgramModel, int>();
        for (int i = 0, iMax = _currentLayerAgents.Count; i < iMax; i++)
        {
            rollValues[_currentLayerAgents[i]] = DamageCalculator.RollDice();
        }

        _currentLayerAgents.Sort((a, b) =>
        {
            // 1. Priority first
            bool aPriority = a.HasPriority();
            bool bPriority = b.HasPriority();
            if (aPriority != bPriority)
                return bPriority.CompareTo(aPriority);

            // 2. Lower roll wins
            return rollValues[a].CompareTo(rollValues[b]);
        });
    }

    public void EndRound()
    {
        // round end triggers
        for (int i = 0, iMax = _participants.Count; i < iMax; i++)
        {
            _participants[i].OnRoundEnd();
        }

        StartRound();
    }

    public void EndBattle()
    {
        _participants.Clear();
        _currentRound = 0;
        _currentTurnIndex = 0;
        SetPhase(RoundPhase.IDLE);
    }

    #endregion

    #region Turn Flow

    public void NextTurn()
    {
        _currentTurnIndex++;

        if (_currentTurnIndex >= _currentLayerAgents.Count)
        {
            ProcessNextLayer();
        }
        else
        {
            StartCurrentTurn();
        }
    }

    public bool IsPlayerTurn()
    {
        return CurrentActor is PlayerProgramModel;
    }

    #endregion
}
