using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI와 RoundController를 연결하는 View 클래스
/// </summary>
public class BattleView : MonoBehaviour
{
    [Header("Layer Selection UI")]
    [SerializeField] private GameObject _layerSelectPanel;
    [SerializeField] private Button _layerButtonPrefab;
    private List<Button> _layerButtons = new();

    [Header("Turn Action UI")]
    [SerializeField] private GameObject _turnActionPanel;
    [SerializeField] private Button _endTurnButton;

    [Header("Command UI")]
    [SerializeField] private Transform _commandButtonContainer;
    [SerializeField] private Button _commandButtonPrefab;
    private List<Button> _commandButtons = new();

    [Header("Info Display")]
    [SerializeField] private TMP_Text _phaseText;
    [SerializeField] private TMP_Text _roundText;
    [SerializeField] private TMP_Text _currentActorText;

    private RoundController _controller;
    private BattleManager _battleManager;

    // Command 선택 이벤트 - BattleManager에서 구독
    public event Action<CommandModel> OnCommandSelected;

    #region Initialization

    /// <summary>
    /// View 초기화 - Controller와 연결
    /// </summary>
    public void Initialize(RoundController controller, BattleManager battleManager)
    {
        _controller = controller;
        _battleManager = battleManager;

        // 이벤트 구독 (Subscribe)
        // controller에서 이벤트가 발생하면 등록된 함수가 호출됨
        _controller.OnPhaseChanged += HandlePhaseChanged;
        _controller.OnTurnStarted += HandleTurnStarted;

        // 버튼 클릭 이벤트 등록
        SetupButtons();
    }

    private void SetupButtons()
    {
        // End Turn 버튼
        if (_endTurnButton != null)
            _endTurnButton.onClick.AddListener(OnEndTurnClicked);
    }

    /// <summary>
    /// 섹터에 따라 레이어 버튼 동적 생성
    /// </summary>
    public void SetupLayerButtons(int maxLayer)
    {
        // 기존 버튼 제거
        for (int i = 0; i < _layerButtons.Count; i++)
        {
            Destroy(_layerButtons[i].gameObject);
        }
        _layerButtons.Clear();

        // 새 버튼 생성
        float buttonWidth = 80f;
        float spacing = 20f;
        float totalWidth = maxLayer * buttonWidth + (maxLayer - 1) * spacing;
        float startX = -totalWidth / 2f + buttonWidth / 2f;

        for (int i = 0; i < maxLayer; i++)
        {
            var button = Instantiate(_layerButtonPrefab, _layerSelectPanel.transform);
            var rect = button.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(startX + i * (buttonWidth + spacing), 0);

            // 버튼 텍스트 설정
            var text = button.GetComponentInChildren<TMP_Text>();
            int layer = i + 1;
            if (text != null)
                text.text = layer.ToString();

            // 클릭 이벤트
            button.onClick.AddListener(() => OnLayerButtonClicked(layer));

            _layerButtons.Add(button);
        }
    }

    /// <summary>
    /// 현재 Actor의 Command 버튼 동적 생성
    /// </summary>
    private void SetupCommandButtons(ProgramModel actor)
    {
        ClearCommandButtons();

        if (actor == null || _commandButtonPrefab == null || _commandButtonContainer == null)
            return;

        var commands = actor.GetAllCommands();

        for (int i = 0, iMax = commands.Count; i < iMax; i++)
        {
            var command = commands[i];
            var button = Instantiate(_commandButtonPrefab, _commandButtonContainer);

            // 버튼 텍스트 설정
            var text = button.GetComponentInChildren<TMP_Text>();
            if (text != null)
                text.text = $"{command.Data.nameKo}\n[{command.Data.clockCost}]";

            // 클릭 이벤트
            button.onClick.AddListener(() => OnCommandButtonClicked(command));

            _commandButtons.Add(button);
        }

        UpdateCommandButtonStates(actor);
    }

    /// <summary>
    /// Command 버튼 상태 업데이트 (사용 가능 여부)
    /// </summary>
    private void UpdateCommandButtonStates(ProgramModel actor)
    {
        if (actor == null)
            return;

        var commands = actor.GetAllCommands();

        for (int i = 0, iMax = _commandButtons.Count; i < iMax; i++)
        {
            if (i >= commands.Count)
                break;

            var command = commands[i];
            var canUse = actor.IsSufficient(command);

            _commandButtons[i].interactable = canUse;
        }
    }

