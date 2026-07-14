using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    abstract class Creature
    {
        public string name { get; protected set; }
        public int attackPoint { get; protected set; }
        public int maxHitPoint { get; protected set; }
        public int hitPoint { get; protected set; }
        public int maxMagicPoint { get; protected set; }
        public int magicPoint { get; protected set; }
        public Skill[] skills { get; protected set; }
        public int exp { get; protected set; }
        public Inventory inventory { get; protected set; }
        public Dictionary<CreatureState, ICreatureState> creatureStateDictionary { get; protected set; }
        public CreatureState creatureState;


        public Creature()
        {
            creatureStateDictionary = new Dictionary<CreatureState, ICreatureState>();
            creatureStateDictionary.Add(CreatureState.ALIVE, new Alive(this));
            creatureStateDictionary.Add(CreatureState.DEAD, new Dead(this));

            creatureStateDictionary[CreatureState.ALIVE].Action();
        }

        public abstract void attack(Creature creature);
        public virtual void getDamage(int damage)
        {
            this.hitPoint -= damage;
            Console.WriteLine();
            Console.WriteLine($"{this.name} 은(는) {damage} 의 피해를 입었다! (남은 체력: {this.hitPoint})");
            Console.WriteLine();

            if (hitPoint <= 0)
            {
                die();
            }
        }
        public virtual void die()
        {
            creatureStateDictionary[CreatureState.DEAD].Action();
        }

        public virtual void heal(int recovery)
        {
            this.hitPoint += recovery;
            if (this.hitPoint > this.maxHitPoint) this.hitPoint = this.maxHitPoint;
            Console.WriteLine();
            Console.WriteLine($"{this.name} 은(는) HP를 {recovery} 만큼 회복했다 (현재 HP:{this.hitPoint})");
            Console.WriteLine();
        }

        public virtual void awake(int recovery)
        {
            this.magicPoint += recovery;
            if (this.magicPoint > this.maxMagicPoint) this.magicPoint = this.maxMagicPoint;
            Console.WriteLine();
            Console.WriteLine($"{this.name} 은(는) MP를 {recovery} 만큼 회복했다 (현재 HP:{this.magicPoint})");
            Console.WriteLine();
        }

        public virtual void viewStat()
        {
            Console.WriteLine();
            Console.WriteLine($"######## {this.name} 스탯창 #########");
            Console.WriteLine($"HP: {this.hitPoint}/{this.maxHitPoint}");
            Console.WriteLine($"MP: {this.magicPoint}/{this.maxMagicPoint}");
        }
    }

    class Human : Creature
    {
        public int level { get; private set; }
        public int requiredExp { get; private set; }

        public Human(string name, Inventory inventory)
        {
            this.name = name;
            this.attackPoint = 1;
            this.maxHitPoint = 2;
            this.hitPoint = 2;
            this.maxMagicPoint = 3;
            this.magicPoint = 3;
            this.skills = new Skill[] { new Kick(), new Heal() };
            this.level = 1;
            this.exp = 0;
            this.requiredExp = 1;
            this.inventory = inventory;
        }

        public override void attack(Creature monster)
        {
            if (this.hitPoint <= 0) return;

            Console.Write("[1. 일반공격 | 2. 스킬사용]    선택하세요: ");
            int choice = int.Parse(Console.ReadLine());
            Console.WriteLine();

            if (choice == 1)
            {
                Console.WriteLine($"{monster.name} 을(를) 공격!");
                monster.getDamage(this.attackPoint);
                Console.WriteLine();
            }
            else if (choice == 2)
            {
                Console.WriteLine("어떤 스킬을 사용하시겠습니까?");
                Console.WriteLine("################################# 스킬 목록 #################################");
                for (int i = 0; i < this.skills.Length; i++)
                {
                    Console.WriteLine($"[{i} 번 스킬 | {this.skills[i].name} | {this.skills[i].description} | 소모 MP {this.skills[i].magicPointCost}]");
                }
                Console.WriteLine("#############################################################################");
                Console.Write("선택할 스킬: ");
                int skillIndex = int.Parse(Console.ReadLine());
                Console.WriteLine();

                skills[skillIndex].use(monster, this);
            }
        }

        public void levelUp(int exceededExp)
        {
            this.maxHitPoint += 10;
            this.maxMagicPoint += 1;
            this.hitPoint = maxHitPoint;
            this.magicPoint = maxMagicPoint;
            this.attackPoint += 10;
            this.level += 1;
            this.exp = exceededExp;
            this.requiredExp += 1;

            Console.WriteLine($"레벨 업!!!! 현재 레벨: {this.level}");

            if (exceededExp >= requiredExp)
            {
                exceededExp -= requiredExp;
                levelUp(exceededExp);
            }
        }

        public override void viewStat()
        {
            base.viewStat();
            Console.WriteLine($"LEVEL: {this.level}");
            Console.WriteLine($"EXP: {this.exp}");
            Console.WriteLine($"REQEXP: {this.requiredExp}");
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
                new MonsterType(0, "달펭이", 5, 20, 1, 1, new Skill[1] { new Sonic() }),
                new MonsterType(1, "슬라윔", 10, 40, 5, 2, new Skill[1] { new AcidBeam() }),
                new MonsterType(2, "버섯맘", 20, 100, 10, 5, new Skill[2] { new GroundCrusher(), new Heal() })
            };
        }
    }

    class Slug : Creature
    {
        private Random random;

        public Slug()
        {
            random = new Random();
            MonsterType m = new MonsterData().monsterList[0];

            this.name = m.species;
            this.maxHitPoint = m.hitPoint;
            this.maxMagicPoint = m.magicPoint;
            this.hitPoint = m.hitPoint;
            this.magicPoint = m.magicPoint;
            this.exp = m.exp;
            this.attackPoint = m.attackPoint;
            this.skills = m.skills;

            this.inventory = new Inventory(1);
            this.inventory.load(new Gold());

        }

        public override void attack(Creature player)
        {
            if (this.hitPoint <= 0) return;

            int roll = random.Next() % 2;

            if (roll == 0)
            {
                Console.WriteLine($"{player.name} 을(를) 공격!");
                player.getDamage(this.attackPoint);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                skills[0].use(player, this);
            }
        }
    }

    class Slime : Creature
    {
        private Random random;

        public Slime()
        {
            random = new Random();
            MonsterType m = new MonsterData().monsterList[1];

            this.name = m.species;
            this.maxHitPoint = m.hitPoint;
            this.maxMagicPoint = m.magicPoint;
            this.hitPoint = m.hitPoint;
            this.magicPoint = m.magicPoint;
            this.exp = m.exp;
            this.attackPoint = m.attackPoint;
            this.skills = m.skills;
            this.inventory = new Inventory(1);
            this.inventory.load(new Gold());
            this.inventory.load(new Gold());
            this.inventory.load(new Gold());
        }

        public override void attack(Creature player)
        {
            if (this.hitPoint <= 0) return;

            int roll = random.Next() % 2;

            if (roll == 0)
            {
                Console.WriteLine($"{player.name} 을(를) 공격!");
                player.getDamage(this.attackPoint);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                skills[0].use(player, this);
            }
        }
    }

    class MushMom : Creature
    {
        private Random random;

        public MushMom()
        {
            random = new Random();
            MonsterType m = new MonsterData().monsterList[2];

            this.name = m.species;
            this.maxHitPoint = m.hitPoint;
            this.maxMagicPoint = m.magicPoint;
            this.hitPoint = m.hitPoint;
            this.magicPoint = m.magicPoint;
            this.exp = m.exp;
            this.attackPoint = m.attackPoint;
            this.skills = m.skills;
            this.inventory = new Inventory(1);
            this.inventory.load(new Gold());
            this.inventory.load(new Gold());
            this.inventory.load(new Gold());
            this.inventory.load(new Gold());
            this.inventory.load(new Gold());
            this.inventory.load(new Gold());
        }

        public override void attack(Creature player)
        {
            if (this.hitPoint <= 0) return;

            int roll = random.Next() % 2;

            if (roll == 0)
            {
                Console.WriteLine($"{player.name} 을(를) 공격!");
                player.getDamage(this.attackPoint);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                skills[0].use(player, this);
            }
        }
    }
}
