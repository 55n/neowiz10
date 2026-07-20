namespace Darkness
{
    public class WeaponEmpowermentEffect : ActiveEffect
    {
        private const int AttackMultiplier = 2;

        public WeaponEmpowermentEffect(EffectType type)
            : base(type)
        {
        }

        public override int ModifyWeaponAttackBonus(
            int weaponAttackBonus)
        {
            return weaponAttackBonus * AttackMultiplier;
        }
    }
}
