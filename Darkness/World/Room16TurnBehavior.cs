namespace Darkness
{
    public class Room16TurnBehavior : IRoomTurnBehavior
    {
        private readonly RoomSlotContentFactory contentFactory;
        private readonly BreathHunterBody body;
        private bool bossPhaseStarted;
        private bool bossDefeatResolved;

        public Room16TurnBehavior(
            RoomSlotContentFactory contentFactory)
        {
            this.contentFactory = contentFactory;
            body = new BreathHunterBody();
        }

        public SlotInteractionResult Act(
            Room room,
            Hero hero,
            PlayerActionContext playerAction)
        {
            if (!bossPhaseStarted)
            {
                return StartBossPhase(room);
            }

            if (!body.IsAbdomenAlive)
            {
                return ResolveBossDefeat(room);
            }

            body.AdvanceTurn();
            return new SlotInteractionResult();
        }

        private SlotInteractionResult StartBossPhase(Room room)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            for (int i = 0; i < room.Slots.Count; i++)
            {
                RoomSlot slot = room.Slots[i];
                Monster web = slot.Content as Monster;
                if (web != null && web.Id == "ceiling_web")
                {
                    result.SlotContentChanges.Add(
                        SlotContentChangeRequest.Remove(slot, web));
                }
            }

            for (int i = 0; i < room.Slots.Count; i++)
            {
                RoomSlot slot = room.Slots[i];
                Monster part = contentFactory.CreateRoom16Part(
                    i,
                    body);
                result.SlotContentChanges.Add(
                    SlotContentChangeRequest.Place(slot, part));
                result.SlotStateChanges.Add(
                    SlotStateChangeRequest.Reveal(slot));
            }

            result.Messages.Add(
                "거미줄이 걷힌 천장에서 거대한 다리와 몸통이 차례로 내려온다.");
            bossPhaseStarted = true;
            return result;
        }

        private SlotInteractionResult ResolveBossDefeat(Room room)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (bossDefeatResolved)
            {
                return result;
            }

            foreach (RoomSlot slot in room.Slots)
            {
                Monster part = slot.Content as Monster;
                if (part != null &&
                    part.Id.StartsWith("breath_hunter_part_"))
                {
                    result.SlotContentChanges.Add(
                        SlotContentChangeRequest.Remove(slot, part));
                }
            }

            RoomSlot doorSlot = room.Slots[2];
            result.SlotStateChanges.Add(
                SlotStateChangeRequest.Reveal(doorSlot));
            result.Messages.Add(
                "거대한 배가 무너지자 남은 다리와 송곳니도 힘없이 바닥에 떨어졌다.");
            bossDefeatResolved = true;
            return result;
        }
    }
}
