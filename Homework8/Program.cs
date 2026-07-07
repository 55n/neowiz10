using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
// 플레이어의 HP == 0 혹은 플레이어의 레벨 > 10 일 시 게임 종료
// (이하는 기획 필요)
// 상점 존재
// 인벤토리 존재
// 방식은 턴제가 좋을 듯

namespace Homework8
{
    class ItemType
    {
        public int typeId { get; }
        public string name { get; }
        public string description { get; }
        public int maxAmount { get; }
        public int price { get; }
        public ItemType(int typeId, string name, string description, int maxAmount, int price)
        {
            this.typeId = typeId;
            this.name = name;
            this.description = description;
            this.maxAmount = maxAmount;
            this.price = price;
        }
    }

    class ItemData
    {
        public ItemType[] itemType { get; }

        public ItemData()
        {
            itemType = new ItemType[3]
            {
                new ItemType(0, "골드", "화폐", 9999, 1),
                new ItemType(1, "HP 포션", "HP를 10% 회복시킨다", 10, 10),
                new ItemType(2, "MP 포션", "MP를 1 회복시킨다", 10, 10),
            };
        }
    }

    class Item
    {
        public ItemType itemType { get; }

        public Item(int typeId)
        {
            ItemData itemData = new ItemData();
            itemType = itemData.itemType[typeId];
        }
    }

    class ItemStack
    {
        public Item[] itemStack;

        public ItemStack(Item item)
        {
            itemStack = new Item[item.itemType.maxAmount];
        }
    }

    class Inventory
    {
        private ItemStack[] inventory;
        private int inventorySize;

        public Inventory(int inventorySize)
        {
            this.inventorySize = inventorySize;
            this.inventory = new ItemStack[this.inventorySize];
        }

        public bool load(Item item)
        {
            for (int i = 0; i < this.inventory.Length; i++)
            {
                // 1. 있는지 확인
                if (this.inventory[i] == null) // 없으면 인벤토리에 스택을 새로 생성하여 아이템 적재
                {
                    this.inventory[i] = new ItemStack(item);
                    this.inventory[i].itemStack[0] = item;
                    return true;
                }
                // 2. 있으면 같은 타입인지 확인
                else if (this.inventory[i].itemStack[0].itemType.typeId == item.itemType.typeId)
                {
                    // 3. 타입이 같으면 슬롯에 빈 공간이 있는지 확인
                    for (int j = 0; j < this.inventory[i].itemStack.Length; j++)
                    {
                        if (this.inventory[i].itemStack[j] == null) // 빈 공간이 있으면 적재
                        {
                            this.inventory[i].itemStack[j] = item;
                            return true;
                        }
                    }
                }
            }
            // 4. 스택에 빈 공간도 없고 남는 슬롯도 없으면 적재 불가
            return false;
        }

        public Item[] pick(int index, int amount)
        {
            int lastIndex = 0;

            if (this.inventory[index] == null) // 빈 슬롯을 고르면 안 됨
            {
                return null;
            }

            if (amount > this.inventory[index].itemStack.Length) // 맥스 스택 보다 많은걸 요구해도 안 됨
            {
                return null;
            }

            for (int i = 0; i < this.inventory[index].itemStack.Length; i++)
            {
                if (this.inventory[index].itemStack[i] == null)
                {
                    lastIndex = i - 1;

                    if (amount > i)
                    {
                        return null;
                    }

                    break;
                }

                if (i == this.inventory[index].itemStack.Length - 1)
                {
                    lastIndex = i;

                    if (amount > this.inventory[index].itemStack.Length)
                    {
                        return null;
                    }
                }
            }

            // 위 조건을 통과한 경우 아이템 반환
            Item[] items = new Item[amount];
            for (int i = 0; i < amount; i++)
            {
                items[i] = this.inventory[index].itemStack[lastIndex - i];
                this.inventory[index].itemStack[lastIndex - i] = null;

                if (this.inventory[index].itemStack[0] == null)
                {
                    this.inventory[index] = null;
                }
            }
            return items;
        }

