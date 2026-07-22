using System;
using System.Collections.Generic;

namespace Darkness
{
    public class RoomSlotContentFactory
    {
        private readonly MonsterData monsterData;
        private readonly TrapData trapData;
        private readonly ItemData itemData;
        private readonly WolfPack room12WolfPack;
        private readonly Dictionary<RoomObjectType, Func<RoomSlotType, int, ISlotContent>> factories;
        private readonly Dictionary<string, Func<Monster>> monsterFactories;
        private readonly Dictionary<string, Func<TreasureChest>> treasureChestFactories;
        private readonly Dictionary<string, string> trapTypeIds;

        public RoomSlotContentFactory()
        {
            monsterData = new MonsterData();
            trapData = new TrapData();
            itemData = new ItemData();
            room12WolfPack = new WolfPack();
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
                { "room9_echo_bat_5", CreateRoom9EchoBat },
                { "room10_swamp_specter", CreateRoom10SwampSpecter },
                { "room12_frost_wolf_1", CreateRoom12FrostWolf },
                { "room12_frost_wolf_2", CreateRoom12FrostWolf },
                { "room12_frost_wolf_3", CreateRoom12FrostWolf },
                { "room16_web_1", CreateRoom16CeilingWeb },
                { "room16_web_2", CreateRoom16CeilingWeb },
                { "room16_web_3", CreateRoom16CeilingWeb },
                { "room16_web_4", CreateRoom16CeilingWeb },
                { "room16_web_5", CreateRoom16CeilingWeb }
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
                { RoomObjectType.Body, CreateBody },
                { RoomObjectType.Water, CreateWater },
                { RoomObjectType.Echo, CreateEcho },
                { RoomObjectType.Sand, CreateSand },
                { RoomObjectType.Prey, CreatePrey },
                { RoomObjectType.Egg, CreateEgg }
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
                    "어둠 속에서 작은 발소리가 빠르게 움직인다.",
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
                    "서로 다른 두 곳에서 작은 발소리가 부산하게 움직인다."
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
            Monster armorSpirit = CreateMonster(
                "bound_armor_spirit",
                new Inventory(0),
                new ArmorSpiritBehavior(),
                new List<string>());
            armorSpirit.ApplyEffect(
                new ActiveEffectFactory().Create(
                    "magic_stone_eater"));
            return armorSpirit;
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
            StoreItem(inventory, "mist_charm", 1);
            return new TreasureChest(8, 1, inventory);
        }

        private TreasureChest CreateRoom6TreasureChest1()
        {
            return CreateTreasureChestWithItem(
                "snowflake_pendant");
        }

        private Monster CreateRoom10SwampSpecter()
        {
            Inventory inventory = new Inventory(2);
            StoreItem(inventory, "freeze_core", 1);
            StoreItem(inventory, "magic_stone", 3);
            Monster monster = CreateMonster(
                "swamp_specter",
                inventory,
                new SwampSpecterBehavior(),
                new List<string>
                {
                    "멀리서 희미하게 첨벙이는 소리가 들린다."
                });
            monster.ApplyEffect(new WetEffect(
                new EffectData().EffectTypes["wet"]));
            return monster;
        }

        public Monster CreateRoom11DoorClingerPart(
            int slotIndex)
        {
            string monsterTypeId;
            IMonsterBehavior behavior;
            switch (slotIndex)
            {
                case 0:
                    monsterTypeId = "door_clinger_tentacle";
                    behavior = new DoorClingerPartBehavior(
                        DoorClingerPartRole.Tentacle);
                    break;
                case 1:
                    monsterTypeId = "door_clinger_leg";
                    behavior = new DoorClingerPartBehavior(
                        DoorClingerPartRole.Leg);
                    break;
                case 2:
                    monsterTypeId = "door_clinger_torso";
                    behavior = new DoorClingerTorsoBehavior(
                        CreateRoom11MagicCore);
                    break;
                case 3:
                    monsterTypeId = "door_clinger_antenna";
                    behavior = new DoorClingerPartBehavior(
                        DoorClingerPartRole.Antenna);
                    break;
                case 4:
                    monsterTypeId = "door_clinger_claw";
                    behavior = new DoorClingerPartBehavior(
                        DoorClingerPartRole.Claw);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        "slotIndex");
            }

            Monster monster = CreateMonster(
                monsterTypeId,
                new Inventory(0),
                behavior,
                new List<string>());
            monster.EnterCombat();
            ActiveEffect magicOverload =
                new ActiveEffectFactory().Create(
                    "magic_overload");
            for (int stackCount = 1;
                 stackCount < 9;
                 stackCount++)
            {
                magicOverload.AddStack();
            }
            monster.ApplyEffect(magicOverload);
            monster.ApplyEffect(
                new ActiveEffectFactory().Create("fixed"));
            return monster;
        }

        public MagicCore CreateRoom11MagicCore()
        {
            Inventory drops = new Inventory(2);
            StoreItem(drops, "magic_stone", 3);
            StoreItem(drops, "forgotten_spear", 1);
            return new MagicCore(12, 3, drops);
        }

        private Monster CreateRoom12FrostWolf()
        {
            Monster wolf = CreateMonster(
                "frost_wolf",
                new Inventory(0),
                new FrostWolfBehavior(room12WolfPack),
                new List<string>(),
                4);
            wolf.EnterAlertState();

            EffectType packTacticsType =
                new EffectData().EffectTypes["pack_tactics"];
            wolf.ApplyEffect(new PackTacticsEffect(
                packTacticsType,
                wolf,
                room12WolfPack));
            room12WolfPack.Add(wolf);
            return wolf;
        }

