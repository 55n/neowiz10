namespace Darkness
{
    public static class EquipmentMessages
    {
        public static string Broken(string equipment)
        {
            return "[" + equipment + "] 이(가) 파손되었습니다";
        }

        public static string Unequipped(string equipment)
        {
            return "[" + equipment + "]을(를) 장비에서 해제했습니다";
        }

        public static string CannotUnequip()
        {
            return "소지품에 공간이 없어 장비를 해제할 수 없습니다";
        }

        public static string Equipped(string equipment)
        {
            return "[" + equipment + "]을(를) 장착했습니다";
        }

        public static string CannotEquip()
        {
            return "선택한 장비를 장착할 수 없습니다";
        }
    }
}
