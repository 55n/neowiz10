namespace Darkness
{
    public class EffectApplication
    {
        public string EffectId { get; private set; }
        public EffectTarget Target { get; private set; }
        public int ApplyChance { get; private set; }

        public EffectApplication(
            string effectId,
            EffectTarget target,
            int applyChance)
        {
            EffectId = effectId;
            Target = target;
            ApplyChance = applyChance;
        }
    }
}
