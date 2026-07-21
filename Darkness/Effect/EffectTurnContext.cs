using System.Collections.Generic;

namespace Darkness
{
    public class EffectTurnContext
    {
        public Room Room { get; private set; }
        public IDamageable Target { get; private set; }
        public List<DamageContext> Damages { get; private set; }
        public List<ActiveEffect> EffectsToRemove { get; private set; }

        public EffectTurnContext(Room room, IDamageable target)
        {
            Room = room;
            Target = target;
            Damages = new List<DamageContext>();
            EffectsToRemove = new List<ActiveEffect>();
        }
    }
}
