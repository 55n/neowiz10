using System;

namespace Darkness
{
    public class DamageContext
    {
        public object Source { get; private set; }
        public IDamageable Target { get; private set; }
        public int BaseDamage { get; private set; }
        public int FinalDamage { get; set; }
        public bool IgnoresDefense { get; private set; }

        public DamageContext(
            object source,
            IDamageable target,
            int baseDamage,
            bool ignoresDefense = false)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            Source = source;
            Target = target;
            BaseDamage = Math.Max(0, baseDamage);
            FinalDamage = BaseDamage;
            IgnoresDefense = ignoresDefense;
        }
    }
}
