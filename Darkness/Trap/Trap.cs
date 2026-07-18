using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Trap : IActionReactor, IRoomObject
    {
        public TrapType Type { get; private set; }
        public int SlotIndex { get; private set; }
        public bool IsActive { get; private set; }
        public string Id { get { return Type.Id; } }
        public string Name { get { return Type.Name; } }
        public RoomObjectType ObjectType { get { return RoomObjectType.Trap; } }

        public Trap(
            TrapType type,
            int slotIndex)
        {
            Type = type;
            SlotIndex = slotIndex;
            IsActive = true;
        }

        protected Trap(TrapType type, int slotIndex, string expectedTypeId)
            : this(type, slotIndex)
        {
            if (type == null || type.Id != expectedTypeId)
            {
                throw new ArgumentException("Trap type does not match concrete trap class.", "type");
            }
        }

        public virtual void Evaluate(
            EncounterActionContext context,
            IList<ReactionResult> results)
        {
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
