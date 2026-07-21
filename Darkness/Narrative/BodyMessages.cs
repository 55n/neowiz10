namespace Darkness
{
    public static class BodyMessages
    {
        public static string Discovered()
        {
            return "손끝에 바싹 마른 뼈가 걸린다. 뼈 사이에는 단단히 얽힌 무언가가 있다.";
        }

        public static string NothingFound()
        {
            return "더 뒤져 보았지만 남아 있는 물건은 없다.";
        }

        public static string ItemsDestroyed()
        {
            return "뼈와 함께 그 안에 있던 물건도 부서졌다.";
        }

        public static string PoisonTrapActivated()
        {
            return "뼈 아래에서 걸쇠가 풀리는 소리가 난다. 매캐한 기체가 쏟아져 나와 방 안을 뒤덮는다.";
        }

        public static string PoisonTrapExamined()
        {
            return "바닥의 장치에서 독기가 계속 뿜어져 나오고 있다.";
        }
    }
}
