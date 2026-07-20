using System.Collections.Generic;

namespace Darkness
{
    public interface IDamageable
    {
        string Name { get; }
        int CurrentHealth { get; }
        int Defense { get; }
        int Evasion { get; }
        List<ActiveEffect> Effects { get; }

        void ReceiveDamage(int damage);
    }
}
