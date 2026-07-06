using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// text rpg 만들기
// struct Monster; class Player 존재
// Player spec: 공격력, Hp, Mp, 닉네임, 스킬, 레벨, 경험치
// 일반 공격과 스킬 공격이 있음
// Mp물약과 Hp물약이 존재
// 스킬 사용 시 Mp 소모
// 몬스터 잡으면 경험치 얻어 레벨 업
// 레벨 업 시 체력 마나 회복 및 필요경험치 증가
// Monster spec: 공격력, Hp, Mp, 스킬
// 몬스터는 3종류
// 일정 확률로 스킬 공격
// 게임의 종료 조건
// 플레이어의 HP == 0 혹은 플레이어의 레벨 > 10 일 시 게임 종료
// (이하는 기획 필요)
// 상점 존재
// 인벤토리 존재
// 방식은 턴제가 좋을 듯

namespace Homework8
{
    enum ItemList
    {
        GOLD, HP_POTION, MP_POTION
    }

    enum CreatureState
    {
        LIVING, DEAD
    }

    class Item
    {
        public int id { get; }
        public string name { get; }
        public string description { get; }

        public Item(string name, string description)
        {
            this.id += 1;
            this.name = name;
            this.description = description;
        }
    }

    class Inventory
    {
        private Item[] items;
        private int inventorySize;

        public Inventory(int inventorySize)
        {
            this.inventorySize = inventorySize;
            this.items = new Item[this.inventorySize];
        }

        public void load(Item item)
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                {
                    if(this.items[i] != null)
                    {
                        this.items[i] = item;
                        break;
                    }
                    
                }
            }
        }

        public Item pick(int index)
        {
            if (this.items[index] == null)
            {
                Console.WriteLine("해당 칸에 아이템이 없습니다.");
            }

            return this.items[index];
        }
    }

    struct Monster
    {
        private string species;
        private int number;
        private string name;
        private int attackPoint;
        private int hitPoint;
        private int magicPoint;
        private int exp;
        private string[] skills;
        
        public void createSlug(int number)
        {
            this.species = "달팽이";
            this.number = number;
            this.attackPoint = 5;
            this.hitPoint = 20;
            this.magicPoint = 1;
            this.exp = 1;
            this.skills = new string[] { "회전 박치기" };
            this.name = this.species + this.number;
        }

        public void createSlime(int number)
        {
            this.species = "슬라임";
            this.attackPoint = 10;
            this.hitPoint = 40;
            this.magicPoint = 5;
            this.exp = 2;
            this.skills = new string[] { "산성 공격" };
            this.name = this.species + this.number;
        }

        public void createMushroom(int number)
        {
            this.species = "버섯";
            this.attackPoint = 20;
            this.hitPoint = 100;
            this.magicPoint = 10;
            this.exp = 5;
            this.skills = new string[] { "바닥부수기", "회복" };
            this.name = this.species + this.number;
        }

        public void attack()
        {
            Console.WriteLine($"{this.name + this.number} 의 공격");
        }

        public void getDamage(int attackPoint)
        {
            Console.WriteLine($"{this.name} 에게 {attackPoint} 의 피해를 줬다");
            this.hitPoint -= attackPoint;

            if (hitPoint <= 0)
            {
                die();
            }
        }

        public void die()
        {
            Console.WriteLine($"{this.name} 은 죽었다...");
        }
    }

    class Player
    {
        private string name;
        private int attackPoint;
        private int maxHitPoint;
        private int hitPoint;
        private int maxMagicPoint;
        private int magicPoint;
        private string[] skills;
        private int level;
        private int exp;
        private int requiredExp;
        private Inventory playerInventory;

        public Player(string name)
        {
            this.name = name;
            this.attackPoint = 10;
            this.maxHitPoint = 20;
            this.hitPoint = 20;
            this.maxMagicPoint = 3;
            this.magicPoint = 3;
            this.skills = new string[] { "발차기", "방어", "회복" };
            this.level = 1;
            this.exp = 0;
            this.requiredExp = 1;
            this.playerInventory = new Inventory(10);
        }

        public void levelUp(int exceededExp)
        {
            this.attackPoint += 10;
            this.hitPoint += 10;
            this.magicPoint += 1;
            this.level += 1;
            this.exp = exceededExp;
            this.requiredExp += 1;

            if(exceededExp > requiredExp)
            {
                exceededExp -= requiredExp;
                levelUp(exceededExp);
            }
        }

        public void attack(Monster monster)
        {
            monster.
        }
    }

    class Program
    {
        void GameLoop()
        {
            Console.WriteLine("[용사의 모험]");

            while (true)
            {

            }
        }
        
        static void Main(string[] args)
        {
            Program p = new Program();
            p.GameLoop();
        }
    }
}
