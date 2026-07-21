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
                new ItemType("loser_mark", "패배자 낙인", "도망친 자에게 새겨지는 불길한 낙인이다.", ItemCategory.Accessory, 0, 1, false, 1, 0, 0, 0, "cowardly_leap", Effects(), Effects(), ItemFunction.None, ItemFunction.None),
                new ItemType("ordinary_sword", "평범한 한손검", "특별할 것 없는 평범한 한손검이다.", ItemCategory.Weapon, 10, 1, false, 4, 3, 0, 5, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("worn_sword", "닳은 한손검", "추락 후에도 손에 남은 낡은 검이다.", ItemCategory.Weapon, 8, 1, false, 4, 3, 0, 5, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("ordinary_armor", "평범한 갑옷", "여러 번 수선한 흔적이 있는 보통 갑옷이다.", ItemCategory.Armor, 10, 1, false, 7, 0, 2, 5, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("guardian_charm", "수호의 부적", "치명적인 피해를 한 번 막아주는 부적이다.", ItemCategory.Accessory, 50, 1, false, 1, 0, 0, 0, "guardian_blessing", Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("cracked_guardian_charm", "금이 간 수호의 부적", "[수호의 가호] 스킬을 사용할 수 있게 해준다.", ItemCategory.Accessory, 30, 1, false, 1, 0, 0, 0, "guardian_blessing", Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("magic_stone", "마석", "몬스터에게서 얻는 응축된 마력이다.", ItemCategory.Consumable, 1, 99, false, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("monster_bait", "몬스터 미끼", "피와 살점으로 만든 미끼로 강한 먹이 냄새가 난다.", ItemCategory.Consumable, 3, 5, false, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Lure),
                new ItemType("pocket", "주머니", "가방에 달아 소지품 칸 수를 2 늘린다.", ItemCategory.Consumable, 8, 1, true, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.ExpandInventory, ItemFunction.None),
                new ItemType("rag_armor", "넝마", "여러 겹의 천 조각을 엮어 만든 조악한 방어구다.", ItemCategory.Armor, 3, 1, false, 2, 0, 1, 3, null, Effects(), Effects(), ItemFunction.None, ItemFunction.Damage),
                new ItemType("troll_oil", "트롤 기름", "체취를 가리거나 다른 위치에 강한 냄새를 남긴다.", ItemCategory.Consumable, 6, 3, true, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.MaskScent, ItemFunction.CreateScent),
                new ItemType("troll_blood", "트롤 피", "재생력이 남은 희귀한 피로 체력을 크게 회복한다.", ItemCategory.Consumable, 12, 2, true, 1, 0, 0, 0, null, Effects(), Effects(), ItemFunction.HealHealth, ItemFunction.HealHealth),
                new ItemType("vibration_shell", "진동벌레 허물", "바닥에 부딪히면 길고 복잡한 진동을 만든다.", ItemCategory.Consumable, 5, 3, false, 2, 0, 0, 0, null, Effects(), Effects(), ItemFunction.None, ItemFunction.CreateVibration),
                new ItemType("door_slime", "문붙이 점액", "사용하면 발소리를 줄이고 던져 맞히면 속박 4스택을 부여한다.", ItemCategory.Consumable, 7, 3, true, 1, 0, 0, 0, null, Effects(), Effects(Target("bind"), Target("bind"), Target("bind"), Target("bind")), ItemFunction.SilenceMovement, ItemFunction.None),
                new ItemType("whetstone", "숫돌", "선택한 무기의 내구도를 완전히 회복하고 소모된다.", ItemCategory.Consumable, 10, 1, true, 2, 0, 0, 0, null, Effects(), Effects(), ItemFunction.RepairWeapon, ItemFunction.Damage),
                new ItemType("antidote", "해독제", "몸속에 퍼진 독을 중화해 중독 상태이상을 제거한다.", ItemCategory.Consumable, 8, 3, true, 1, 0, 0, 0, null, Effects(EffectApplication.RemoveStatus("poison", EffectTarget.Self)), Effects(), ItemFunction.None, ItemFunction.None)
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
