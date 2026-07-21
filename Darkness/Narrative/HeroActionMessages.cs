namespace Darkness
{
    public static class HeroActionMessages
    {
        public static string Started(
            PlayerActionType action,
            string target)
        {
            switch (action)
            {
                case PlayerActionType.Wait:
                    return "당신은 잠시 움직임을 멈추고 주변의 변화를 살핀다.";
                case PlayerActionType.Talk:
                    return "당신은 " + target + "을(를) 향해 말을 건다.";
                case PlayerActionType.Search:
                    return target == "???"
                        ? "당신은 ???의 정체를 확인하려고 손을 뻗는다."
                        : "당신은 " + target + "을(를) 자세히 살핀다.";
                default:
                    return null;
            }
        }

        public static string Moved(string roomName)
        {
            return "당신은 열린 통로를 따라 [" + roomName + "](으)로 이동했다.";
        }
    }
}