        private ISlotContent CreateEcho(
            RoomSlotType slotType,
            int slotIndex)
        {
            if (slotType.ObjectTypeId == null ||
                !slotType.ObjectTypeId.StartsWith("room13_echo_"))
            {
                throw new InvalidOperationException(
                    "Unknown echo instance: " +
                    slotType.ObjectTypeId);
            }

            return CreateMonster(
                "echo_mimic",
                new Inventory(0),
                new EchoMimicBehavior(),
                new List<string>());
        }

        private ISlotContent CreateSand(
            RoomSlotType slotType,
            int slotIndex)
        {
            if (slotType.ObjectTypeId == "room14_sand_3")
            {
                return CreateMonster(
                    "room14_vibration_worm",
                    new Inventory(0),
                    new VibrationWormBehavior(),
                    new List<string>());
            }

            if (slotType.ObjectTypeId == null ||
                !slotType.ObjectTypeId.StartsWith("room14_sand_"))
            {
                throw new InvalidOperationException(
                    "Unknown sand instance: " +
                    slotType.ObjectTypeId);
            }

            return new Sand(slotType.ObjectTypeId);
        }

        private ISlotContent CreatePrey(
            RoomSlotType slotType,
            int slotIndex)
        {
            switch (slotType.ObjectTypeId)
            {
                case "room15_bound_troll":
                    return new BoundPrey(
                        "묶인트롤",
                        "거대한 생물이 끈적한 실에 겹겹이 감겨 있다.",
                        "낮은 신음과 거친 숨소리만 돌아온다.");
                case "room15_bound_goblin":
                    return new BoundPrey(
                        "묶인고블",
                        "작은 생물이 입까지 실로 봉해진 채 떨고 있다.",
                        "막힌 입 너머로 다급한 소리가 새어 나온다.");
                case "room15_bound_wolf":
                    return new BoundPrey(
                        "묶인늑대",
                        "짐승의 털과 차가운 숨결이 손끝에 닿는다.",
                        "힘없는 으르렁거림이 들린다.");
                default:
                    throw new InvalidOperationException(
                        "Unknown prey instance: " +
                        slotType.ObjectTypeId);
            }
        }

        private ISlotContent CreateEgg(
            RoomSlotType slotType,
            int slotIndex)
        {
            if (slotType.ObjectTypeId !=
                    "room15_breath_hunter_egg_1" &&
                slotType.ObjectTypeId !=
                    "room15_breath_hunter_egg_2")
            {
                throw new InvalidOperationException(
                    "Unknown egg instance: " +
                    slotType.ObjectTypeId);
            }

            return new BreathHunterEgg(
                CreateRoom15Spiderling);
        }

        private Monster CreateRoom15Spiderling()
        {
            Monster spiderling = CreateMonster(
                "breath_hunter_spiderling",
                new Inventory(0),
                new SpiderlingBehavior(),
                new List<string>());
            spiderling.EnterCombat();
            return spiderling;
        }

        private Monster CreateRoom16CeilingWeb()
        {
            Monster web = CreateMonster(
                "ceiling_web",
                new Inventory(0),
                new CeilingWebBehavior(),
                new List<string>());
            web.EnterCombat();
            return web;
        }

        public Monster CreateRoom16Part(
            int slotIndex,
            BreathHunterBody body)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            string monsterTypeId;
            BreathHunterPartRole role;
            switch (slotIndex)
            {
                case 0:
                    monsterTypeId =
                        "breath_hunter_part_left_foreleg";
                    role = BreathHunterPartRole.LeftForeleg;
                    break;
                case 1:
                    monsterTypeId = "breath_hunter_part_head";
                    role = BreathHunterPartRole.Head;
                    break;
                case 2:
                    monsterTypeId = "breath_hunter_part_abdomen";
                    role = BreathHunterPartRole.Abdomen;
                    break;
                case 3:
                    monsterTypeId = "breath_hunter_part_spinneret";
                    role = BreathHunterPartRole.Spinneret;
                    break;
                case 4:
                    monsterTypeId =
                        "breath_hunter_part_right_foreleg";
                    role = BreathHunterPartRole.RightForeleg;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        "slotIndex");
            }

            Monster part = CreateMonster(
                monsterTypeId,
                new Inventory(0),
                new BreathHunterPartBehavior(body, role),
                new List<string>());
            part.EnterCombat();
            body.AddPart(role, part);
            return part;
        }

        private ISlotContent CreateWater(
            RoomSlotType slotType,
            int slotIndex)
        {
            return new Water(slotType.ObjectTypeId);
        }

        private TreasureChest CreateRoom6TreasureChest2()
        {
            return CreateTreasureChestWithItem("chain_sword");
        }

        private TreasureChest CreateRoom6TreasureChest3()
        {
            return CreateTreasureChestWithItem("spirit_armor");
        }

        private TreasureChest CreateRoom6TreasureChest4()
        {
            return CreateTreasureChestWithItem("memory_charm");
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

            if (slotType.ObjectTypeId ==
                "room11_magic_stone_pile")
            {
                inventory = new Inventory(1);
                StoreItem(inventory, "magic_stone", 3);
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
            int inventoryCapacity = slotType.ObjectTypeId ==
                "room8_antidote_skeleton"
                ? 2
                : 1;
            Inventory inventory = new Inventory(inventoryCapacity);
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
                    StoreItem(inventory, "bandage", 1);
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
