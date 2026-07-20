using System;
using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public class SkillData
    {
        public Dictionary<string, SkillType> SkillTypes { get; private set; }

        public SkillData()
        {
            SkillTypes = new List<SkillType>
            {
                new SkillType("cowardly_leap", "비겁한 도약", "마석을 소모해 출구로 도망친다.", SkillCostType.MagicStone, 4, false, Effects()),
                new SkillType("presence_tracking", "기척 추적", "다음 행동까지 정확도가 30 증가한다.", SkillCostType.Focus, 1, false, Effects(Self("focused_accuracy"), Self("focused_accuracy"), Self("focused_accuracy"))),
                new SkillType("deflect", "받아흘리기", "다음 자신의 턴까지 회피율이 40 증가한다.", SkillCostType.Focus, 2, false, Effects(Self("deflecting"), Self("deflecting"), Self("deflecting"), Self("deflecting"))),
                new SkillType("desperate_counter", "결사 반격", "다음 피해를 받으면 실제 공격자에게 반격한다.", SkillCostType.Focus, 3, false, Effects(Self("counter_stance"))),
                new SkillType("last_focus", "마지막 집중", "다음 공격의 정확도에 100을 더한다.", SkillCostType.Focus, 5, false, SkillTargetingType.None, Effects(EffectApplication.ApplyStatus("perfect_focus", EffectTarget.Self))),
                new SkillType("power_strike", "강타", "집중을 소모해 공격력의 150% 피해로 공격한다.", SkillCostType.Focus, 1, false, SkillTargetingType.SingleSlot, SkillAttackType.Weapon, Effects(EffectApplication.Attack(EffectTarget.Target, 150, 100, AttackDeliveryType.EquippedWeapon))),
                new SkillType("guardian_blessing", "수호의 가호", "자신에게 수호의 가호를 부여해 치명적인 피해를 한 번 막는다.", SkillCostType.MagicStone, 5, false, SkillTargetingType.None, Effects(EffectApplication.ApplyStatus("guardian_blessing", EffectTarget.Self)), "{0} 은(는) 한 번의 죽음을 면할 수 있게 되었다"),
                new SkillType("black_step", "검은 걸음", "현재 조우에서 즉시 벗어나 방문한 안전한 방 중 하나로 이동한다.", SkillCostType.MagicStone, 3, false, Effects()),
                new SkillType("sword_last_day", "칼의 최후", "장착한 무기의 공격력을 2배로 만들고 내구도를 1로 만든다.", SkillCostType.MagicStone, 2, false, SkillTargetingType.None, SkillAttackType.Weapon, Effects(EffectApplication.ApplyStatus("weapon_empowerment", EffectTarget.EquippedWeapon), EffectApplication.SetEquipmentDurability("weapon_consumption", EffectTarget.Self, EquipmentSlot.Weapon, 1))),

                new SkillType("goblin_rush", "겁먹은 돌진", "명중률이 50 하락하고 5의 피해를 주는 돌진 공격을 한다", SkillCostType.Focus, 1, false, SkillTargetingType.SingleSlot, SkillAttackType.Natural, Effects(EffectApplication.FixedAttack(EffectTarget.Target, 5, -50))),
                new SkillType("troll_regeneration", "트롤 재생", "집중을 소모해 최대 생명력의 25%를 회복한다.", SkillCostType.Focus, 2, false, Effects()),
                new SkillType("armor_chain_bind", "속박의 사슬", "사슬로 대상에게 속박 4스택을 부여한다.", SkillCostType.Focus, 2, false, Effects(Target("bind"), Target("bind"), Target("bind"), Target("bind"))),
                new SkillType("wolf_pack_assault", "무리 협공", "확인된 동굴늑대들이 한 대상을 차례로 공격한다.", SkillCostType.Focus, 2, false, Effects()),
                new SkillType("bat_echo_strike", "반향 급습", "정확도에 25를 더해 대상을 공격한다.", SkillCostType.Focus, 1, false, Effects()),
                new SkillType("worm_ambush", "지면 매복", "지면 아래에 숨었다가 정확도에 30을 더해 다음 행동자를 공격한다.", SkillCostType.Focus, 2, false, Effects()),
                new SkillType("water_wail", "젖은 울음", "전체 대상에게 공포 4스택을 부여하고 집중력 1을 감소시킨다.", SkillCostType.Focus, 2, false, Effects(AllTargets("fear"), AllTargets("fear"), AllTargets("fear"), AllTargets("fear"))),
                new SkillType("echo_decoy", "거짓 메아리", "다음 자신의 턴까지 회피율이 50 증가한다.", SkillCostType.Focus, 2, false, Effects(Self("false_echo"), Self("false_echo"), Self("false_echo"), Self("false_echo"), Self("false_echo"))),
                new SkillType("breath_hunt", "숨결 추적", "다음 행동까지 정확도가 40 증가한다.", SkillCostType.Focus, 3, false, Effects(Self("focused_accuracy"), Self("focused_accuracy"), Self("focused_accuracy"), Self("focused_accuracy"))),
                new SkillType("door_bind", "점액 속박", "대상에게 속박 4스택을 부여한다.", SkillCostType.Focus, 2, false, Effects(Target("bind"), Target("bind"), Target("bind"), Target("bind")))
            }.ToDictionary(skillType => skillType.Id);
        }

        private static List<EffectApplication> Effects(
            params EffectApplication[] effects)
        {
            return new List<EffectApplication>(effects);
        }

        private static EffectApplication Self(string effectId)
        {
            return new EffectApplication(effectId, EffectTarget.Self, 100);
        }

        private static EffectApplication Target(string effectId)
        {
            return new EffectApplication(effectId, EffectTarget.Target, 100);
        }

        private static EffectApplication AllTargets(string effectId)
        {
            return new EffectApplication(effectId, EffectTarget.AllTargets, 100);
        }
    }
}
