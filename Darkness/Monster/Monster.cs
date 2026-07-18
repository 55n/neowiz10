using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Monster : IEffectTarget, IEncounterActor, IActionReactor, IRoomObject
    {
        public MonsterType Type { get; private set; }
        public int CurrentHealth { get; private set; }
        public int CurrentFocus { get; private set; }
        public Inventory Inventory { get; private set; }
        public MonsterAwareness Awareness { get; private set; }
        public int SearchTurnsRemaining { get; private set; }
        public List<ActiveEffect> Effects { get; private set; }
        public bool IsAlive { get { return CurrentHealth > 0; } }
        public bool CanAct { get { return IsAlive; } }
        public string Id { get { return Type.Id; } }
        public string Name { get { return Type.Name; } }
        public RoomObjectType ObjectType { get { return RoomObjectType.Monster; } }

        public Monster(
            MonsterType type,
            Inventory inventory,
            List<ActiveEffect> effects)
        {
            Type = type;
            CurrentHealth = type.MaxHealth;
            CurrentFocus = type.MaxFocus;
            Inventory = inventory;
            Effects = effects;
            Awareness = MonsterAwareness.Indifferent;
        }

        protected Monster(
            MonsterType type,
            Inventory inventory,
            List<ActiveEffect> effects,
            string expectedTypeId)
            : this(type, inventory, effects)
        {
            if (type == null || type.Id != expectedTypeId)
            {
                throw new ArgumentException("Monster type does not match concrete monster class.", "type");
            }
        }

        public void React(MonsterReaction reaction)
        {
            if (!IsAlive || reaction == null)
            {
                return;
            }

            if (reaction.Type == MonsterReactionType.Attack)
            {
                BeginAttack();
                return;
            }

            if (Awareness == MonsterAwareness.Indifferent)
            {
                if (reaction.AlertChange <= 0)
                {
                    return;
                }

                Awareness = MonsterAwareness.Searching;
                SearchTurnsRemaining = Type.InitialSearchTurns - reaction.AlertChange;
            }
            else if (Awareness == MonsterAwareness.Searching)
            {
                SearchTurnsRemaining -= reaction.AlertChange;
            }

            if (Awareness != MonsterAwareness.Searching)
            {
                return;
            }

            if (SearchTurnsRemaining <= 0)
            {
                BeginAttack();
            }
            else if (SearchTurnsRemaining > Type.MaxSearchTurns)
            {
                Awareness = MonsterAwareness.Indifferent;
                SearchTurnsRemaining = 0;
            }
        }

        public void BeginAttack()
        {
            Awareness = MonsterAwareness.Attacking;
            SearchTurnsRemaining = 0;
        }

        public bool TakeDamage(int damage)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - Math.Max(0, damage));
            return !IsAlive;
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

        public virtual void Evaluate(
            EncounterActionContext context,
            IList<ReactionResult> results)
        {
            MonsterReaction reaction = GetReaction(Type.Reactions, context.Action.Type);
            if (reaction != null)
            {
                results.Add(ReactionResult.React(this, reaction));
            }
        }

        private static MonsterReaction GetReaction(
            MonsterReactionSet reactions,
            EncounterActionType actionType)
        {
            switch (actionType)
            {
                case EncounterActionType.Focus: return reactions.Focus;
                case EncounterActionType.Wait: return reactions.Wait;
                case EncounterActionType.Approach: return reactions.Approach;
                case EncounterActionType.MakeNoise: return reactions.MakeNoise;
                case EncounterActionType.UseItem:
                case EncounterActionType.ThrowItem:
                case EncounterActionType.UseSkill: return reactions.UseItem;
                case EncounterActionType.Search: return reactions.Search;
                case EncounterActionType.Attack: return reactions.Attack;
                default: return null;
            }
        }
    }
}
