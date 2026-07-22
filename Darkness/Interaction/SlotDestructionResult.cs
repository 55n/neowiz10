using System.Collections.Generic;

namespace Darkness
{
    public class SlotDestructionResult
    {
        public Inventory Drops { get; private set; }
        public List<string> Messages { get; private set; }
        public List<SlotContentChangeRequest> ContentChanges
        {
            get;
            private set;
        }

        public SlotDestructionResult(Inventory drops = null)
        {
            Drops = drops ?? new Inventory(0);
            Messages = new List<string>();
            ContentChanges =
                new List<SlotContentChangeRequest>();
        }
    }
}
