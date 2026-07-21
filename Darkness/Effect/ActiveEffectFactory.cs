namespace Darkness
{
    public class ActiveEffectFactory
    {
        private readonly EffectData effectData;

        public ActiveEffectFactory()
        {
            effectData = new EffectData();
        }

        public ActiveEffect Create(string effectId)
        {
            return Create(effectId, null);
        }

        public ActiveEffect Create(string effectId, object source)
        {
            EffectType effectType;
            if (string.IsNullOrEmpty(effectId) ||
                !effectData.EffectTypes.TryGetValue(
                    effectId,
                    out effectType))
            {
                return null;
            }

            switch (effectId)
            {
                case "guardian_blessing":
                    return new GuardianBlessingEffect(effectType);
                case "hasty":
                    return new HastyEffect(effectType);
                case "perfect_focus":
                    return new PerfectFocusEffect(effectType);
                case "defending":
                    return new DefendingEffect(effectType);
                case "trap_misfire":
                    return new TrapMisfireEffect(effectType);
                case "weapon_empowerment":
                    return new WeaponEmpowermentEffect(effectType);
                case "startled":
                    return new StartledEffect(effectType);
                case "poison":
                    return new PoisonEffect(effectType, source);
                case "wet":
                    return new WetEffect(effectType);
                case "frozen":
                    return new FrozenEffect(effectType);
                default:
                    return null;
            }
        }
    }
}
