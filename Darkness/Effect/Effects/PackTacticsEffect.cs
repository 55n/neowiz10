namespace Darkness
{
    public class PackTacticsEffect : ActiveEffect
    {
        private const int BonusPerAlly = 1;

        private readonly Monster owner;
        private readonly WolfPack pack;

        public PackTacticsEffect(
            EffectType type,
            Monster owner,
            WolfPack pack)
            : base(type, pack)
        {
            this.owner = owner;
            this.pack = pack;
        }

        public override int GetAttackBonus()
        {
            return GetCurrentBonus();
        }

        public override int GetDefenseBonus()
        {
            return GetCurrentBonus();
        }

        private int GetCurrentBonus()
        {
            return owner == null || pack == null
                ? 0
                : pack.GetLivingAllyCount(owner) *
                  BonusPerAlly;
        }
    }
}
