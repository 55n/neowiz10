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
        private readonly Dictionary<string, string> trapTypeIds;

        public RoomSlotContentFactory()
        {
            monsterData = new MonsterData();
            trapData = new TrapData();
            itemData = new ItemData();
            monsterFactories = new Dictionary<string, Func<Monster>>
            {
                { "room1_lost_goblin", CreateRoom1LostGoblin },
                { "room3_hungry_troll", CreateRoom3HungryTroll },
                { "room5_goblin_1", CreateRoom5BaitGoblin },
                { "room5_goblin_2", CreateRoom5RagGoblin },
                { "room6_armor_spirit", CreateRoom6ArmorSpirit },
                { "room9_echo_bat_1", CreateRoom9EchoBat },
                { "room9_echo_bat_2", CreateRoom9EchoBat },
                { "room9_echo_bat_3", CreateRoom9EchoBat },
                { "room9_echo_bat_4", CreateRoom9EchoBat },
                { "room9_echo_bat_5", CreateRoom9EchoBat }
            };
            treasureChestFactories =
                new Dictionary<string, Func<TreasureChest>>
                {
                    { "room2_treasure_chest", CreateRoom2TreasureChest },
                    { "room3_treasure_chest", CreateRoom3TreasureChest },
                    { "room6_treasure_chest_1", CreateRoom6TreasureChest1 },
                    { "room6_treasure_chest_2", CreateRoom6TreasureChest2 },
                    { "room6_treasure_chest_3", CreateRoom6TreasureChest3 },
                    { "room6_treasure_chest_4", CreateRoom6TreasureChest4 }
                };
            trapTypeIds = new Dictionary<string, string>
            {
                { "room2_arrow_trap", "arrow_trap" }
            };
            factories = new Dictionary<RoomObjectType, Func<RoomSlotType, int, ISlotContent>>
            {
                { RoomObjectType.Monster, CreateMonster },
                { RoomObjectType.Trap, CreateTrap },
                { RoomObjectType.TreasureChest, CreateTreasureChest },
                { RoomObjectType.Pile, CreatePile },
                { RoomObjectType.Coffin, CreateCoffin },
                { RoomObjectType.Body, CreateBody }
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
            string trapTypeId;
            if (!trapTypeIds.TryGetValue(
                    slotType.ObjectTypeId,
                    out trapTypeId))
            {
                throw new InvalidOperationException(
                    "Unknown trap instance: " +
                    slotType.ObjectTypeId);
            }

            TrapType trapType = trapData.TrapTypes[trapTypeId];
            return new Trap(trapType);
        }

        private Monster CreateRoom5BaitGoblin()
        {
            Inventory inventory = new Inventory(1);
            StoreItem(inventory, "monster_bait", 1);
            return CreateRoom5Goblin(
                inventory,
                new List<string>
                {
                    "어둠 속에서 두 개의 작은 그림자가 움직인다."
                });
        }

        private Monster CreateRoom5RagGoblin()
        {
            Inventory inventory = new Inventory(1);
            StoreItem(inventory, "rag_armor", 1);
            return CreateRoom5Goblin(
                inventory,
                new List<string>());
        }

        private Monster CreateRoom5Goblin(
            Inventory inventory,
            List<string> encounterMessages)
        {
            return CreateMonster(
                "lost_goblin",
                inventory,
                new ConfidentGoblinBehavior(),
                encounterMessages,
                3);
        }

        private Monster CreateRoom6ArmorSpirit()
        {
            return CreateMonster(
                "bound_armor_spirit",
                new Inventory(0),
                new ArmorSpiritBehavior(),
                new List<string>());
        }

        private Monster CreateRoom9EchoBat()
        {
            return CreateMonster(
                "echo_bat",
                new Inventory(0),
                new EchoBatBehavior(3),
                new List<string>());
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

        private TreasureChest CreateRoom6TreasureChest1()
        {
            return CreateTreasureChestWithItem("whetstone");
        }

        private TreasureChest CreateRoom6TreasureChest2()
        {
            return CreateTreasureChestWithItem("ordinary_sword");
        }

        private TreasureChest CreateRoom6TreasureChest3()
        {
            return CreateTreasureChestWithItem("ordinary_armor");
        }

        private TreasureChest CreateRoom6TreasureChest4()
        {
            return CreateTreasureChestWithItem("guardian_charm");
        }

        private TreasureChest CreateTreasureChestWithItem(string itemId)
        {
            Inventory inventory = new Inventory(1);
            StoreItem(inventory, itemId, 1);
            return new TreasureChest(8, 1, inventory);
        }

        private ISlotContent CreatePile(
            RoomSlotType slotType,
            int slotIndex)
        {
            Inventory inventory;
            if (slotType.ObjectTypeId == "room4_loser_mark_pile")
            {
                inventory = new Inventory(1);
                StoreItem(inventory, "loser_mark", 1);
                return new Pile(inventory);
            }

            if (slotType.ObjectTypeId == "room5_supply_pile")
            {
                inventory = new Inventory(1);
                StoreItem(inventory, "pocket", 1);
                return new Pile(inventory);
            }

            throw new InvalidOperationException(
                "Unknown pile instance: " +
                slotType.ObjectTypeId);
        }

        private ISlotContent CreateCoffin(
            RoomSlotType slotType,
            int slotIndex)
        {
            if (slotType.ObjectTypeId == null ||
                !slotType.ObjectTypeId.StartsWith("room7_coffin_") ||
                slotIndex < 0 || slotIndex > 4)
            {
                throw new InvalidOperationException(
                    "Unknown coffin instance: " +
                    slotType.ObjectTypeId);
            }

            Monster occupant = CreateMonster(
                "coffin_undead",
                new Inventory(0),
                new CoffinUndeadBehavior(),
                new List<string>());
            return new Coffin(
                CoffinMessages.Hint(slotIndex),
                occupant);
        }

        private ISlotContent CreateBody(
            RoomSlotType slotType,
            int slotIndex)
        {
            Inventory inventory = new Inventory(1);
            PoisonFogTrap hiddenTrap = null;
            switch (slotType.ObjectTypeId)
            {
                case "room8_trapped_skeleton":
                    StoreItem(inventory, "magic_stone", 1);
                    hiddenTrap = new PoisonFogTrap(
                        trapData.TrapTypes["poison_fog_trap"]);
                    break;
                case "room8_antidote_skeleton":
                    StoreItem(inventory, "antidote", 1);
                    break;
                case "room8_skeleton_3":
                    StoreItem(inventory, "monster_bait", 1);
                    break;
                case "room8_skeleton_4":
                    StoreItem(inventory, "magic_stone", 2);
                    break;
                case "room8_skeleton_5":
                    StoreItem(inventory, "whetstone", 1);
                    break;
                default:
                    throw new InvalidOperationException(
                        "Unknown body instance: " +
                        slotType.ObjectTypeId);
            }

            return new Body(8, 0, inventory, hiddenTrap);
        }
    }
}
