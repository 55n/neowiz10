using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Monster : IDamageable, ISlotContent
    {
        public MonsterType Type { get; private set; }
        public int CurrentHealth { get; private set; }
        public int CurrentFocus { get; private set; }
        public int Evasion { get { return Type.Evasion; } }
        public Inventory Inventory { get; private set; }
        public List<ActiveEffect> Effects { get; private set; }
        public MonsterState State { get; private set; }
        public MonsterActionPlan NextAction { get; private set; }
        public IMonsterBehavior Behavior { get; private set; }
        public bool IsAlive { get { return CurrentHealth > 0; } }
        public bool CanAct { get { return IsAlive; } }
        public string Id { get { return Type.Id; } }
        public string Name { get { return Type.Name; } }
        public RoomObjectType ObjectType { get { return RoomObjectType.Monster; } }

        public Monster(
            MonsterType type,
            Inventory inventory,
            List<ActiveEffect> effects,
            IMonsterBehavior behavior)
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
            State = MonsterState.Indifferent;
            NextAction = MonsterActionPlan.None();
        }

        public void ReceiveDamage(int damage)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - Math.Max(0, damage));
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
                result.Attacks.Add(new AttackContext(
                    context.Actor,
                    this,
                    context.Actor.Type.Attack,
                    context.Actor.Type.Accuracy,
                    Type.Evasion));
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

            MonsterDecision decision = Behavior.Decide(this, perception) ??
                MonsterDecision.None(State);
            State = decision.NextState;
            NextAction = decision.Action;
            return decision;
        }

        public SlotInteractionResult Act(Hero target)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            MonsterActionPlan action = NextAction;
            NextAction = MonsterActionPlan.None();

            if (!CanAct || action == null ||
                action.Type == MonsterActionType.None ||
                action.Type == MonsterActionType.Wait)
            {
                return result;
            }

            if (action.Type == MonsterActionType.Attack)
            {
                result.Attacks.Add(new AttackContext(
                    this,
                    target,
                    Type.Attack,
                    Type.Accuracy,
                    target.Type.Evasion));
            }
            else if (action.Type == MonsterActionType.Move &&
                     action.TargetSlot != null)
            {
                result.MonsterMoves.Add(
                    new MonsterMoveRequest(this, action.TargetSlot));
            }

            return result;
        }

    }
}
