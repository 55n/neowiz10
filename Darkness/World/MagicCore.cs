using System;
using System.Collections.Generic;

namespace Darkness
{
    public class MagicCore : IDestroyableSlotContent
    {
        private static readonly HashSet<string> DoorClingerPartIds =
            new HashSet<string>
            {
                "door_clinger_tentacle",
                "door_clinger_leg",
                "door_clinger_antenna",
                "door_clinger_torso",
                "door_clinger_claw"
            };

        private readonly Inventory drops;

        public string Name { get { return "마핵"; } }
        public int CurrentHealth { get; private set; }
        public int Defense { get; private set; }
        public int Evasion { get { return 0; } }
        public List<ActiveEffect> Effects { get; private set; }
        public bool IsDestroyed { get { return CurrentHealth <= 0; } }

        public MagicCore(
            int health,
            int defense,
            Inventory drops)
        {
            CurrentHealth = Math.Max(1, health);
            Defense = Math.Max(0, defense);
            this.drops = drops ?? new Inventory(0);
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
            if (context == null || context.Actor == null ||
                context.Action != PlayerActionType.Attack)
            {
                return result;
            }

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
            return result;
        }

        public SlotDestructionResult ResolveDestruction(
            Room room,
            RoomSlot slot)
        {
            SlotDestructionResult result =
                new SlotDestructionResult(drops);
            result.Messages.Add(
                "마핵이 산산이 부서지자 남은 문붙이의 부위가 함께 말라붙어 소멸했다.");
            result.Messages.Add(
                "파편 사이로 마석 세 개와 [잊혀진 창] 한 자루가 바닥에 떨어졌다.");

            if (room == null)
            {
                return result;
            }

            foreach (RoomSlot roomSlot in room.Slots)
            {
                Monster monster = roomSlot.Content as Monster;
                if (monster == null ||
                    !DoorClingerPartIds.Contains(monster.Id))
                {
                    continue;
                }

                result.ContentChanges.Add(
                    SlotContentChangeRequest.Remove(
                        roomSlot,
                        monster));
            }

            return result;
        }
    }
}
