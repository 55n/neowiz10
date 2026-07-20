using System;
using System.Collections.Generic;

namespace Darkness
{
    public class RoomSlotContentFactory
    {
        private readonly MonsterData monsterData;
        private readonly TrapData trapData;
        private readonly ItemData itemData;
        private readonly Dictionary<RoomObjectType, Func<RoomSlotType, int, ISlotContent>> factories;
        private readonly Dictionary<string, Func<Monster>> monsterFactories;

        public RoomSlotContentFactory()
        {
            monsterData = new MonsterData();
            trapData = new TrapData();
            itemData = new ItemData();
            monsterFactories = new Dictionary<string, Func<Monster>>
            {
                { "room1_lost_goblin", CreateRoom1LostGoblin },
                { "room3_hungry_troll", CreateRoom3HungryTroll }
            };
            factories = new Dictionary<RoomObjectType, Func<RoomSlotType, int, ISlotContent>>
            {
                { RoomObjectType.Monster, CreateMonster },
                { RoomObjectType.Trap, CreateTrap }
            };
        }

        public ISlotContent Create(RoomSlotType slotType, int slotIndex)
        {
            Func<RoomSlotType, int, ISlotContent> factory;
            return factories.TryGetValue(slotType.ObjectType, out factory)
                ? factory(slotType, slotIndex)
                : null;
        }

        private ISlotContent CreateMonster(RoomSlotType slotType, int slotIndex)
        {
            Func<Monster> createMonster;
            if (!monsterFactories.TryGetValue(
                slotType.ObjectTypeId,
                out createMonster))
            {
                throw new InvalidOperationException(
                    "Unknown monster instance: " +
                    slotType.ObjectTypeId);
            }

            return createMonster();
        }

        private Monster CreateRoom1LostGoblin()
        {
            Inventory inventory = new Inventory(1);
            StoreItem(inventory, "monster_bait", 1);
            return CreateMonster(
                "lost_goblin",
                inventory,
                new LostGoblinBehavior(),
                2);
        }

        private Monster CreateRoom3HungryTroll()
        {
            return CreateMonster(
                "hungry_troll",
                new Inventory(0),
                new HungryTrollBehavior());
        }

        private Monster CreateMonster(
            string monsterTypeId,
            Inventory inventory,
            IMonsterBehavior behavior,
            int detectionDelayTurns = 0)
        {
            MonsterType monsterType;
            if (!monsterData.MonsterTypes.TryGetValue(
                monsterTypeId,
                out monsterType))
            {
                throw new InvalidOperationException(
                    "Unknown monster type: " +
                    monsterTypeId);
            }

            return new Monster(
                monsterType,
                inventory,
                new List<ActiveEffect>(),
                behavior,
                detectionDelayTurns);
        }

        private void StoreItem(
            Inventory inventory,
            string itemId,
            int amount)
        {
            ItemType itemType;
            if (!itemData.ItemTypes.TryGetValue(
                itemId,
                out itemType))
            {
                throw new InvalidOperationException(
                    "Unknown item: " + itemId);
            }

            int remaining = inventory.Store(
                new ItemStack(
                    new Item(itemType),
                    amount));
            if (remaining != 0)
            {
                throw new InvalidOperationException(
                    "Monster inventory is too small: " +
                    itemId);
            }
        }

        private ISlotContent CreateTrap(RoomSlotType slotType, int slotIndex)
        {
            TrapType trapType = trapData.TrapTypes[slotType.ObjectTypeId];
            return new Trap(trapType);
        }
    }
}
