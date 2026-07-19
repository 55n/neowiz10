using System.Collections.Generic;

namespace Darkness
{
    public interface IDamageable
    {
        int CurrentHealth { get; }
        int Evasion { get; }
        List<ActiveEffect> Effects { get; }

        void ReceiveDamage(int damage);
    }
}
