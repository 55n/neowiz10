namespace Darkness
{
    public class BoundPrey : ISlotContent
    {
        public string Name { get; private set; }
        private readonly string searchMessage;
        private readonly string talkMessage;

        public BoundPrey(
            string name,
            string searchMessage,
            string talkMessage)
        {
            Name = name;
            this.searchMessage = searchMessage;
            this.talkMessage = talkMessage;
        }

        public SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (context.Action == PlayerActionType.Search)
            {
                result.Messages.Add(searchMessage);
            }
            else if (context.Action == PlayerActionType.Talk)
            {
                result.Messages.Add(talkMessage);
            }
            else if (context.Action == PlayerActionType.Attack)
            {
                result.Messages.Add(
                    "촘촘한 거미줄이 무기를 휘감아 묶인 먹잇감에게 공격이 닿지 않는다.");
            }
            else
            {
                result.Messages.Add(
                    "거미줄에 묶인 먹잇감은 제대로 움직이지 못한다.");
            }

            return result;
        }
    }
}
