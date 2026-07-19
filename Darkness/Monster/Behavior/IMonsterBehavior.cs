namespace Darkness
{
    public interface IMonsterBehavior
    {
        MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception);
    }
}
