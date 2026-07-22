namespace Darkness
{
    public static class InventoryMessages
    {
        public static string ItemObtained(string item)
        {
            return "[" + item + "] 을(를) 얻었습니다";
        }

        public static string LootTaken(string item)
        {
            return item + " 을(를) 얻었다";
        }

        public static string NothingToLoot()
        {
            return "더 얻을 것은 없다";
        }

        public static string InventoryFull()
        {
            return "소지품에 공간이 없어 일부 물건을 가져가지 못했습니다";
        }

        public static string ItemThrown(string item)
        {
            return "당신은 " + item + " 을(를) 던졌다";
        }

        public static string ItemLostInTerrain(
            string item,
            string terrain)
        {
            return item + "은(는) " + terrain +
                   " 깊이 파묻혀 회수할 수 없게 되었다.";
        }

        public static string InventoryExpanded(int amount)
        {
            return "소지품 칸 수가 " + amount + " 늘어났습니다";
        }

        public static string CannotUseItem(string item)
        {
            return item + " 을(를) 사용할 수 없습니다";
        }

        public static string ItemUsed(string item)
        {
            return item + " 을(를) 사용했다";
        }

        public static string HealthRestored(
            string item,
            int amount)
        {
            return item + "을(를) 사용해 생명력 " +
                   amount + "을(를) 회복했다";
        }

        public static string WeaponRepaired(
            string item,
            string weapon)
        {
            return item + "을(를) 사용해 " + weapon +
                   "의 내구도를 완전히 회복했다";
        }

        public static string ItemHadNoEffect(string item)
        {
            return item + " 을(를) 사용할 필요가 없다";
        }

        public static string ThrowItemPrompt(string item)
        {
            return item + " 을(를) 던집니까?";
        }
    }
}
