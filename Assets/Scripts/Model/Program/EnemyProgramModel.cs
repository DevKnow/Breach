public class EnemyProgramModel : ProgramModel
{
    public EnemyProgramModel(EnemyData data, int sectorNum) : base(data, Team.ENEMY)
    {
        // TODO: modules 초기화
    }

    public void DecideEnemyLayer()
    {
        // TODO: AI logic for layer selection
        PrepareRound(GameConstants.MinLayer);
    }
}
