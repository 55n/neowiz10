using System;

namespace Darkness
{
    public class Item
    {
        public ItemType Type { get; private set; }

        public Item(ItemType type)
        {
            Type = type;
        }

        public void Transform(ItemType transformedType)
        {
            if (transformedType == null)
            {
                throw new ArgumentNullException("transformedType");
            }

            Type = transformedType;
        }
    }
}
