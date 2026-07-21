using System;
using System.Collections.Generic;

namespace Darkness
{
    public class RoomType
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<string> EnterMessages { get; private set; }
        public Dictionary<RoomDirection, RoomEdgeType> Edges { get; private set; }
        public List<RoomSlotType> Slots { get; private set; }
        public bool RevealsAllSlotsOnEntry { get; private set; }
        public bool ConsumesTurnOnFirstEntry { get; private set; }
        

        public RoomType(
            string id,
            string name,
            string description,
            List<string> enterMessages,
            Dictionary<RoomDirection, RoomEdgeType> edges,
            List<RoomSlotType> slots,
            bool revealsAllSlotsOnEntry = false,
            bool consumesTurnOnFirstEntry = false)
        {
            Id = id;
            Name = name;
            Description = description;
            EnterMessages = enterMessages;
            Edges = edges;
            Slots = slots;
            RevealsAllSlotsOnEntry = revealsAllSlotsOnEntry;
            ConsumesTurnOnFirstEntry = consumesTurnOnFirstEntry;
        }
    }
}