        public void veiwInventory()
        {
            Console.WriteLine($"########### 인벤토리 ({this.inventorySize} 칸) ###########");
            for (int i = 0; i < this.inventory.Length; i++)
            {
                if(this.inventory[i] == null)
                {
                    Console.WriteLine($"{i}번 슬롯     비어있음");
                }
                else
                {
                    int count;
                    for (count = 0; count < this.inventory[i].itemStack.Length; count++) 
                    { 
                        if (this.inventory[i].itemStack[count] == null) break; 
                    }
                    
                    Console.WriteLine($"{i}번 슬롯     {this.inventory[i].itemStack[0].itemType.name} : {count} 개  {this.inventory[i].itemStack[0].itemType.price}G");
                }
            }
            Console.WriteLine("########################################");

        }
    }

    class Trade
    {
        public Inventory inventory1 { get; private set; }
        public Inventory inventory2 {  get; private set; }

        public Trade(Inventory sellerInventory, Inventory buyerInventory)
        {
            this.inventory1 = sellerInventory;
            this.inventory2 = buyerInventory;
        }

        public bool playerTradeItems(int index, int amount)
        {
            Item[] items = this.inventory1.pick(index, amount);

            int price = items[0].itemType.price * amount;

            Item[] gold = this.inventory2.pick(0, price);

            if (gold == null) 
            {
                Console.WriteLine($"돈이 부족하다");

                for(int i = 0; i < items.Length; i++)
                {
                    inventory1.load(items[i]);
                }

                return false;
            }

            for (int i = 0; i < gold.Length;i++)
            {
                inventory1.load(gold[i]);
            }

            for (int i = 0; i < items.Length; i++)
            {
                inventory2.load(items[i]);
            }

            return true;
            
        }
    }

    enum CreatureState
    {
        LIVING, DEAD
    }

    struct Monster
    {
        public string species { get; private set; }
        public int number { get; private set; }
        public string name { get; private set; }
        public int attackPoint { get; private set; }
        public int hitPoint { get; private set; }
        public int magicPoint { get; private set; }
        public int exp { get; private set; }
        public string[] skills { get; private set; }
        public CreatureState state { get; private set; }

        public Monster createSlug(int number)
        {
            this.species = "달펭이";
            this.number = number;
            this.attackPoint = 5;
            this.hitPoint = 20;
            this.magicPoint = 1;
            this.exp = 10;
            this.skills = new string[] { "회전 박치기" };
            this.name = this.species + this.number;
            this.state = CreatureState.LIVING;

            return this;
        }

        public Monster createSlime(int number)
        {
            this.species = "슬라윔";
            this.number = number;
            this.attackPoint = 10;
            this.hitPoint = 40;
            this.magicPoint = 5;
            this.exp = 20;
            this.skills = new string[] { "산성 공격" };
            this.name = this.species + this.number;
            this.state = CreatureState.LIVING;

            return this;
        }

        public Monster createMushroom(int number)
        {
            this.species = "버섯맘";
            this.number = number;
            this.attackPoint = 20;
            this.hitPoint = 100;
            this.magicPoint = 10;
            this.exp = 5;
            this.skills = new string[] { "바닥부수기", "회복" };
            this.name = this.species + this.number;
            this.state = CreatureState.LIVING;

            return this;
        }

        public void attack(Player player)
        {
            Console.WriteLine($"{this.name} 의 공격");
            player.getDamage(this.attackPoint);
        }

        public void skillAttack(Player player)
        {
            switch (this.species)
            {
                case "달펭이":
                    Console.WriteLine($"{this.name} 의 {this.skills[0]} 공격!");
                    player.getDamage(this.attackPoint * 2);
                    break;
                case "슬라윔":
                    Console.WriteLine($"{this.name} 의 {this.skills[0]} 공격!");
                    player.getDamage(this.attackPoint * 2);
                    break;
                case "버섯맘":
                    if (this.hitPoint < 40)
                    {
                        Console.WriteLine($"{this.name} 의 {this.skills[1]}!");
                        this.hitPoint += 30;
                    }
                    else
                    {
                        Console.WriteLine($"{this.name} 의 {this.skills[0]} 공격!");
                        player.getDamage(this.attackPoint * 2);
                    }
                    break;
            }
        }

        public void getDamage(int attackPoint)
        {
            this.hitPoint -= attackPoint;

            Console.WriteLine($"{this.name} 에게 {attackPoint} 의 피해를 줬다");

            if (hitPoint <= 0)
            {
                die();
            }
        }

