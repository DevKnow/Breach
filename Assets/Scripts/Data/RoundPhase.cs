public enum RoundPhase
{
    IDLE,           // 전투 시작 전
    LAYER_SELECT,   // 플레이어 레이어 선택 대기
    TURN_ACTION,    // 플레이어 행동 대기
    ENEMY_TURN,     // 적 턴 (자동)
    PROCESSING      // 처리 중
}
