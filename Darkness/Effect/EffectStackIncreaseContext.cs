using System.Collections.Generic;

namespace Darkness
{
    public class EffectStackIncreaseContext
    {
        public object Source { get; private set; }
        public IEffectTarget Target { get; private set; }
        public ActiveEffect Effect { get; private set; }
        public int PreviousStackCount { get; private set; }
        public int CurrentStackCount { get; private set; }
        public List<DamageContext> Damages { get; private set; }

        public EffectStackIncreaseContext(
            object source,
            IEffectTarget target,
            ActiveEffect effect,
            int previousStackCount,
            int currentStackCount)
        {
            Source = source;
            Target = target;
            Effect = effect;
            PreviousStackCount = previousStackCount;
            CurrentStackCount = currentStackCount;
            Damages = new List<DamageContext>();
        }

        public void AddDamage(
            int amount,
            bool ignoresDefense = false)
        {
            IDamageable damageable = Target as IDamageable;
            if (damageable == null)
            {
                return;
            }

            Damages.Add(new DamageContext(
                Effect,
                damageable,
                amount,
                ignoresDefense));
        }
    }
}
