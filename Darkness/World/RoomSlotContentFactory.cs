using System;
using System.Collections.Generic;

namespace Darkness
{
    public class RoomSlotContentFactory
    {
        private readonly MonsterData monsterData;
        private readonly TrapData trapData;
        private readonly Dictionary<RoomObjectType, Func<RoomSlotType, int, ISlotContent>> factories;
        private readonly Dictionary<string, Func<IMonsterBehavior>> monsterBehaviors;

        public RoomSlotContentFactory()
        {
            monsterData = new MonsterData();
            trapData = new TrapData();
            monsterBehaviors = new Dictionary<string, Func<IMonsterBehavior>>
            {
                { "hungry_troll", () => new HungryTrollBehavior() }
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
            MonsterType monsterType =
                monsterData.MonsterTypes[slotType.ObjectTypeId];
            Func<IMonsterBehavior> createBehavior;
            IMonsterBehavior behavior = monsterBehaviors.TryGetValue(
                monsterType.Id,
                out createBehavior)
                ? createBehavior()
                : new DefaultMonsterBehavior();

            return new Monster(
                monsterType,
                new Inventory(0),
                new List<ActiveEffect>(),
                behavior);
        }

        private ISlotContent CreateTrap(RoomSlotType slotType, int slotIndex)
        {
            TrapType trapType = trapData.TrapTypes[slotType.ObjectTypeId];
            return new Trap(trapType);
        }
    }
}
