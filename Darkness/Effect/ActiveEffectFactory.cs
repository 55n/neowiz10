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
                case "weapon_empowerment":
                    return new WeaponEmpowermentEffect(effectType);
                default:
                    return null;
            }
        }
    }
}
