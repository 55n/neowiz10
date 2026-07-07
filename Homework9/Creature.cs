using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework9
{
    abstract class Creature
    {
        public string name { get; private set; }

        public abstract void attack(Creature creature);
        public abstract void getDamage(int damage);
        public virtual void die()
        {
            Console.WriteLine($"{this.name} 은(는) 죽었다...");
        }
    }

    class Humanoid : Creature
    {
        public int attackPoint { get; private set; }
        public int maxHitPoint { get; private set; }
        public int hitPoint { get; private set; }
        public int maxMagicPoint { get; private set; }
        public int magicPoint { get; private set; }
        public Skill[] skills { get; private set; }
        public int level { get; private set; }
        public int exp { get; private set; }
        public int requiredExp { get; private set; }
        public Inventory playerInventory { get; private set; }

        public override void attack(Creature creature)
        {
            Console.Write("1. 일반공격 | 2. 스킬사용    : ");
            int choice = int.Parse(Console.ReadLine());

            if(choice == 1)
            {
                Console.WriteLine($"{creature.name} 을(를) 공격!");
                creature.getDamage(this.attackPoint);
            }
            else
            {
                Console.WriteLine($"어떤 스킬을 사용하시겠습니까?");
                Console.WriteLine("############# 스킬 목록 #############");
                for (int i = 0; i < this.skills.Length; i++)
                {
                    Console.WriteLine($"{i} 번 스킬 | {this.skills[i].skillName} | {this.skills[i].skillDescription} | 소모 MP 1");
                }
                Console.WriteLine("###################################");
            }
        }

        public override void getDamage(int damage)
        {
            Console.WriteLine($"{damage} 의 피해를 받았다");
        }

    }

    class MonsterType
    {
        public int typeId { get; }
        public string species { get; }
        public int attackPoint { get; }
        public int hitPoint { get; }
        public int magicPoint { get; }
        public int exp { get; }
        public Skill[] skills { get; }

        public MonsterType(int typeId, string species, int attackPoint, int hitPoint, int magicPoint, int exp, Skill[] skills)
        {
            this.typeId = typeId;
            this.species = species;
            this.attackPoint = attackPoint;
            this.hitPoint = hitPoint;
            this.magicPoint = magicPoint;
            this.exp = exp;
            this.skills = skills;
        }
    }

    class MonsterData
    {
        public MonsterType[] monsterList;

        public MonsterData()
        {
            monsterList = new MonsterType[3]
            {
                new MonsterType(0, "달펭이", 5, 20, 1, 1, new Skill[1] { new Skill(0) }),
                new MonsterType(1, "슬라윔", 10, 40, 5, 2, new Skill[1] { new Skill(3) }),
                new MonsterType(2, "버섯맘", 20, 100, 10, 5, new Skill[2] { new Skill(4), new Skill(1) })
            };

        }
    }

    class Monster : Creature
    {
        public override void attack(Creature creature)
        {

        }

        public override void getDamage(int damage)
        {

        }
    }
}
