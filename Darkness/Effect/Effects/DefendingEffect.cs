namespace Darkness
{
    public class DefendingEffect : ActiveEffect
    {
        private const int DefenseBonus = 3;

        public DefendingEffect(EffectType type)
            : base(type)
        {
        }

        public override int GetDefenseBonus()
        {
            return DefenseBonus;
        }
    }
}
