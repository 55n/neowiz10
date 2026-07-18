using System;
using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public class MonsterData
    {
        public Dictionary<string, MonsterType> MonsterTypes { get; private set; }

        public MonsterData()
        {
            MonsterTypes = new List<MonsterType>
            {
                new MonsterType(
                    "lost_goblin", "길 잃은 고블린", "어둠에 적응하지 못한 작고 겁 많은 잡식성 생물이다.",
                    6, 2, 2, 0, 4, 70, 20, 3, 5,
                    S(MonsterSense.Hearing, MonsterSense.Smell),
                    Set(
                        R(MonsterReactionType.Observe, 0, "호흡과 금속 소리로 개체 수를 드러낸다."),
                        R(MonsterReactionType.Threaten, 1, "서로 짧은 울음으로 신호를 주고받는다."),
                        R(MonsterReactionType.Retreat, 1, "혼자면 물러나고 무리라면 몰려서 위협한다."),
                        R(MonsterReactionType.Threaten, 2, "작은 소리에는 숨고 큰 소리에는 비명을 지른다."),
                        R(MonsterReactionType.Investigate, -2, "음식이나 고기 냄새가 나면 경계를 풀고 다가간다."),
                        R(MonsterReactionType.Investigate, 1, "탐색 소리를 듣고 소리가 난 곳을 살핀다."),
                        R(MonsterReactionType.Attack, 3, "겁에 질려 반격하며 가까운 동족을 부른다."),
                        R(MonsterReactionType.Retreat, -1, "추격하기보다 안전한 어둠 속으로 숨는다.")),
                    Effects(), MonsterFunction.Damage, Skills("goblin_rush")),

                new MonsterType(
                    "hungry_troll", "굶주린 트롤", "후각으로 먹이를 추적하며 상처가 빠르게 아물어 가는 대형 포식자다.",
                    24, 3, 6, 3, 1, 75, 5, 2, 4,
                    S(MonsterSense.Smell, MonsterSense.Hearing),
                    Set(
                        R(MonsterReactionType.Observe, 0, "무거운 호흡과 냄새 맡는 소리로 위치를 드러낸다."),
                        R(MonsterReactionType.Approach, 1, "플레이어의 체취를 따라 천천히 가까워진다."),
                        R(MonsterReactionType.Attack, 2, "가까워진 체취를 맡고 즉시 먹이로 판단한다."),
                        R(MonsterReactionType.Attack, 2, "소리가 난 위치로 거칠게 돌진한다."),
                        R(MonsterReactionType.Investigate, -2, "고기와 강한 냄새가 나는 위치로 유인된다."),
                        R(MonsterReactionType.Approach, 1, "탐색하는 동안 체취의 근원을 좁힌다."),
                        R(MonsterReactionType.Attack, 3, "상처를 무시하고 힘으로 짓누르려 한다."),
                        R(MonsterReactionType.Attack, 2, "감지한 먹이가 달아나면 강하게 추격한다.")),
                    Effects(), MonsterFunction.Damage, Skills("troll_regeneration")),

                new MonsterType(
                    "bound_armor_spirit", "속박된 갑주령", "갑옷과 방에 속박되어 마석으로 기억과 영혼을 유지하는 옛 탐험가다.",
                    30, 5, 7, 6, 2, 85, 10, 4, 6,
                    S(MonsterSense.Magic, MonsterSense.Hearing),
                    Set(
                        R(MonsterReactionType.Observe, 0, "사슬과 빈 갑옷의 움직임을 들려준다."),
                        R(MonsterReactionType.Communicate, -1, "충분히 기다리면 먼저 말을 건다."),
                        R(MonsterReactionType.Threaten, 1, "일정 거리 안으로 들어오면 낮은 목소리로 경고한다."),
                        R(MonsterReactionType.Communicate, -1, "말을 이해하고 주변 미궁에 관한 대화를 시작한다."),
                        R(MonsterReactionType.Trade, -2, "마석을 감지하면 거래와 가르침을 제안한다."),
                        R(MonsterReactionType.Communicate, 0, "통로 정보의 대가로 마석을 요구한다."),
                        R(MonsterReactionType.Attack, 3, "거래를 끝내고 사슬과 갑옷으로 공격한다."),
                        R(MonsterReactionType.Ignore, -1, "방을 떠나는 상대를 막지 않는다.")),
                    Effects(Target("bind", 40), Target("bind", 40)), MonsterFunction.Damage, Skills("armor_chain_bind")),

                new MonsterType(
                    "blind_cave_wolf", "눈먼 동굴늑대", "후각과 무리 신호가 발달한 미궁의 육식동물이다.",
                    10, 3, 4, 1, 5, 80, 30, 2, 4,
                    S(MonsterSense.Smell, MonsterSense.Hearing, MonsterSense.Vibration),
                    Set(
                        R(MonsterReactionType.Observe, 0, "서로 다른 위치의 호흡과 발소리로 수를 드러낸다."),
                        R(MonsterReactionType.Approach, 1, "한 마리가 접근하고 나머지는 양옆으로 퍼진다."),
                        R(MonsterReactionType.Threaten, 2, "가까운 늑대가 울음으로 무리에게 신호한다."),
                        R(MonsterReactionType.Investigate, 1, "무리 전체가 동시에 소리 난 방향을 향한다."),
                        R(MonsterReactionType.Investigate, -2, "고기가 충분하면 한곳에 모이고 부족하면 일부만 반응한다."),
                        R(MonsterReactionType.Approach, 1, "한 마리가 소리를 확인하는 동안 다른 개체가 접근한다."),
                        R(MonsterReactionType.Attack, 3, "공격받은 개체를 중심으로 무리가 협공한다."),
                        R(MonsterReactionType.Attack, 2, "도망가는 먹이의 냄새와 발소리를 쫓는다.")),
                    Effects(), MonsterFunction.Damage, Skills("wolf_pack_assault")),

                new MonsterType(
                    "echo_bat", "울림박쥐", "반향 기관으로 완전한 어둠 속을 비행하는 소형 포식자다.",
                    5, 2, 2, 0, 6, 75, 40, 2, 3,
                    S(MonsterSense.Echolocation, MonsterSense.Hearing),
                    Set(
                        R(MonsterReactionType.Observe, 0, "고음의 간격을 통해 개체 수를 추정하게 한다."),
                        R(MonsterReactionType.Observe, 0, "벽에 매달리고 비행하는 행동 주기를 반복한다."),
                        R(MonsterReactionType.Attack, 1, "비행 중인 개체 가까이에 들어오면 급강하한다."),
                        R(MonsterReactionType.Attack, 2, "큰 소리의 위치를 즉시 파악하고 덮친다."),
                        R(MonsterReactionType.Investigate, -1, "던진 물건의 충돌 지점으로 공격 방향을 바꾼다."),
                        R(MonsterReactionType.Investigate, 1, "벽을 더듬는 소리를 반향으로 확인한다."),
                        R(MonsterReactionType.Retreat, 3, "공격 후 다시 천장 쪽으로 거리를 벌린다."),
                        R(MonsterReactionType.Ignore, -1, "영역에서 멀어지는 대상을 오래 쫓지 않는다.")),
                    Effects(Target("confusion", 30), Target("confusion", 30), Target("confusion", 30)), MonsterFunction.DrainFocus, Skills("bat_echo_strike")),

                new MonsterType(
                    "vibration_worm", "진동벌레", "바닥 아래에 숨어 충격과 체중 이동을 기다리는 매복 생물이다.",
                    14, 2, 5, 4, 2, 80, 10, 3, 5,
                    S(MonsterSense.Vibration),
                    Set(
                        R(MonsterReactionType.Observe, 0, "지면 아래의 긁는 방향으로 위치를 드러낸다."),
                        R(MonsterReactionType.Observe, 0, "일정한 잠복 이동 주기를 반복한다."),
                        R(MonsterReactionType.Approach, 2, "발걸음 진동을 따라 지면 아래에서 접근한다."),
                        R(MonsterReactionType.Ignore, 0, "공기를 통한 목소리에는 거의 반응하지 않는다."),
                        R(MonsterReactionType.Investigate, -2, "가장 무거운 충돌이 발생한 지점으로 이동한다."),
                        R(MonsterReactionType.Attack, 2, "벽과 바닥을 더듬는 진동을 먹이로 오인한다."),
                        R(MonsterReactionType.Attack, 3, "지면 밖으로 튀어나와 공격자를 덮친다."),
                        R(MonsterReactionType.Approach, 1, "멀어지는 발걸음 진동을 잠시 추적한다.")),
                    Effects(Target("fracture", 30), Target("fracture", 30), Target("fracture", 30)), MonsterFunction.Damage, Skills("worm_ambush")),

                new MonsterType(
                    "swamp_water_spirit", "늪지 물귀신", "물길과 젖은 바닥을 영역으로 삼는 습윤 포식자다.",
                    12, 4, 4, 2, 3, 75, 20, 3, 5,
                    S(MonsterSense.Smell, MonsterSense.Hearing),
                    Set(
                        R(MonsterReactionType.Observe, 0, "젖은 호흡과 물소리로 영역과 개체 수를 드러낸다."),
                        R(MonsterReactionType.Threaten, 1, "목을 부풀려 낮은 경고음을 낸다."),
                        R(MonsterReactionType.Threaten, 2, "영역 침범자를 향해 물가에서 몸을 일으킨다."),
                        R(MonsterReactionType.Attack, 2, "반복되는 소리를 침입자의 도발로 받아들인다."),
                        R(MonsterReactionType.Retreat, -2, "생선에는 끌리고 소금에는 물가 안쪽으로 물러난다."),
                        R(MonsterReactionType.Attack, 2, "영역 가까운 탐색 소리에 즉시 반응한다."),
                        R(MonsterReactionType.Attack, 3, "물가로 끌어당기며 젖은 팔로 공격한다."),
                        R(MonsterReactionType.Ignore, -2, "자기 영역 밖으로 나간 상대는 추격하지 않는다.")),
                    Effects(Target("fear", 35), Target("fear", 35), Target("fear", 35), Target("fear", 35)), MonsterFunction.DrainFocus, Skills("water_wail")),

                new MonsterType(
                    "echo_mimic", "메아리꾼", "다른 생물의 소리를 복제해 가짜 위치로 먹이를 유인하는 포식자다.",
                    8, 4, 3, 1, 4, 70, 25, 3, 5,
                    S(MonsterSense.Hearing),
                    Set(
                        R(MonsterReactionType.Observe, 0, "원음보다 조금 늦은 복제음을 반복한다."),
                        R(MonsterReactionType.Mislead, 0, "여러 위치에서 같은 소리를 내어 기다리는 상대를 혼란시킨다."),
                        R(MonsterReactionType.Mislead, 1, "가짜 위치로 접근을 유도하고 실제 위치를 숨긴다."),
                        R(MonsterReactionType.Mislead, 1, "목소리와 타격음을 이해 없이 그대로 복제한다."),
                        R(MonsterReactionType.Observe, -1, "물건의 원음과 복제음 사이의 시간차를 노출한다."),
                        R(MonsterReactionType.Mislead, 1, "탐색음을 복제해 다른 곳에 문이 있는 것처럼 속인다."),
                        R(MonsterReactionType.Attack, 2, "가짜 위치를 공격하게 한 뒤 실제 위치에서 덮친다."),
                        R(MonsterReactionType.Ignore, -1, "먹이가 멀어지면 다른 소리를 흉내 내며 남는다.")),
                    Effects(Target("confusion", 50), Target("confusion", 50), Target("confusion", 50), Target("confusion", 50), Target("confusion", 50)), MonsterFunction.DrainFocus, Skills("echo_decoy")),

                new MonsterType(
                    "breath_hunter", "숨결사냥꾼", "살아 있는 먹이의 호흡과 체취를 끝까지 좇는 미궁의 최상위 포식자다.",
                    22, 5, 7, 3, 6, 90, 35, 1, 3,
                    S(MonsterSense.Breath, MonsterSense.Smell),
                    Set(
                        R(MonsterReactionType.Observe, 0, "마른 마찰음으로 접근 방향과 속도를 드러낸다."),
                        R(MonsterReactionType.Approach, 2, "멈춘 먹이의 호흡을 따라 반드시 가까워진다."),
                        R(MonsterReactionType.Attack, 3, "거리 감소를 감지하고 즉시 사냥을 시작한다."),
                        R(MonsterReactionType.Ignore, 0, "목소리보다 계속 이어지는 호흡을 추적한다."),
                        R(MonsterReactionType.Investigate, -1, "강한 냄새에는 잠시 혼란스러워하지만 사체에는 오래 속지 않는다."),
                        R(MonsterReactionType.Approach, 2, "탐색에 걸린 시간만큼 호흡의 위치를 좁힌다."),
                        R(MonsterReactionType.Attack, 3, "공격자의 호흡을 붙잡아 실제 위치로 파고든다."),
                        R(MonsterReactionType.Investigate, -2, "빠른 후퇴로 거리가 벌어지면 추적이 잠시 끊긴다.")),
                    Effects(Target("tracking", 100), Target("tracking", 100), Target("tracking", 100)), MonsterFunction.Damage, Skills("breath_hunt")),

                new MonsterType(
                    "door_clinger", "문붙이", "문과 통로에 붙어 지나가는 생물을 기다리는 부착형 포식자다.",
                    18, 3, 5, 5, 1, 85, 5, 2, 4,
                    S(MonsterSense.Vibration, MonsterSense.Hearing),
                    Set(
                        R(MonsterReactionType.Observe, 0, "바람에 반응하는 끈적한 마찰음으로 위치를 드러낸다."),
                        R(MonsterReactionType.Observe, 0, "문에 붙은 채 바람과 진동을 기다린다."),
                        R(MonsterReactionType.Threaten, 2, "문에 가까워질수록 마찰음과 경고음을 키운다."),
                        R(MonsterReactionType.Retreat, 1, "큰 소리에 몸을 움츠리지만 작은 소리는 무시한다."),
                        R(MonsterReactionType.Investigate, -2, "다른 위치에 던진 먹이를 따라 문에서 떨어진다."),
                        R(MonsterReactionType.Attack, 3, "붙어 있는 문을 조사하는 손을 즉시 덮친다."),
                        R(MonsterReactionType.Attack, 3, "단단한 등면으로 버티며 점액으로 공격자를 붙잡는다."),
                        R(MonsterReactionType.Ignore, -2, "방 밖으로 나간 상대는 추격하지 않는다.")),
                    Effects(Target("bind", 50), Target("bind", 50)), MonsterFunction.Damage, Skills("door_bind"))
            }.ToDictionary(monsterType => monsterType.Id);
        }

        private static List<MonsterSense> S(params MonsterSense[] senses)
        {
            return new List<MonsterSense>(senses);
        }

        private static List<string> Skills(params string[] skillIds)
        {
            return new List<string>(skillIds);
        }

        private static List<EffectApplication> Effects(
            params EffectApplication[] effects)
        {
            return new List<EffectApplication>(effects);
        }

        private static EffectApplication Target(
            string effectId,
            int applyChance)
        {
            return new EffectApplication(
                effectId,
                EffectTarget.Target,
                applyChance);
        }

        private static MonsterReaction R(
            MonsterReactionType type,
            int alertChange,
            string description)
        {
            return new MonsterReaction(type, alertChange, description);
        }

        private static MonsterReactionSet Set(
            MonsterReaction focus,
            MonsterReaction wait,
            MonsterReaction approach,
            MonsterReaction makeNoise,
            MonsterReaction useItem,
            MonsterReaction search,
            MonsterReaction attack,
            MonsterReaction retreat)
        {
            return new MonsterReactionSet(
                focus,
                wait,
                approach,
                makeNoise,
                useItem,
                search,
                attack,
                retreat);
        }
    }
}
