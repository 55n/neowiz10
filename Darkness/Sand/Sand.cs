using System.Collections.Generic;

namespace Darkness
{
    public class Sand : ISlotContent, IEffectTarget,
        INonBlockingTerrain, ISlotAppearance
    {
        public string Id { get; private set; }
        public string Name { get { return "모래"; } }
        public string SlotDisplayName { get { return "모래"; } }
        public List<ActiveEffect> Effects { get; private set; }

        public Sand(string id)
        {
            Id = id;
            Effects = new List<ActiveEffect>();
        }

        public SlotInteractionResult React(
            PlayerActionContext context)
        {
            return new SlotInteractionResult();
        }

        public void ApplyEffect(ActiveEffect effect)
        {
            if (effect == null)
            {
                return;
            }

            ActiveEffect existing = Effects.Find(active =>
                active.Type.Id == effect.Type.Id);
            if (existing != null && existing.Type.IsStackable)
            {
                existing.AddStack();
            }
            else if (existing == null)
            {
                Effects.Add(effect);
            }
        }

        public void RemoveEffect(string effectId)
        {
            Effects.RemoveAll(effect => effect.Type.Id == effectId);
        }
    }
}
