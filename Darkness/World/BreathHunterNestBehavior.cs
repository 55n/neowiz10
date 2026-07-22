namespace Darkness
{
    public class BreathHunterNestBehavior : IRoomTurnBehavior
    {
        private const int SafeTurnCount = 5;
        private int elapsedTurns;

        public SlotInteractionResult Act(
            Room room,
            Hero hero,
            PlayerActionContext playerAction)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (hero == null || !hero.CanAct)
            {
                return result;
            }

            elapsedTurns++;
            if (elapsedTurns <= SafeTurnCount)
            {
                string warning = GetWarning(elapsedTurns);
                if (!string.IsNullOrEmpty(warning))
                {
                    result.Messages.Add(warning);
                }

                return result;
            }

            result.Messages.Add(
                "바로 등 뒤에서 정체를 알 수 없는 존재가 숨을 들이마신다.");
            result.Messages.Add(
                "알 수 없는 존재가 당신을 덮쳤다.");
            result.Damages.Add(new DamageContext(
                this,
                hero,
                9999,
                true));
            return result;
        }

        private static string GetWarning(int elapsedTurns)
        {
            switch (elapsedTurns)
            {
                case 1:
                    return "멀리서 무언가가 바닥을 긁는 소리가 들린다.";
                case 2:
                    return "규칙적인 마찰음이 조금 더 가까워졌다.";
                case 3:
                    return "어둠 속에서 무겁고 느린 호흡이 들린다.";
                case 4:
                    return "목덜미에 축축한 바람이 스친다.";
                case 5:
                    return "바로 뒤에서 가느다란 다리가 바닥을 짚는다.";
                default:
                    return null;
            }
        }
    }
}
