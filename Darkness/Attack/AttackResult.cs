namespace Darkness
{
    public class AttackResult
    {
        public bool IsHit { get; private set; }
        public int Damage { get; private set; }

        public AttackResult(bool isHit, int damage)
        {
            IsHit = isHit;
            Damage = damage;
        }
    }
}
