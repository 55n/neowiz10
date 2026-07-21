using System.Collections.Generic;

namespace Darkness
{
    public class EffectTargetResolver
    {
        public List<object> Resolve(
            EffectTarget target,
            EffectContext context)
        {
            if (context == null)
            {
                return new List<object>();
            }

            if (target == EffectTarget.Self)
            {
                return new List<object> { context.Source };
            }

            if (target == EffectTarget.Target)
            {
                return context.SelectedTargets.Count == 0
                    ? new List<object>()
                    : new List<object>
                    {
                        context.SelectedTargets[0]
                    };
            }

            if (target == EffectTarget.AllTargets)
            {
                return new List<object>(context.SelectedTargets);
            }

            if (target == EffectTarget.AllRoomSlots)
            {
                return ResolveRoomContents(context.Room);
            }

            if (target == EffectTarget.AllRoomOccupants)
            {
                return ResolveRoomOccupants(context);
            }

            if (target == EffectTarget.EquippedWeapon)
            {
                IEquipmentUser equipmentUser =
                    context.Source as IEquipmentUser;
                Item weapon = equipmentUser == null
                    ? null
                    : equipmentUser.GetEquippedItem(
                        EquipmentSlot.Weapon);
                return weapon == null
                    ? new List<object>()
                    : new List<object> { weapon };
            }

            return new List<object>();
        }

        private static List<object> ResolveRoomContents(Room room)
        {
            List<object> targets = new List<object>();
            if (room == null)
            {
                return targets;
            }

            foreach (RoomSlot slot in room.Slots)
            {
                if (slot.Content is IDamageable)
                {
                    targets.Add(slot.Content);
                }
            }

            return targets;
        }

        private static List<object> ResolveRoomOccupants(
            EffectContext context)
        {
            List<object> targets = new List<object>();
            foreach (object selectedTarget in context.SelectedTargets)
            {
                IEffectTarget occupant = selectedTarget as IEffectTarget;
                if (occupant != null && !targets.Contains(occupant))
                {
                    targets.Add(occupant);
                }
            }

            if (context.Room == null)
            {
                return targets;
            }

            foreach (RoomSlot slot in context.Room.Slots)
            {
                Monster occupant = slot.Content as Monster;
                if (occupant != null && !targets.Contains(occupant))
                {
                    targets.Add(occupant);
                }
            }

            return targets;
        }
    }
}