        public void die()
        {
            Console.WriteLine($"{this.name} 은 죽었다...");
            this.state = CreatureState.DEAD;
        }
    }

    class Player
    {
        public string name { get; private set; }
        public int attackPoint { get; private set; }
        public int maxHitPoint { get; private set; }
        public int hitPoint { get; private set; }
        public int maxMagicPoint { get; private set; }
        public int magicPoint { get; private set; }
        public string[] skills { get; private set; }
        public int level { get; private set; }
        public int exp { get; private set; }
        public int requiredExp { get; private set; }
        public Inventory playerInventory { get; private set; }
        public CreatureState state { get; private set; }

        public Player(string name, Inventory inventory)
        {
            this.name = name;
            this.attackPoint = 1000;
            this.maxHitPoint = 2000;
            this.hitPoint = 2000;
            this.maxMagicPoint = 3;
            this.magicPoint = 3;
            this.skills = new string[] { "발차기", "회복" };
            this.level = 1;
            this.exp = 0;
            this.requiredExp = 1;
            this.playerInventory = inventory;
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

        public Monster attack(Monster monster)
        {
            monster.getDamage(this.attackPoint);
            return monster;
        }

        public Monster skillAttack(Monster monster)
        {
            if (this.magicPoint > 0) 
            {
                Random random = new Random();

                int randomResult = random.Next() % 2;

                Console.WriteLine($"당신은 {this.skills[randomResult]}를 사용했다!");

                if (randomResult == 0)
                {
                    monster.getDamage(this.attackPoint * 2);
                }
                else if (randomResult == 1)
                {
                    this.hitPoint += 10;
                }

                this.magicPoint--;
            }
            else
            {
                Console.WriteLine("Mp가 없어요");
            }

            return monster;
        }

        public void getDamage(int attackPoint)
        {
            this.hitPoint -= attackPoint;

            Console.WriteLine($"{attackPoint}의 데미지를 받았다. 남은 체력은 {this.hitPoint}");

            if (hitPoint <= 0)
            {
                die();
            }
        }

        public void die()
        {
            Console.WriteLine("당신은 죽어버렸다...");
            this.state = CreatureState.DEAD;
        }

        public void viewPlayerStat()
        {
            Console.WriteLine("############### 플레이어 스탯창 ################");
            Console.WriteLine($"Level: {this.level}");
            Console.WriteLine($"ReqExp: {this.requiredExp}");
            Console.WriteLine($"Exp: {this.exp}");
        }
    }

    class Stage
    {
        public int stageId { get; }
        public Monster[] monsters { get; private set; }
        public bool isCompleted { get; private set; }


        public Stage(Monster[] monsters)
        {
            this.stageId++;
            this.monsters = monsters;
            this.isCompleted = false;
        }


        public void judge(Player player)
        {
            int killCount = 0;
            int reward = 0;

            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i].state == CreatureState.DEAD)
                {
                    reward += monsters[i].exp;
                    killCount++;
                }
            }

