using System.Collections.Generic;

namespace Darkness
{
    public interface IEffectTarget
    {
        string Name { get; }
        List<ActiveEffect> Effects { get; }

        void ApplyEffect(ActiveEffect effect);
        void RemoveEffect(string effectId);
    }
}
