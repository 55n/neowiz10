using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice2
{
    class Program
    {
        static void Main(string[] args)
        {
            MonsterSpawner spawner = new MonsterSpawner();
            spawner.SpawnMonster();
        }
    }
}
