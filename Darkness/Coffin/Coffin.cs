namespace Darkness
{
    public class Coffin : ISlotContent
    {
        public string Name { get { return "돌관"; } }

        private readonly string hint;
        private readonly Monster occupant;

        public Coffin(string hint, Monster occupant)
        {
            this.hint = hint;
            this.occupant = occupant;
        }

        public SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (context == null || context.TargetSlot == null)
            {
                return result;
            }

            if (context.Action == PlayerActionType.Talk)
            {
                result.Messages.Add(
                    context.TargetSlot.State == SlotState.REVEALED
                        ? hint
                        : CoffinMessages.HiddenVoice());
                return result;
            }

            if (context.Action == PlayerActionType.Search)
            {
                if (context.TargetSlot.State == SlotState.UNREVEALED)
                {
                    result.Messages.Add(
                        CoffinMessages.Discovered());
                    return result;
                }

                Open(context, result);
                return result;
            }

            if (context.Action == PlayerActionType.Attack ||
                context.Action == PlayerActionType.ThrowItem)
            {
                result.Messages.Add(
                    CoffinMessages.DoesNotMove());
            }

            return result;
        }

        private void Open(
            PlayerActionContext context,
            SlotInteractionResult result)
        {
            result.LoudEventOccurred = true;
            result.Messages.Add(CoffinMessages.Opened());
            result.Messages.Add(CoffinMessages.UndeadAwakened());
            result.SlotContentChanges.Add(
                SlotContentChangeRequest.Replace(
                    context.TargetSlot,
                    this,
                    occupant));

            if (context.TargetSlot.Type.HasDoor)
            {
                result.Messages.Add(
                    CoffinMessages.LiarUndeadVanished());
                result.SlotContentChanges.Add(
                    SlotContentChangeRequest.Remove(
                        context.TargetSlot,
                        occupant));
                return;
            }

            ApplyStartled(context.Actor);
            result.Messages.Add(CoffinMessages.StartledApplied());
        }

        private static void ApplyStartled(Hero hero)
        {
            if (hero == null)
            {
                return;
            }

            hero.RemoveEffect("startled");
            hero.ApplyEffect(
                new ActiveEffectFactory().Create("startled"));
        }
    }
}
