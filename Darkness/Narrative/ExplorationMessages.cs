namespace Darkness
{
    public static class ExplorationMessages
    {
        public static string[] FirstRoomScript()
        {
            return new[]
            {
                "아무것도 보이지 않는다",
                "장비가 어딘가로 날아간 것 같다",
                "소지품이 부서진 것 같다",
                "당신은 숨을 죽이고 주변을 살피려고 했다",
                "어둠에 시야가 적응하도록 기다렸다",
                "여전히 아무것도 보이지 않는다",
                "당신은 손을 더듬어 바닥을 살핀다",
                "무언가가 손에 잡혔다!"
            };
        }

        public static string[] DefaultRoomEnterMessage()
        {
            return new[]
            {
                "공간에는 적막이 맴돌고 있다",
                "당신은 신중하게 탐색을 시작했다"
            };
        }

        public static string[] SecondRoomEnterMessages()
        {
            return new[]
            {
                "기척이 느껴진다",
                "무언가가 긁는 소리를 낸다"
            };
        }

        public static string DoorBehindGoblinFound()
        {
            return "고블린이 쓰러지자 그 뒤에 가려져 있던 문이 드러났다";
        }

        public static string Door()
        {
            return "문";
        }

        public static string EmptySlotFound()
        {
            return "아무것도 없다";
        }

        public static string DoorFound()
        {
            return "문을 발견했다";
        }

        public static string TreasureChestFound()
        {
            return "상자를 발견했다.";
        }

        public static string TreasureChestEmpty()
        {
            return "보물 상자는 비어 있다.";
        }

        public static string NoResponse()
        {
            return "아무 반응이 없다.";
        }

        public static string PileEmpty()
        {
            return "물건 더미는 비어 있다.";
        }

        public static string NothingHappened()
        {
            return "아무 일도 일어나지 않는다.";
        }
    }
}
