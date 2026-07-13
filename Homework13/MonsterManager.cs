using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework13
{
    class MonsterManager
    {
        private static MonsterManager _instance = null;

        public static MonsterManager Instance
        {
            get
            {
                if (_instance == null) _instance = new MonsterManager();
                return _instance;
            }
        }

        private List<Creature> _monsters;
        private Random _randomForMonster;
        private CreatureFactory _slugFactory;
        private CreatureFactory _slimeFactory;
        private CreatureFactory _mushMomFactory;

        private MonsterManager()
        {
            _monsters = new List<Creature>();
            _randomForMonster = new Random();
            _slugFactory = new SlugFactory();
            _slimeFactory = new SlimeFactory();
            _mushMomFactory = new MushMomFactory();
        }
        
        public List<Creature> AddMonsters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int species = _randomForMonster.Next() % 3;
                if (species == 0) _monsters.Add(_slugFactory.CreateCreature());
                else if (species == 1) _monsters.Add(_slimeFactory.CreateCreature());
                else if (species == 2) _monsters.Add(_mushMomFactory.CreateCreature());
            }

            return _monsters;
        }

        public void ClearMonsters()
        {
            _monsters.Clear();
        }
    }
}
