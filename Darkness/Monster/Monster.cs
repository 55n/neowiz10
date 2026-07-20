using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Monster : IDamageable, ISkillUser, IEffectTarget, ISlotContent
    {
        public MonsterType Type { get; private set; }
        public int CurrentHealth { get; private set; }
        public int CurrentFocus { get; private set; }
        public int Attack
        {
            get
            {
                return Math.Max(
                    0,
                    Type.Attack + GetEffectAttackBonus());
            }
        }
        public int Defense
        {
            get
            {
                return Math.Max(
                    0,
                    Type.Defense + GetEffectDefenseBonus());
            }
        }
        public int Accuracy { get { return Type.Accuracy; } }
        public int Evasion { get { return Type.Evasion; } }
        public Inventory Inventory { get; private set; }
        public List<ActiveEffect> Effects { get; private set; }
        public MonsterState State { get; private set; }
        public MonsterActionPlan NextAction { get; private set; }
        public IMonsterBehavior Behavior { get; private set; }
        public int DetectionDelayTurns { get; private set; }
        public int DetectionTurnsRemaining { get; private set; }
        public bool WasHitSinceLastAction { get; private set; }
        public bool IsAlive { get { return CurrentHealth > 0; } }
        public bool CanAct { get { return IsAlive; } }
        public string Id { get { return Type.Id; } }
        public string Name { get { return Type.Name; } }
        public RoomObjectType ObjectType { get { return RoomObjectType.Monster; } }

        public Monster(
            MonsterType type,
            Inventory inventory,
            List<ActiveEffect> effects,
            IMonsterBehavior behavior,
            int detectionDelayTurns = 0)
        {
            if (behavior == null)
            {
                throw new ArgumentNullException("behavior");
            }

            Type = type;
            CurrentHealth = type.MaxHealth;
            CurrentFocus = type.MaxFocus;
            Inventory = inventory;
            Effects = effects;
            Behavior = behavior;
            DetectionDelayTurns = Math.Max(0, detectionDelayTurns);
            DetectionTurnsRemaining = DetectionDelayTurns;
            State = MonsterState.Indifferent;
            NextAction = MonsterActionPlan.None();
        }

        public void ReceiveDamage(int damage)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - Math.Max(0, damage));
        }

        public void RegisterHit()
        {
            WasHitSinceLastAction = true;
        }

        public void EnterAlert()
        {
            DetectionTurnsRemaining = DetectionDelayTurns;
        }

        public bool AdvanceDetection()
        {
            DetectionTurnsRemaining = Math.Max(
                0,
                DetectionTurnsRemaining - 1);
            return DetectionTurnsRemaining == 0;
        }

        public void SpendFocus(int amount)
        {
            CurrentFocus = Math.Max(
                0,
                CurrentFocus - Math.Max(0, amount));
        }

        public void RestoreFocus(int amount)
        {
            CurrentFocus = Math.Min(
                Type.MaxFocus,
                CurrentFocus + Math.Max(0, amount));
        }

        public bool KnowsSkill(string skillId)
        {
            return !string.IsNullOrEmpty(skillId) &&
                   Type.FocusSkillIds.Contains(skillId);
        }

        public void ApplyEffect(ActiveEffect effect)
        {
            if (effect == null)
            {
                return;
            }

            ActiveEffect existing = Effects.Find(active => active.Type.Id == effect.Type.Id);
            if (existing != null && existing.Type.IsStackable)
            {
                existing.AddStack();
                return;
            }

            if (existing == null)
            {
                Effects.Add(effect);
            }
        }

        public void RemoveEffect(string effectId)
        {
            Effects.RemoveAll(effect => effect.Type.Id == effectId);
        }

        private int GetEffectAttackBonus()
        {
            int bonus = 0;
            foreach (ActiveEffect effect in Effects)
            {
                bonus += effect.GetAttackBonus();
            }

            return bonus;
        }

        private int GetEffectDefenseBonus()
        {
            int bonus = 0;
            foreach (ActiveEffect effect in Effects)
            {
                bonus += effect.GetDefenseBonus();
            }

            return bonus;
        }

        public virtual SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            if (!IsAlive)
            {
                return result;
            }

            if (context.Action == PlayerActionType.Attack)
            {
                Item weapon = context.Actor.GetEquippedItem(
                    EquipmentSlot.Weapon);
                result.Attacks.Add(new AttackContext(
                    context.Actor,
                    this,
                    context.Actor.Attack,
                    context.Actor.Accuracy,
                    Type.Evasion,
                    weapon == null
                        ? AttackDeliveryType.Natural
                        : AttackDeliveryType.EquippedWeapon,
                    weapon,
                    weapon == null ? 0 : 1));
            }

            return result;
        }

        public MonsterDecision Decide(MonsterPerception perception)
        {
            if (!CanAct)
            {
                NextAction = MonsterActionPlan.None();
                return MonsterDecision.None(State);
            }

            RemoveEffect("defending");

            MonsterDecision decision = Behavior.Decide(this, perception) ??
                MonsterDecision.None(State);
            State = decision.NextState;
            NextAction = decision.Action;
            return decision;
        }

        public SlotInteractionResult Act(Hero target, Room room)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            MonsterActionPlan action = NextAction;
            NextAction = MonsterActionPlan.None();

            if (!CanAct || action == null)
            {
                return result;
            }

            if (action.Type == MonsterActionType.Attack)
            {
                result.Attacks.Add(new AttackContext(
                    this,
                    target,
                    Attack,
                    Accuracy,
                    target.Type.Evasion));
            }
            else if (action.Type == MonsterActionType.Move &&
                     action.TargetSlot != null)
            {
                result.MonsterMoves.Add(
                    new MonsterMoveRequest(this, action.TargetSlot));
            }
            else if (action.Type == MonsterActionType.Defend)
            {
                EffectType defendingType =
                    new EffectData().EffectTypes["defending"];
                ApplyEffect(new DefendingEffect(defendingType));
            }
            else if (action.Type == MonsterActionType.UseSkill &&
                     !string.IsNullOrEmpty(action.SkillId))
            {
                SkillType skill;
                if (new SkillData().SkillTypes.TryGetValue(
                        action.SkillId,
                        out skill))
                {
                    result.SkillUses.Add(new SkillUseContext(
                        this,
                        skill,
                        room,
                        new object[] { target }));
                }
            }

            if (action.StateAfterAction.HasValue)
            {
                State = action.StateAfterAction.Value;
            }

            WasHitSinceLastAction = false;

            return result;
        }

    }
}
