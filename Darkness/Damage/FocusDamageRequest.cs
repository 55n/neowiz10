using System;

namespace Darkness
{
    public class FocusDamageRequest
    {
        public IDamageable Source { get; private set; }
        public ISkillUser Target { get; private set; }
        public int Amount { get; private set; }

        public FocusDamageRequest(
            IDamageable source,
            ISkillUser target,
            int amount)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            Source = source;
            Target = target;
            Amount = Math.Max(0, amount);
        }
    }
}
