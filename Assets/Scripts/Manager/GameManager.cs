using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main game manager
/// Handles initialization and core game logic
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private BattleView _battleView;

    public BattleManager BattleManager => _battleManager;

    private void Awake()
    {
        // Load all data at game start
        DataLoader.LoadAll();
    }

    private void Start()
    {
        // View 초기화 - BattleManager의 RoundController와 연결
        if (_battleView != null && _battleManager != null)
        {
            _battleView.Initialize(_battleManager.RoundController, _battleManager);
            _battleView.OnCommandSelected += HandleCommandSelected;
        }

        // 전투 결과 이벤트 구독
        if (_battleManager != null)
        {
            _battleManager.OnBattleVictory += HandleVictory;
            _battleManager.OnBattleDefeat += HandleDefeat;
        }
    }

    private void OnDestroy()
    {
        if (_battleManager != null)
        {
            _battleManager.OnBattleVictory -= HandleVictory;
            _battleManager.OnBattleDefeat -= HandleDefeat;
        }

        if (_battleView != null)
        {
            _battleView.OnCommandSelected -= HandleCommandSelected;
        }
    }

    #region Battle Events

    private void HandleVictory()
    {
        Debug.Log("[Game] Victory!");
        // TODO: 승리 처리 (보상, 다음 스테이지 등)
    }

    private void HandleDefeat()
    {
        Debug.Log("[Game] Defeat...");
        // TODO: 패배 처리 (게임 오버, 재시작 등)
    }

    private void HandleCommandSelected(CommandModel command)
    {
        Debug.Log($"[Game] Command selected: {command.Data.nameKo}");

        // 일단 첫 번째 적을 타겟으로 설정 (나중에 타겟 선택 UI 추가)
        var enemies = _battleManager.GetAliveEnemies();
        if (enemies.Count == 0)
            return;

        var target = enemies[0];
        bool success = _battleManager.TryPlayerAttack(target, command);

        if (success)
        {
            Debug.Log($"[Game] Attack executed: {command.Data.nameKo} -> {target.GetID()}");
            _battleView.RefreshCommandButtons();
        }
        else
        {
            Debug.Log($"[Game] Attack failed (insufficient clock or invalid phase)");
        }
    }

    #endregion

    #region Test Methods

    private void Update()
    {
        // 테스트용 키보드 입력
        if (_battleManager == null)
            return;

        var round = _battleManager.RoundController;

        // LAYER_SELECT 상태에서 1~6 키로 레이어 선택
        if (round.Phase == RoundPhase.LAYER_SELECT)
        {
            for (int i = 1; i <= 6; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i - 1))
                {
                    Debug.Log($"[Test] Player selected layer {i}");
                    round.OnPlayerSelectLayer(i);
                    break;
                }
            }
        }
        // TURN_ACTION 상태에서 Space로 턴 종료
        else if (round.Phase == RoundPhase.TURN_ACTION)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[Test] Player ended turn");
                round.NextTurn();
            }
        }
    }

    /// <summary>
    /// 테스트용 전투 시작
    /// </summary>
    [Button("Start Test Battle")]
    public void StartTestBattle()
    {
        var playerAgents = new List<ProgramModel>();
        var enemyAgents = new List<ProgramModel>();

        // 테스트용 플레이어 생성
        var playerData = DataLoader.GetProgramData("proc_ghost");
        if (playerData != null)
        {
            var player = new PlayerProgramModel(playerData);
            // 테스트용 모듈 장착
            player.InstallModule("mod_worm");
            playerAgents.Add(player);
        }

        // 테스트용 적 생성
        var enemyData = DataLoader.GetEnemyData("enemy_firewall");
        if (enemyData != null)
        {
            enemyAgents.Add(new EnemyProgramModel(enemyData, 1));
        }

        if (playerAgents.Count > 0 && enemyAgents.Count > 0)
        {
            // 테스트: 섹터 1 = 레이어 3개
            int testSector = 1;
            int maxLayer = GetMaxLayerBySector(testSector);

            if (_battleView != null)
                _battleView.SetupLayerButtons(maxLayer);

            _battleManager.StartBattle(playerAgents, enemyAgents);
        }
        else
        {
            Debug.LogWarning("[Game] Test data not found!");
        }
    }

    /// <summary>
    /// 섹터별 최대 레이어 수
    /// </summary>
    private int GetMaxLayerBySector(int sector)
    {
        return sector switch
        {
            1 or 2 => 3,
            3 or 4 => 4,
            5 or 6 => 5,
            7 => 6,
            _ => 3
        };
    }

    #endregion
}