            if (killCount >= monsters.Length)
            {
                Console.WriteLine($"스테이지 클리어 보상을 받습니다: {reward}exp");
                if (player.requiredExp <= reward) player.levelUp(reward - player.requiredExp);
                this.isCompleted = true;
            }
        }
    }
    class GameMaster
    {
        public Stage createStage()
        {
            Random random = new Random(); // 레벨 디자인 생략

            int monsterNumber = random.Next() % 3 + 1;

            Monster[] monsters = new Monster[monsterNumber];

            for(int i = 0; i < monsters.Length; i++)
            {
                int species = random.Next() % 3;

                switch (species)
                {
                    case 0:
                        monsters[i].createSlug(i);
                        break;
                    case 1:
                        monsters[i].createSlime(i);
                        break;
                    case 2:
                        monsters[i].createMushroom(i);
                        break;
                }
            }

            return new Stage(monsters);
        }

    }

    class Program
    {
        void GameLoop()
        {


            Console.WriteLine("###############################################################");
            Console.WriteLine("##################[    매이풀 용사의 모험   ]##################");
            Console.WriteLine("###############################################################");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("??? : 이름 적어라");
            Console.Write("당신의 이름: ");
            string name = Console.ReadLine();

            Inventory playerInventory = new Inventory(10);
            for(int i = 0; i < 20; i++) { playerInventory.load(new Item(0)); }
            playerInventory.load(new Item(1));
            playerInventory.load(new Item(2));

            Inventory merchantInventroy = new Inventory(10);
            for (int i = 0; i < 500; i++) { merchantInventroy.load(new Item(0)); }
            for (int i = 0; i < 30; i++) { merchantInventroy.load(new Item(1)); }
            for (int i = 0; i < 30; i++) { merchantInventroy.load(new Item(2)); }

            Player player = new Player(name, playerInventory);
            Player merchant = new Player("상점주인", merchantInventroy);

            Console.WriteLine();
            Console.WriteLine($"??? : {player.name} 사냥을 시작해라");
            Console.WriteLine();

            GameMaster gm  = new GameMaster();

            while (true)
            {
                // 1. 전투와 상점 중 선택지가 주어짐
                Console.WriteLine("1. 전투 | 2. 상점 | 3. 상태창");
                int playerAnswer = int.Parse(Console.ReadLine());

                // 2. 전투를 선택한 경우 몬스터와 전투
                if (playerAnswer == 1)
                {
                    Random random = new Random();
                    Stage s = gm.createStage();

                    while (player.hitPoint > 0)
                    {

                        Console.WriteLine($"[{player.name} 님의 턴]");
                        Console.WriteLine("1. 일반 공격 | 2. 스킬 공격");
                        playerAnswer = int.Parse(Console.ReadLine());

                        Console.Write($"누굴 공격할까?");
                        for (int i = 0; i < s.monsters.Length; i++)
                        {
                            if (s.monsters[i].state == CreatureState.LIVING)
                            {
                                Console.Write($"{i}){s.monsters[i].name}    ");
                            }
                        }
                        Console.Write(" : ");
                        int playerSelect = int.Parse(Console.ReadLine());

                        if (playerAnswer == 1)
                        {
                            Monster m = player.attack(s.monsters[playerSelect]);
                            s.monsters[playerSelect] = m;
                        }
                        else
                        {
                            Monster m = player.skillAttack(s.monsters[playerSelect]);
                            s.monsters[playerSelect] = m;
                        }

                        s.judge(player);

                        if (s.isCompleted)
                        {
                            break;
                        }

                        int monsterAction = random.Next() % 2;
                        for (int i = 0; i < s.monsters.Length; i++)
                        {
                            if (s.monsters[i].state == CreatureState.LIVING)
                            {
                                if (monsterAction == 0) s.monsters[i].attack(player);
                                else s.monsters[i].skillAttack(player);
                            }
                        }
                    }

                    if (player.hitPoint <= 0)
                    {
                        Console.WriteLine("장렬하게 전사했다...");
                        Console.WriteLine("게임오버");
                        break;
                    }
                    else if (player.level > 10)
                    {
                        Console.WriteLine("엄청난 업적을 세웠다!");
                        Console.WriteLine("호화스러운 은퇴를 했다.");
                        Console.WriteLine("게임 클리어");
                    }
                }
                else if (playerAnswer == 2)
                {
                    player.playerInventory.veiwInventory();
                    Console.WriteLine();
                    Console.WriteLine("상점에 어서오시오. 무슨 물건을 사려고 하시나?");
                    Console.WriteLine();
                    merchant.playerInventory.veiwInventory();

                    Console.Write("index 입력: ");
                    int index = int.Parse(Console.ReadLine());
                    Console.Write("수량 입력: ");
                    int amount = int.Parse(Console.ReadLine());

                    Trade t = new Trade(merchant.playerInventory, player.playerInventory);

                    t.playerTradeItems(index, amount);

                    player.playerInventory.veiwInventory();
                    Console.WriteLine();
                    Console.WriteLine("좋은 거래였소. 또 봅시다.");
                    Console.WriteLine();
                    merchant.playerInventory.veiwInventory();
                }
                else if (playerAnswer == 3)
                {
                    player.viewPlayerStat();
                }
            }
        }
        
        static void Main(string[] args)
        {
            Program p = new Program();
            p.GameLoop();
        }
    }
}

