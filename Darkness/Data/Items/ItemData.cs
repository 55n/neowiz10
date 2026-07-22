using System;
using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public class ItemData
    {
        public Dictionary<string, ItemType> ItemTypes { get; private set; }

        public ItemData()
        {
            ItemTypes = new List<ItemType>
            {
                new ItemType("loser_mark", "패배자 낙인", "도망친 자에게 새겨지는 불길한 낙인이다.", ItemCategory.Accessory, 1, false, 1, 0, 0, 0, "cowardly_leap", Effects(), Effects(), ItemFunction.None, ItemFunction.None),
                new ItemType("worn_sword", "닳은 한손검", "추락 후에도 손에 남은 낡은 검이다.", ItemCategory.Weapon, 1, false, 4, 3, 0, 6, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("chain_sword", "사슬검", "갑주령의 사슬을 벼려 만든 검이다. 장착하면 [사슬결박]을 익힌다.", ItemCategory.Weapon, 1, false, 5, 4, 0, 10, "chain_bind", Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("forgotten_spear", "잊혀진 창", "이름을 잃은 전사가 쓰던 창이다. 장착하면 [휩쓸기]를 익힌다.", ItemCategory.Weapon, 1, false, 5, 7, 0, 15, "sweep", Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("ordinary_armor", "평범한 갑옷", "여러 번 수선한 흔적이 있는 보통 갑옷이다.", ItemCategory.Armor, 1, false, 7, 0, 1, 10, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("spirit_armor", "영혼갑주", "갑주령의 마력이 깃든 갑옷이다. 장착하면 [영혼방벽]을 익힌다.", ItemCategory.Armor, 1, false, 6, 0, 2, 12, "spirit_barrier", Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("memory_charm", "회상의 부적", "마석에 깃든 기억을 끌어내는 부적이다. 장착하면 [마력회상]을 익힌다.", ItemCategory.Accessory, 1, false, 1, 0, 0, 0, "magic_recollection", Effects(), Effects(), ItemFunction.None, ItemFunction.None),
                new ItemType("guardian_charm", "수호의 부적", "치명적인 피해를 한 번 막아주는 부적이다.", ItemCategory.Accessory, 1, false, 1, 0, 0, 0, "guardian_blessing", Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("cracked_guardian_charm", "금이 간 수호의 부적", "[수호의 가호] 스킬을 사용할 수 있게 해준다.", ItemCategory.Accessory, 1, false, 1, 0, 0, 0, "guardian_blessing", Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("snowflake_pendant", "눈송이 펜던트", "차가운 기운이 맴도는 눈송이 펜던트다. 장착하면 [겨울바람]을 익힌다.", ItemCategory.Accessory, 1, false, 1, 0, 0, 0, "winter_wind", Effects(), Effects(), ItemFunction.None, ItemFunction.None),
                new ItemType("mist_charm", "물안개 부적", "축축한 안개가 맴도는 부적이다. 장착하면 [물보라]를 익힌다.", ItemCategory.Accessory, 1, false, 1, 0, 0, 0, "water_splash", Effects(), Effects(), ItemFunction.None, ItemFunction.None),
                new ItemType("magic_stone", "마석", "몬스터에게서 얻는 응축된 마력이다.", ItemCategory.Consumable, 99, false, 1, 0, 0, 0, null, Effects(), Effects(Target("magic_charge")), ItemFunction.None, ItemFunction.Damage),
                new ItemType("freeze_core", "동결핵", "늪귀신의 차가운 핵이다. 던진 슬롯의 대상을 얼어붙게 한다.", ItemCategory.Consumable, 3, false, 1, 0, 0, 0, null, Effects(), Effects(Target("frozen")), ItemFunction.None, ItemFunction.None),
                new ItemType("stone", "돌멩이", "모래 속에서 찾아낸 묵직한 돌멩이다.", ItemCategory.Consumable, 5, false, 2, 0, 0, 0, null, Effects(), Effects(), ItemFunction.None, ItemFunction.None),
                new ItemType("monster_bait", "몬스터 미끼", "피와 살점으로 만든 미끼로 강한 먹이 냄새가 난다.", ItemCategory.Consumable, 5, false, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Lure),
                new ItemType("pocket", "주머니", "가방에 달아 소지품 칸 수를 2 늘린다.", ItemCategory.Consumable, 1, true, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.ExpandInventory, ItemFunction.None),
                new ItemType("rag_armor", "넝마", "여러 겹의 천 조각을 엮어 만든 조악한 방어구다.", ItemCategory.Armor, 1, false, 2, 0, 1, 5, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("whetstone", "숫돌", "장착한 무기가 파손되기 전에 사용하면 내구도를 모두 회복하고 소모된다.", ItemCategory.Consumable, 1, true, 2, 0, 0, 0, null, Effects(), Effects(), ItemFunction.RepairWeapon, ItemFunction.Damage),
                new ItemType("antidote", "해독제", "몸속에 퍼진 독을 중화해 중독 상태이상을 제거한다.", ItemCategory.Consumable, 3, true, 1, 0, 0, 0, null, Effects(EffectApplication.RemoveStatus("poison", EffectTarget.Self)), Effects(), ItemFunction.None, ItemFunction.None),
                new ItemType("bandage", "붕대", "상처를 감싸 생명력을 5 회복한다.", ItemCategory.Consumable, 2, true, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.RestoreHealth, ItemFunction.None)
            }.ToDictionary(itemType => itemType.Id);
        }

        private static List<EffectApplication> Effects(
            params EffectApplication[] effects)
        {
            return new List<EffectApplication>(effects);
        }

        private static EffectApplication Target(string effectId)
        {
            return new EffectApplication(effectId, EffectTarget.Target, 100);
        }
    }
}
