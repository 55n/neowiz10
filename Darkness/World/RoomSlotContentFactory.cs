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
        private readonly Dictionary<string, Func<TreasureChest>> treasureChestFactories;

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
            treasureChestFactories =
                new Dictionary<string, Func<TreasureChest>>
                {
                    { "room2_treasure_chest", CreateRoom2TreasureChest },
                    { "room3_treasure_chest", CreateRoom3TreasureChest }
                };
            factories = new Dictionary<RoomObjectType, Func<RoomSlotType, int, ISlotContent>>
            {
                { RoomObjectType.Monster, CreateMonster },
                { RoomObjectType.Trap, CreateTrap },
                { RoomObjectType.TreasureChest, CreateTreasureChest },
                { RoomObjectType.Pile, CreatePile }
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
                new List<string>
                {
                    "어둠 속에서 작은 그림자가 움직인다.",
                },
                3);
        }

        private Monster CreateRoom3HungryTroll()
        {
            return CreateMonster(
                "hungry_troll",
                new Inventory(0),
                new HungryTrollBehavior(),
                new List<string>
                {
                },
                3);
        }

        private Monster CreateMonster(
            string monsterTypeId,
            Inventory inventory,
            IMonsterBehavior behavior,
            List<string> encounterMessages,
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
                encounterMessages,
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

        private ISlotContent CreateTreasureChest(
            RoomSlotType slotType,
            int slotIndex)
        {
            Func<TreasureChest> createTreasureChest;
            if (!treasureChestFactories.TryGetValue(
                slotType.ObjectTypeId,
                out createTreasureChest))
            {
                throw new InvalidOperationException(
                    "Unknown treasure chest instance: " +
                    slotType.ObjectTypeId);
            }

            return createTreasureChest();
        }

        private TreasureChest CreateRoom2TreasureChest()
        {
            Inventory inventory = new Inventory(2);
            StoreItem(inventory, "magic_stone", 2);
            StoreItem(inventory, "ordinary_armor", 1);
            return new TreasureChest(8, 1, inventory);
        }

        private TreasureChest CreateRoom3TreasureChest()
        {
            Inventory inventory = new Inventory(1);
            StoreItem(inventory, "magic_stone", 1);
            return new TreasureChest(8, 1, inventory);
        }

        private ISlotContent CreatePile(
            RoomSlotType slotType,
            int slotIndex)
        {
            if (slotType.ObjectTypeId != "room4_loser_mark_pile")
            {
                throw new InvalidOperationException(
                    "Unknown pile instance: " +
                    slotType.ObjectTypeId);
            }

            Inventory inventory = new Inventory(1);
            StoreItem(inventory, "loser_mark", 1);
            return new Pile(inventory);
        }
    }
}
