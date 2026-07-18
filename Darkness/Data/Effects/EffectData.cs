using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public class EffectData
    {
        public Dictionary<string, EffectType> EffectTypes { get; private set; }

        public EffectData()
        {
            EffectTypes = new List<EffectType>
            {
                Effect("bleeding", "출혈", "1스택마다 행동 시 체력이 1 감소한다.", EffectCategory.Debuff, EffectDurationScope.Persistent, 3, EffectDurationUnit.Action, true, 5, EffectFunction.DamageHealth1OnAction),
                Effect("poison", "중독", "1스택마다 방 이동 시 체력이 1 감소한다.", EffectCategory.Debuff, EffectDurationScope.Persistent, 3, EffectDurationUnit.RoomMove, true, 5, EffectFunction.DamageHealth1OnRoomMove),
                Effect("fracture", "골절", "1스택마다 정확도와 회피율이 5 감소한다.", EffectCategory.Debuff, EffectDurationScope.Persistent, 3, EffectDurationUnit.RoomMove, true, 4, EffectFunction.ReduceAccuracy5AndEvasion5),
                Effect("fear", "공포", "1스택마다 정확도가 5 감소한다.", EffectCategory.Debuff, EffectDurationScope.Persistent, 2, EffectDurationUnit.RoomMove, true, 4, EffectFunction.ReduceAccuracy5),
                Effect("bind", "속박", "1스택마다 회피율이 25 감소한다.", EffectCategory.Debuff, EffectDurationScope.Timed, 2, EffectDurationUnit.Turn, true, 4, EffectFunction.ReduceEvasion25),
                Effect("confusion", "혼란", "1스택마다 정확도가 10 감소한다.", EffectCategory.Debuff, EffectDurationScope.Timed, 2, EffectDurationUnit.Turn, true, 5, EffectFunction.ReduceAccuracy10),
                Effect("tracking", "추적", "1스택마다 회피율이 10 감소한다.", EffectCategory.Debuff, EffectDurationScope.Timed, 3, EffectDurationUnit.Turn, true, 5, EffectFunction.ReduceEvasion10),
                Effect("focused_accuracy", "집중된 조준", "1스택마다 정확도가 10 증가한다.", EffectCategory.Buff, EffectDurationScope.Timed, 1, EffectDurationUnit.Action, true, 10, EffectFunction.IncreaseAccuracy10),
                Effect("deflecting", "받아흘리기", "1스택마다 회피율이 10 증가한다.", EffectCategory.Buff, EffectDurationScope.Timed, 1, EffectDurationUnit.Turn, true, 10, EffectFunction.IncreaseEvasion10),
                Effect("exposed", "빈틈", "1스택마다 회피율이 10 감소한다.", EffectCategory.Debuff, EffectDurationScope.Timed, 1, EffectDurationUnit.Turn, true, 10, EffectFunction.ReduceEvasion10),
                Effect("false_echo", "거짓 메아리", "1스택마다 회피율이 10 증가한다.", EffectCategory.Buff, EffectDurationScope.Timed, 1, EffectDurationUnit.Turn, true, 10, EffectFunction.IncreaseEvasion10),
                Effect("weapon_empowerment", "무기 강화", "1스택마다 공격력이 1 증가한다.", EffectCategory.Buff, EffectDurationScope.Encounter, 1, EffectDurationUnit.Encounter, true, 10, EffectFunction.IncreaseAttack1),
                Condition("perfect_focus", "완전한 집중", "다음 공격의 정확도를 100으로 만든다.", EffectTrigger.OnAttack, true, EffectFunction.SetAttackAccuracyTo100),
                Condition("counter_stance", "반격 태세", "다음 피해를 받으면 실제 공격자에게 반격한다.", EffectTrigger.OnDamageTaken, true, EffectFunction.CounterAttack),
                Condition("guardian_blessing", "수호의 가호", "치명적인 피해를 받으면 체력을 1로 남긴다.", EffectTrigger.OnLethalDamage, true, EffectFunction.PreventDeath),
                Condition("weapon_destruction", "무기의 마지막", "조우가 끝나면 장착 무기를 파괴한다.", EffectTrigger.OnEncounterEnd, true, EffectFunction.DestroyEquippedWeapon)
            }.ToDictionary(effectType => effectType.Id);
        }

        private static EffectType Effect(
            string id,
            string name,
            string description,
            EffectCategory category,
            EffectDurationScope durationScope,
            int duration,
            EffectDurationUnit durationUnit,
            bool isStackable,
            int maxStackCount,
            EffectFunction function)
        {
            return new EffectType(
                id,
                name,
                description,
                category,
                durationScope,
                duration,
                durationUnit,
                isStackable,
                maxStackCount,
                EffectTrigger.None,
                false,
                function);
        }

        private static EffectType Condition(
            string id,
            string name,
            string description,
            EffectTrigger trigger,
            bool removeAfterTrigger,
            EffectFunction function)
        {
            return new EffectType(
                id,
                name,
                description,
                EffectCategory.Condition,
                EffectDurationScope.Infinite,
                0,
                EffectDurationUnit.Turn,
                false,
                1,
                trigger,
                removeAfterTrigger,
                function);
        }
    }
}
