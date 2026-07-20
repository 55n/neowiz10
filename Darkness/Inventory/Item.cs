using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Item : IEffectTarget
    {
        public ItemType Type { get; private set; }
        public string Name { get { return Type.Name; } }
        public List<ActiveEffect> Effects { get; private set; }
        public int Attack
        {
            get
            {
                int attack = Type.Attack;
                foreach (ActiveEffect effect in Effects)
                {
                    attack = effect.ModifyWeaponAttackBonus(attack);
                }

                return Math.Max(0, attack);
            }
        }
        public int CurrentDurability { get; private set; }
        public bool UsesDurability
        {
            get { return Type.MaxDurability > 0; }
        }
        public bool IsBroken
        {
            get { return UsesDurability && CurrentDurability <= 0; }
        }

        public Item(ItemType type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            Type = type;
            Effects = new List<ActiveEffect>();
            CurrentDurability = type.MaxDurability;
        }

        public void ApplyEffect(ActiveEffect effect)
        {
            if (effect == null)
            {
                return;
            }

            ActiveEffect existing = Effects.Find(
                active => active.Type.Id == effect.Type.Id);
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
            Effects.RemoveAll(
                effect => effect.Type != null &&
                          effect.Type.Id == effectId);
        }

        public bool ConsumeDurability(int amount)
        {
            if (!UsesDurability || IsBroken || amount <= 0)
            {
                return false;
            }

            CurrentDurability = Math.Max(0, CurrentDurability - amount);
            return IsBroken;
        }

        public bool SetDurability(int durability)
        {
            if (!UsesDurability)
            {
                return false;
            }

            CurrentDurability = Math.Max(
                0,
                Math.Min(Type.MaxDurability, durability));
            return true;
        }

        public void Transform(ItemType transformedType)
        {
            if (transformedType == null)
            {
                throw new ArgumentNullException("transformedType");
            }

            Type = transformedType;
            CurrentDurability = transformedType.MaxDurability;
        }
    }
}
