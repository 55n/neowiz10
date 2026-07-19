using System.Collections.Generic;

namespace Darkness
{
    public class TurnResult
    {
        public List<string> Messages { get; private set; }
        public HashSet<int> ChangedSlotIndexes { get; private set; }
        public bool TurnCompleted { get; set; }
        public bool RoomChanged { get; set; }
        public bool HeroDied { get; set; }
        public int TurnNumber { get; set; }

        public TurnResult()
        {
            Messages = new List<string>();
            ChangedSlotIndexes = new HashSet<int>();
        }
    }
}
