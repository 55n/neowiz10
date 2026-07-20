using System;

namespace Darkness
{
    public class EquipmentDurabilityRequest
    {
        public IEquipmentUser Owner { get; private set; }
        public EquipmentSlot Slot { get; private set; }
        public int Durability { get; private set; }

        public EquipmentDurabilityRequest(
            IEquipmentUser owner,
            EquipmentSlot slot,
            int durability)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            Owner = owner;
            Slot = slot;
            Durability = Math.Max(0, durability);
        }
    }
}
