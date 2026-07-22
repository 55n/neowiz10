namespace Darkness
{
    public class MoveInterceptionResult
    {
        public bool MovementCanceled { get; private set; }
        public bool ConsumesTurn { get; private set; }
        public SlotInteractionResult Interaction { get; private set; }

        private MoveInterceptionResult(
            bool movementCanceled,
            SlotInteractionResult interaction,
            bool consumesTurn)
        {
            MovementCanceled = movementCanceled;
            ConsumesTurn = consumesTurn;
            Interaction = interaction ?? new SlotInteractionResult();
        }

        public static MoveInterceptionResult Continue()
        {
            return new MoveInterceptionResult(false, null, false);
        }

        public static MoveInterceptionResult Cancel(
            SlotInteractionResult interaction,
            bool consumesTurn = false)
        {
            return new MoveInterceptionResult(
                true,
                interaction,
                consumesTurn);
        }
    }
}
