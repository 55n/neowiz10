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
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (context == null)
            {
                return result;
            }

            if (context.Action == PlayerActionType.Talk)
            {
                result.Messages.Add(
                    ExplorationMessages.NoResponse());
            }
            else if (context.Action == PlayerActionType.Search)
            {
                result.Messages.Add(
                    context.TargetSlot != null &&
                    context.TargetSlot.Type.HasDoor
                        ? "모래를 헤집자 벽 아래로 이어지는 틈이 드러난다."
                        : "모래를 헤집어 보았지만 특별한 것은 없다.");
            }
            else if (context.Action == PlayerActionType.Attack)
            {
                result.Messages.Add(
                    "모래만 사방으로 흩어진다.");
            }

            return result;
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
