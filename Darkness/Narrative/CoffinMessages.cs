namespace Darkness
{
    public static class CoffinMessages
    {
        public static string HiddenVoice()
        {
            return "돌 너머에서 목소리가 들리지만 무슨 말인지는 알아들을 수 없다.";
        }

        public static string Discovered()
        {
            return "손끝에 차갑고 거친 돌관이 닿았다.";
        }

        public static string DoesNotMove()
        {
            return "단단한 돌관은 꿈쩍하지 않는다.";
        }

        public static string Opened()
        {
            return "돌관의 뚜껑이 바닥에 떨어졌다.";
        }

        public static string UndeadAwakened()
        {
            return "관 속의 망자가 몸을 일으킨다.";
        }

        public static string LiarUndeadVanished()
        {
            return "망자의 몸이 재처럼 무너져 내리자 관 뒤에 가려져 있던 문이 드러났다.";
        }

        public static string StartledApplied()
        {
            return "당신은 갑작스러운 움직임에 깜짝 놀랐다. " +
                   "[깜짝 놀람] 정확도와 회피율이 10 감소했다.";
        }

        public static string Hint(int slotIndex)
        {
            switch (slotIndex)
            {
                case 0:
                    return "\"자네는 이미 한 번 죽었어야 했다.\"";
                case 1:
                    return "\"굶주린 큰 놈은 냄새로 먹이를 찾는다.\"";
                case 2:
                    return "\"물에 젖으면 냉기에 취약해진다.\"";
                case 3:
                    return "\"소리를 흉내 내는 놈이 있다.\"";
                case 4:
                    return "\"자네가 쉬었던 방은 마석이 불을 밝히고 있었다.\"";
                default:
                    return HiddenVoice();
            }
        }
    }
}
