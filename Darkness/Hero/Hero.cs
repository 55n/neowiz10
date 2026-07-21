using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Hero : IDamageable, ISkillUser, IEffectTarget,
        IEquipmentUser, IPoisonable
    {
        public HeroType Type { get; private set; }
        public string Name { get { return Type.Name; } }
        public int CurrentHealth { get; private set; }
        public int CurrentFocus { get; private set; }
        public int Attack
        {
            get
            {
                return Math.Max(
                    0,
                    Type.Attack + GetEquippedAttack() +
                    GetEffectAttackBonus());
            }
        }
        public int Defense
        {
            get
            {
                return Math.Max(
                    0,
                    Type.Defense + GetEquippedDefense() +
                    GetEffectDefenseBonus());
            }
        }
        public int Accuracy { get { return Type.Accuracy; } }
        public int Evasion { get { return Type.Evasion; } }
        public Inventory Inventory { get; private set; }
        public Dictionary<EquipmentSlot, ItemStack> Equipment { get; private set; }
        public HashSet<string> LearnedSkillIds { get; private set; }
        public List<ActiveEffect> Effects { get; private set; }
        public bool CanAct { get { return CurrentHealth > 0; } }
        public bool CanBePoisoned { get { return true; } }

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
            LearnSkill(equippedItem.Type.BoundSkillId);
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

        public Item GetEquippedItem(EquipmentSlot slot)
        {
            ItemStack equipment;
            return Equipment != null &&
                   Equipment.TryGetValue(slot, out equipment) &&
                   equipment != null
                ? equipment.Item
                : null;
        }

        public bool RemoveEquippedItem(Item item)
        {
            if (item == null || Equipment == null)
            {
                return false;
            }

            foreach (EquipmentSlot slot in
                     new List<EquipmentSlot>(Equipment.Keys))
            {
                ItemStack equipment = Equipment[slot];
                if (equipment != null &&
                    ReferenceEquals(equipment.Item, item))
                {
                    Equipment[slot] = null;
                    return true;
                }
            }

            return false;
        }

        public void LearnSkill(string skillId)
        {
            if (!string.IsNullOrEmpty(skillId))
            {
                LearnedSkillIds.Add(skillId);
            }
        }

        public bool KnowsSkill(string skillId)
        {
            return !string.IsNullOrEmpty(skillId) &&
                   LearnedSkillIds.Contains(skillId);
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

        private int GetEquippedAttack()
        {
            ItemStack weapon;
            int weaponAttack = Equipment != null &&
                   Equipment.TryGetValue(
                       EquipmentSlot.Weapon,
                       out weapon) &&
                   weapon != null && weapon.Item != null
                ? weapon.Item.Attack
                : 0;

            return Math.Max(0, weaponAttack);
        }

        private int GetEquippedDefense()
        {
            ItemStack armor;
            return Equipment != null &&
                   Equipment.TryGetValue(
                       EquipmentSlot.Armor,
                       out armor) &&
                   armor != null && armor.Item != null
                ? armor.Item.Type.Defense
                : 0;
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

    }
}
