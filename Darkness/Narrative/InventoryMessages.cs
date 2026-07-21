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

        public static string InventoryExpanded(int amount)
        {
            return "소지품 칸 수가 " + amount + " 늘어났습니다";
        }

        public static string CannotUseItem(string item)
        {
            return item + " 을(를) 사용할 수 없습니다";
        }

        public static string ThrowItemPrompt(string item)
        {
            return item + " 을(를) 던집니까?";
        }
    }
}
