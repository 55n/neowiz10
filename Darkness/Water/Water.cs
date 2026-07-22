using System.Collections.Generic;

namespace Darkness
{
    public class Water : ISlotContent, IEffectTarget,
        INonBlockingTerrain
    {
        public string Id { get; private set; }
        public string Name { get { return "물"; } }
        public List<ActiveEffect> Effects { get; private set; }

        public Water(string id)
        {
            Id = id;
            Effects = new List<ActiveEffect>();
        }

        public SlotInteractionResult React(PlayerActionContext context)
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
                        ? "물속을 더듬자 벽 쪽으로 이어지는 틈이 손끝에 닿는다."
                        : "물속을 더듬어 보았지만 특별한 것은 없다.");
            }
            else if (context.Action == PlayerActionType.Attack)
            {
                result.Messages.Add(
                    "공격은 물보라만 일으키고 허공을 가른다.");
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
                return;
            }

            if (existing == null)
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
