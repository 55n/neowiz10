namespace Darkness
{
    public class SlotContentChangeRequest
    {
        public SlotContentChangeType ChangeType { get; private set; }
        public RoomSlot SourceSlot { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public ISlotContent ExpectedContent { get; private set; }
        public ISlotContent NewContent { get; private set; }

        private SlotContentChangeRequest(
            SlotContentChangeType changeType,
            RoomSlot sourceSlot,
            RoomSlot targetSlot,
            ISlotContent expectedContent,
            ISlotContent newContent)
        {
            ChangeType = changeType;
            SourceSlot = sourceSlot;
            TargetSlot = targetSlot;
            ExpectedContent = expectedContent;
            NewContent = newContent;
        }

        public static SlotContentChangeRequest Place(
            RoomSlot targetSlot,
            ISlotContent content)
        {
            return new SlotContentChangeRequest(
                SlotContentChangeType.Place,
                null,
                targetSlot,
                null,
                content);
        }

        public static SlotContentChangeRequest Remove(
            RoomSlot sourceSlot,
            ISlotContent expectedContent)
        {
            return new SlotContentChangeRequest(
                SlotContentChangeType.Remove,
                sourceSlot,
                null,
                expectedContent,
                null);
        }

        public static SlotContentChangeRequest Move(
            RoomSlot sourceSlot,
            RoomSlot targetSlot,
            ISlotContent expectedContent)
        {
            return new SlotContentChangeRequest(
                SlotContentChangeType.Move,
                sourceSlot,
                targetSlot,
                expectedContent,
                null);
        }

        public static SlotContentChangeRequest MoveReplacing(
            RoomSlot sourceSlot,
            RoomSlot targetSlot,
            ISlotContent expectedContent,
            ISlotContent sourceReplacement)
        {
            return new SlotContentChangeRequest(
                SlotContentChangeType.Move,
                sourceSlot,
                targetSlot,
                expectedContent,
                sourceReplacement);
        }

        public static SlotContentChangeRequest Replace(
            RoomSlot targetSlot,
            ISlotContent expectedContent,
            ISlotContent newContent)
        {
            return new SlotContentChangeRequest(
                SlotContentChangeType.Replace,
                targetSlot,
                targetSlot,
                expectedContent,
                newContent);
        }
    }
}
