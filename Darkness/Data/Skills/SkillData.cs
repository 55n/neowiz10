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
                new SkillType("presence_tracking", "기척 추적", "다음 행동까지 정확도가 30 증가한다.", SkillCostType.Focus, 1, false, Effects(Self("focused_accuracy"), Self("focused_accuracy"), Self("focused_accuracy")), SkillFunction.None),
                new SkillType("deflect", "받아흘리기", "다음 자신의 턴까지 회피율이 40 증가한다.", SkillCostType.Focus, 2, false, Effects(Self("deflecting"), Self("deflecting"), Self("deflecting"), Self("deflecting")), SkillFunction.None),
                new SkillType("desperate_counter", "결사 반격", "다음 피해를 받으면 실제 공격자에게 반격한다.", SkillCostType.Focus, 3, false, Effects(Self("counter_stance")), SkillFunction.None),
                new SkillType("sweeping_strike", "휩쓸기", "가까운 확인 대상 모두에게 기본 공격 피해를 주고 대상마다 무기 내구도를 1 소모한다.", SkillCostType.Focus, 3, false, Effects(), SkillFunction.SweepAttack),
                new SkillType("last_focus", "마지막 집중", "다음 공격의 정확도가 100이 된다.", SkillCostType.Focus, 5, false, Effects(Self("perfect_focus")), SkillFunction.None),
                new SkillType("guardian_blessing", "수호의 가호", "치명적인 피해를 받으면 자동으로 체력을 1로 남긴다.", SkillCostType.MagicStone, 5, true, Effects(Self("guardian_blessing")), SkillFunction.None),
                new SkillType("black_step", "검은 걸음", "현재 조우에서 즉시 벗어나 방문한 안전한 방 중 하나로 이동한다.", SkillCostType.MagicStone, 3, false, Effects(), SkillFunction.EscapeEncounter),
                new SkillType("sword_last_day", "칼의 마지막 날", "현재 조우 동안 공격력을 높이고 조우가 끝나면 무기를 파괴한다.", SkillCostType.MagicStone, 4, false, Effects(Self("weapon_empowerment"), Self("weapon_empowerment"), Self("weapon_empowerment"), Self("weapon_destruction")), SkillFunction.None),

                new SkillType("goblin_rush", "겁먹은 돌진", "가장 가까운 적에게 돌진하고 다음 턴까지 자신의 회피율이 20 감소한다.", SkillCostType.Focus, 1, false, Effects(Self("exposed"), Self("exposed")), SkillFunction.RushAttack),
                new SkillType("troll_regeneration", "트롤 재생", "집중을 소모해 최대 생명력의 25%를 회복한다.", SkillCostType.Focus, 2, false, Effects(), SkillFunction.RegenerateHealth),
                new SkillType("armor_chain_bind", "속박의 사슬", "사슬로 대상에게 속박 4스택을 부여한다.", SkillCostType.Focus, 2, false, Effects(Target("bind"), Target("bind"), Target("bind"), Target("bind")), SkillFunction.None),
                new SkillType("wolf_pack_assault", "무리 협공", "확인된 동굴늑대들이 한 대상을 차례로 공격한다.", SkillCostType.Focus, 2, false, Effects(), SkillFunction.PackAssault),
                new SkillType("bat_echo_strike", "반향 급습", "정확도에 25를 더해 대상을 공격한다.", SkillCostType.Focus, 1, false, Effects(), SkillFunction.AttackWithAccuracyBonus25),
                new SkillType("worm_ambush", "지면 매복", "지면 아래에 숨었다가 정확도에 30을 더해 다음 행동자를 공격한다.", SkillCostType.Focus, 2, false, Effects(), SkillFunction.AttackWithAccuracyBonus30),
                new SkillType("water_wail", "젖은 울음", "전체 대상에게 공포 4스택을 부여하고 집중력 1을 감소시킨다.", SkillCostType.Focus, 2, false, Effects(AllTargets("fear"), AllTargets("fear"), AllTargets("fear"), AllTargets("fear")), SkillFunction.DrainFocus1),
                new SkillType("echo_decoy", "거짓 메아리", "다음 자신의 턴까지 회피율이 50 증가한다.", SkillCostType.Focus, 2, false, Effects(Self("false_echo"), Self("false_echo"), Self("false_echo"), Self("false_echo"), Self("false_echo")), SkillFunction.None),
                new SkillType("breath_hunt", "숨결 추적", "다음 행동까지 정확도가 40 증가한다.", SkillCostType.Focus, 3, false, Effects(Self("focused_accuracy"), Self("focused_accuracy"), Self("focused_accuracy"), Self("focused_accuracy")), SkillFunction.None),
                new SkillType("door_bind", "점액 속박", "대상에게 속박 4스택을 부여한다.", SkillCostType.Focus, 2, false, Effects(Target("bind"), Target("bind"), Target("bind"), Target("bind")), SkillFunction.None)
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
