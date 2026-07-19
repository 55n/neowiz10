using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    public class Room
    {
        public RoomType Type { get; private set; }
        public Dictionary<RoomDirection, RoomEdge> Edges { get; private set; }
        public List<RoomSlot> Slots { get; private set; }

        public Room(RoomType type, RoomSlotContentFactory contentFactory)
        {
            Type = type;
            Edges = new Dictionary<RoomDirection, RoomEdge>();
            Slots = type.Slots
                .Select((slotType, slotIndex) => new RoomSlot(
                    slotType,
                    contentFactory.Create(slotType, slotIndex)))
                .ToList();
        }
    }
    public class RoomEdge
    {
        public Room TargetRoom { get; private set; }
        public bool IsLocked { get; private set; }

        public RoomEdge(Room targetRoom, bool isLocked)
        {
            TargetRoom = targetRoom;
            IsLocked = isLocked;
        }
    }
}
