using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class10
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

        private List<Monster> monsterList;

        private MonsterManager()
        {
            monsterList = new List<Monster>();   
        }

        public void Create_Monster(string name, int HP)
        {
            monsterList.Add(new Monster(name, HP));
            Console.WriteLine($"{name} Monster를 추가했습니다");
        }

        public void All_Viewing_Monster()
        {
            for(int i = 0; i < monsterList.Count; i++)
            {
                monsterList[i].Viewing_Monster();
            }
        }
    }

    class Monster
    {
        private string Name;
        private int HP;

        public Monster(string name, int HP)
        {
            this.Name = name;
            this.HP = HP;
        }

        public void Viewing_Monster()
        {
            Console.WriteLine($"몬스터 [{Name} >> HP : {HP}]");
        }
    }
}
