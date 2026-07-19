using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Hero : IDamageable
    {
        public HeroType Type { get; private set; }
        public int CurrentHealth { get; private set; }
        public int CurrentFocus { get; private set; }
        public int Evasion { get { return Type.Evasion; } }
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

        public void ReceiveDamage(int damage)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - Math.Max(0, damage));
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

        public bool Equip(EquipmentSlot slot, ItemStack itemStack)
        {
            if (itemStack == null || !Inventory.ItemStacks.Contains(itemStack))
            {
                return false;
            }

            ItemStack previous;
            Equipment.TryGetValue(slot, out previous);
            Item equippedItem = itemStack.Item;
            if (Inventory.Discard(itemStack, 1) != 1)
            {
                return false;
            }

            if (previous != null && Inventory.Store(previous) != 0)
            {
                Inventory.Store(new ItemStack(equippedItem, 1));
                return false;
            }

            Equipment[slot] = new ItemStack(equippedItem, 1);
            return true;
        }

        public bool Unequip(EquipmentSlot slot)
        {
            ItemStack equipment;
            if (!Equipment.TryGetValue(slot, out equipment) ||
                equipment == null)
            {
                return false;
            }

            if (Inventory.Store(equipment) != 0)
            {
                return false;
            }

            Equipment[slot] = null;
            return true;
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
