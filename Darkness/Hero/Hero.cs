using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Hero : IEffectTarget
    {
        public HeroType Type { get; private set; }
        public int CurrentHealth { get; private set; }
        public int CurrentFocus { get; private set; }
        public Inventory Inventory { get; private set; }
        public Dictionary<EquipmentSlot, ItemStack> Equipment { get; private set; }
        public HashSet<string> LearnedSkillIds { get; private set; }
        public List<ActiveEffect> Effects { get; private set; }
        public bool CanAct { get { return CurrentHealth > 0; } }

        public Hero(
            HeroType type,
            Inventory inventory,
            Dictionary<EquipmentSlot, ItemStack> equipment,
            HashSet<string> learnedSkillIds,
            List<ActiveEffect> effects)
        {
            Type = type;
            CurrentHealth = type.MaxHealth;
            CurrentFocus = type.MaxFocus;
            Inventory = inventory;
            Equipment = equipment;
            LearnedSkillIds = learnedSkillIds;
            Effects = effects;
        }

        public void TakeDamage(int damage)
        {
            int appliedDamage = Math.Max(0, damage);
            ActiveEffect guardian = Effects.Find(effect =>
                effect.Type.Function == EffectFunction.PreventDeath);
            if (guardian != null && appliedDamage >= CurrentHealth)
            {
                CurrentHealth = 1;
                RemoveEffect(guardian.Type.Id);
                return;
            }

            CurrentHealth = Math.Max(0, CurrentHealth - appliedDamage);
        }

        public void RestoreHealth(int amount)
        {
            CurrentHealth = Math.Min(Type.MaxHealth, CurrentHealth + Math.Max(0, amount));
        }

        public void SpendFocus(int amount)
        {
            CurrentFocus = Math.Max(0, CurrentFocus - Math.Max(0, amount));
        }

        public void RestoreFocus(int amount)
        {
            CurrentFocus = Math.Min(Type.MaxFocus, CurrentFocus + Math.Max(0, amount));
        }

        public void Equip(EquipmentSlot slot, ItemStack itemStack)
        {
            if (itemStack == null || !Inventory.ItemStacks.Contains(itemStack))
            {
                return;
            }

            ItemStack previous;
            Equipment.TryGetValue(slot, out previous);
            Item equippedItem = itemStack.Item;
            Inventory.Discard(itemStack, 1);
            Equipment[slot] = new ItemStack(equippedItem, 1);
            if (previous != null)
            {
                Inventory.Store(previous);
            }
        }

        public void Unequip(EquipmentSlot slot)
        {
            ItemStack equipment;
            if (Equipment.TryGetValue(slot, out equipment) && equipment != null &&
                Inventory.Store(equipment) == 0)
            {
                Equipment[slot] = null;
            }
        }

        public void LearnSkill(string skillId)
        {
            if (!string.IsNullOrEmpty(skillId))
            {
                LearnedSkillIds.Add(skillId);
            }
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
            Effects.RemoveAll(effect => effect.Type != null && effect.Type.Id == effectId);
        }

    }
}
