using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    enum CreatureState
    {
        ALIVE, DEAD
    }

    interface ICreatureState
    {
        void Action();
    }

    class Alive : ICreatureState
    {
        private Creature _creature;

        public Alive(Creature creature)
        {
            _creature = creature;
        }

        public void Action()
        {
            _creature.creatureState = CreatureState.ALIVE;
            Console.WriteLine($"{_creature.name} 살아있음.");
        }
    }

    class Dead : ICreatureState
    {
        private Creature _creature;

        public Dead(Creature creature)
        {
            _creature = creature;
        }

        public void Action()
        {
            _creature.creatureState = CreatureState.DEAD;
            Console.WriteLine();
            Console.WriteLine($"{_creature.name} 죽었다...");
            Console.WriteLine();
        }
    }

}
