using System;
using System.Collections.Generic;

namespace Darkness
{
    public class BreathHunterEgg : IDestroyableSlotContent,
        IEffectTarget
    {
        private readonly Func<Monster> createSpiderling;

        public string Name { get { return "숨추적알"; } }
        public int CurrentHealth { get; private set; }
        public int Defense { get { return 0; } }
        public int Evasion { get { return -100; } }
        public List<ActiveEffect> Effects { get; private set; }
        public bool IsDestroyed { get { return CurrentHealth <= 0; } }

        public BreathHunterEgg(Func<Monster> createSpiderling)
        {
            if (createSpiderling == null)
            {
                throw new ArgumentNullException("createSpiderling");
            }

            this.createSpiderling = createSpiderling;
            CurrentHealth = 5;
            Effects = new List<ActiveEffect>();
        }

        public void ReceiveDamage(int damage)
        {
            CurrentHealth = Math.Max(
                0,
                CurrentHealth - Math.Max(0, damage));
        }

        public SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (context.Action == PlayerActionType.Attack)
            {
                Item weapon = context.Actor.GetEquippedItem(
                    EquipmentSlot.Weapon);
                result.Attacks.Add(new AttackContext(
                    context.Actor,
                    this,
                    context.Actor.Attack,
                    context.Actor.Accuracy,
                    Evasion,
                    weapon == null
                        ? AttackDeliveryType.Natural
                        : AttackDeliveryType.EquippedWeapon,
                    weapon,
                    weapon == null ? 0 : 1));
            }
            else if (context.Action == PlayerActionType.Search)
            {
                result.Messages.Add(
                    "팽팽한 거미줄 사이로 질긴 알껍질이 만져진다.");
            }
            else if (context.Action == PlayerActionType.Talk)
            {
                result.Messages.Add(
                    "푸른 알은 아무 대답 없이 희미한 빛만 흘린다.");
            }

            return result;
        }

        public SlotDestructionResult ResolveDestruction(
            Room room,
            RoomSlot slot)
        {
            SlotDestructionResult result =
                new SlotDestructionResult();
            result.Messages.Add(
                "질긴 알껍질이 갈라지며 안쪽으로 무너졌다.");
            result.Messages.Add(
                "깨진 알 속에서 작은 거미 한 마리가 기어 나온다.");
            result.ContentChanges.Add(
                SlotContentChangeRequest.Place(
                    slot,
                    createSpiderling()));
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
