namespace Darkness
{
    public class MoveInterceptionResult
    {
        public bool MovementCanceled { get; private set; }
        public SlotInteractionResult Interaction { get; private set; }

        private MoveInterceptionResult(
            bool movementCanceled,
            SlotInteractionResult interaction)
        {
            MovementCanceled = movementCanceled;
            Interaction = interaction ?? new SlotInteractionResult();
        }

        public static MoveInterceptionResult Continue()
        {
            return new MoveInterceptionResult(false, null);
        }

        public static MoveInterceptionResult Cancel(
            SlotInteractionResult interaction)
        {
            return new MoveInterceptionResult(true, interaction);
        }
    }
}
