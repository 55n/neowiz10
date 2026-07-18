namespace Darkness
{
    public class RoomEdgeType
    {
        public string TargetRoomId { get; private set; }
        public bool InitiallyLocked { get; private set; }

        public RoomEdgeType(
            string targetRoomId,
            bool initiallyLocked)
        {
            TargetRoomId = targetRoomId;
            InitiallyLocked = initiallyLocked;
        }
    }
}
