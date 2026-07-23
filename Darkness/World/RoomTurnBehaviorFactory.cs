namespace Darkness
{
    public class RoomTurnBehaviorFactory
    {
        public IRoomTurnBehavior Create(
            string roomId,
            RoomSlotContentFactory contentFactory = null)
        {
            if (roomId == "room-0")
            {
                return new Room0DeveloperSwordBehavior();
            }

            if (roomId == "room-15")
            {
                return new BreathHunterNestBehavior();
            }

            if (roomId == "room-16" && contentFactory != null)
            {
                return new Room16TurnBehavior(contentFactory);
            }

            return null;
        }
    }
}