    private void ClearCommandButtons()
    {
        for (int i = 0; i < _commandButtons.Count; i++)
        {
            Destroy(_commandButtons[i].gameObject);
        }
        _commandButtons.Clear();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제 (Unsubscribe)
        // 메모리 누수 방지를 위해 반드시 해제해야 함
        if (_controller != null)
        {
            _controller.OnPhaseChanged -= HandlePhaseChanged;
            _controller.OnTurnStarted -= HandleTurnStarted;
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Phase 변경 시 호출되는 핸들러
    /// Action<RoundPhase> 시그니처와 일치해야 함
    /// </summary>
    private void HandlePhaseChanged(RoundPhase phase)
    {
        // UI 텍스트 업데이트
        if (_phaseText != null)
            _phaseText.text = phase.ToString();

        // Round/Layer 텍스트 업데이트
        UpdateRoundText();

        // Phase에 따라 UI 패널 표시/숨김
        switch (phase)
        {
            case RoundPhase.LAYER_SELECT:
                ShowLayerSelectUI();
                break;

            case RoundPhase.TURN_ACTION:
                ShowTurnActionUI();
                break;

            case RoundPhase.ENEMY_TURN:
            case RoundPhase.PROCESSING:
            case RoundPhase.IDLE:
                HideAllPanels();
                break;
        }
    }

    /// <summary>
    /// Round/Layer 텍스트 업데이트
    /// _controller에서 현재 라운드/레이어 값을 가져와서 표시
    /// </summary>
    private void UpdateRoundText()
    {
        if (_roundText == null || _controller == null)
            return;

        _roundText.text = $"Round {_controller.CurrentRound} - Layer {_controller.CurrentLayer}";
    }

    /// <summary>
    /// 턴 시작 시 호출되는 핸들러
    /// Action<ProgramModel> 시그니처와 일치해야 함
    /// </summary>
    private void HandleTurnStarted(ProgramModel actor)
    {
        if (actor == null)
        {
            if (_currentActorText != null)
                _currentActorText.text = "No Actor";
            ClearCommandButtons();
            return;
        }

        if (_currentActorText != null)
            _currentActorText.text = $"{actor.GetID()} - Clock: {actor.Combat.RemainingClock}/{actor.Combat.Clock}";

        // 플레이어 턴이면 Command 버튼 생성
        if (actor.Team == Team.PLAYER)
        {
            SetupCommandButtons(actor);
        }
        else
        {
            ClearCommandButtons();
        }
    }

    #endregion

    #region UI Actions

    private void ShowLayerSelectUI()
    {
        if (_layerSelectPanel != null)
            _layerSelectPanel.SetActive(true);
        if (_turnActionPanel != null)
            _turnActionPanel.SetActive(false);
    }

    private void ShowTurnActionUI()
    {
        if (_layerSelectPanel != null)
            _layerSelectPanel.SetActive(false);
        if (_turnActionPanel != null)
            _turnActionPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        if (_layerSelectPanel != null)
            _layerSelectPanel.SetActive(false);
        if (_turnActionPanel != null)
            _turnActionPanel.SetActive(false);
    }

    #endregion

    #region Button Callbacks

    private void OnLayerButtonClicked(int layer)
    {
        // Phase 체크 - LAYER_SELECT 상태에서만 처리
        if (_controller.Phase != RoundPhase.LAYER_SELECT)
            return;

        _controller.OnPlayerSelectLayer(layer);
    }

    private void OnEndTurnClicked()
    {
        // Phase 체크 - TURN_ACTION 상태에서만 처리
        if (_controller.Phase != RoundPhase.TURN_ACTION)
            return;

        _controller.NextTurn();
    }

    private void OnCommandButtonClicked(CommandModel command)
    {
        // Phase 체크 - TURN_ACTION 상태에서만 처리
        if (_controller.Phase != RoundPhase.TURN_ACTION)
            return;

        OnCommandSelected?.Invoke(command);
    }

    /// <summary>
    /// Command 사용 후 버튼 상태 갱신 (외부에서 호출)
    /// </summary>
    public void RefreshCommandButtons()
    {
        var actor = _controller.CurrentActor;
        if (actor != null)
        {
            UpdateCommandButtonStates(actor);

            // Clock 표시 갱신
            if (_currentActorText != null)
                _currentActorText.text = $"{actor.GetID()} - Clock: {actor.Combat.RemainingClock}/{actor.Combat.Clock}";
        }
    }

    #endregion
}
