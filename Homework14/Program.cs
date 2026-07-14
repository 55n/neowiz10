using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*1.어제 만든 TextRPG
    1-1 player State pattern 적용시키기
    1-2 Monster State Pattern 적용시키기
    종료조건을 Die or 10Level로 하는데
    Die가 되면 게임 재실행 물어보도록 합시다.
*/

namespace Homework14
{
    class Program
    {
        void GameLoop()
        {
            Console.WriteLine("###############################################################");
            Console.WriteLine("###############################################################");
            Console.WriteLine("##################[    매이풀 용사의 모험   ]##################");
            Console.WriteLine("###############################################################");
            Console.WriteLine("###############################################################");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("??? : 이름 적어라");
            Console.Write("당신의 이름: ");
            string name = Console.ReadLine();

            // 플레이어 인벤토리 초기화 및 기본 물품 지급
            Inventory playerInventory = new Inventory(10);
            for (int i = 0; i < 20; i++) { playerInventory.load(new Gold()); }
            playerInventory.load(new HpPotion());
            playerInventory.load(new MpPotion());

            // 상인 인벤토리 초기화 및 기본 물품 지급
            Inventory merchantInventroy = new Inventory(10);
            for (int i = 0; i < 500; i++) { merchantInventroy.load(new Gold()); }
            for (int i = 0; i < 30; i++) { merchantInventroy.load(new HpPotion()); }
            for (int i = 0; i < 30; i++) { merchantInventroy.load(new MpPotion()); }

            Human player = new Human(name, playerInventory);
            Human merchant = new Human("상점주인", merchantInventroy);

            Console.WriteLine();
            Console.WriteLine($"???: {player.name} 사냥을 시작해라");
            Console.WriteLine();

            GameMaster gm = new GameMaster(player);
            gm.GameStateDictionary[GameState.PLAYING].Action();

            PlayerManager pm = PlayerManager.Instance;

            while (gm.GameState == GameState.PLAYING)
            {
                // 조건 검사
                if (player.hitPoint <= 0)
                {
                    Console.WriteLine("사망했습니다. 게임을 다시 시작할까요?");
                    Console.WriteLine("1. 다시 시작 | 2. 종료");
                    Console.Write("선택하세요: ");
                    pm.setPlayerChoice(int.Parse(Console.ReadLine()));
                    
                    if(pm.getPlayerChoice() == 1)
                    {
                        Console.Clear();
                        // ㅅㅂ ㅋㅋ
                        Console.WriteLine("###############################################################");
                        Console.WriteLine("###############################################################");
                        Console.WriteLine("##################[    매이풀 용사의 모험   ]##################");
                        Console.WriteLine("###############################################################");
                        Console.WriteLine("###############################################################");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("??? : 이름 적어라");
                        Console.Write("당신의 이름: ");
                        name = Console.ReadLine();

                        // 플레이어 인벤토리 초기화 및 기본 물품 지급
                        playerInventory = new Inventory(10);
                        for (int i = 0; i < 20; i++) { playerInventory.load(new Gold()); }
                        playerInventory.load(new HpPotion());
                        playerInventory.load(new MpPotion());

                        // 상인 인벤토리 초기화 및 기본 물품 지급
                        merchantInventroy = new Inventory(10);
                        for (int i = 0; i < 500; i++) { merchantInventroy.load(new Gold()); }
                        for (int i = 0; i < 30; i++) { merchantInventroy.load(new HpPotion()); }
                        for (int i = 0; i < 30; i++) { merchantInventroy.load(new MpPotion()); }

                        player = new Human(name, playerInventory);
                        merchant = new Human("상점주인", merchantInventroy);

                        Console.WriteLine();
                        Console.WriteLine($"???: {player.name} 사냥을 시작해라");
                        Console.WriteLine();

                        gm = new GameMaster(player);
                        gm.GameStateDictionary[GameState.PLAYING].Action();
                        gm.removeStage();

                        pm = PlayerManager.Instance;
                    }
                    else
                    {
                        gm.GameStateDictionary[GameState.GAME_OVER].Action();
                        continue;
                    }
                }
                else if (player.level > 10)
                {
                    gm.GameStateDictionary[GameState.GAME_CLEAR].Action();
                    continue;
                }

                // 1. 전투와 상점 중 선택지가 주어짐
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. 전투 | 2. 상점 | 3. 상태창");
                pm.setPlayerChoice(int.Parse(Console.ReadLine()));

                // 2. 전투를 선택한 경우 몬스터와 전투
                if (pm.getPlayerChoice() == 1)
                {
                    Random random = new Random();
                    gm.createStage(); // GM이 새로운 스테이지 생성
                    Stage s = gm.getStage();

                    Console.WriteLine("몬스터가 나타났다!");
                    // 3. 전투는 내턴 상대턴으로 번갈아가며 진행
                    while (player.hitPoint > 0)
                    {
                        for (int i = 0; i < s.monsters.Count; i++)
                        {
                            if (s.monsters[i].creatureState == CreatureState.ALIVE)
                            {
                                s.monsters[i].viewStat();
                            }
                        }

                        Console.WriteLine($"[{player.name} 님의 턴]");
                        Console.Write("[1. 공격 | 2. 아이템 사용]    선택하세요: ");
                        pm.setPlayerChoice(int.Parse(Console.ReadLine()));

                        // 공격/아이템 사용 분기
                        if (pm.getPlayerChoice() == 1)
                        {
                            Console.WriteLine($"누굴 공격할까?");
                            for (int i = 0; i < s.monsters.Count; i++)
                            {
                                if (s.monsters[i].creatureState == CreatureState.ALIVE)
                                {
                                    Console.WriteLine($"{i}){s.monsters[i].name}    ");
                                }
                            }
                            Console.Write("선택하세요: ");
                            int playerSelect = int.Parse(Console.ReadLine());

                            player.attack(s.monsters[playerSelect]);
                        }
                        else if (pm.getPlayerChoice() == 2)
                        {
                            player.inventory.veiwInventory();
                            Console.WriteLine();
                            Console.Write("어떤 아이템을 사용할까? 인덱스: ");
                            pm.setPlayerChoice(int.Parse(Console.ReadLine()));
                            Item[] items = player.inventory.pick(pm.getPlayerChoice(), 1);
                            if (items == null) Console.WriteLine("다음엔 잘 골라라");
                            items[0].use(player);
                        }

                        // 내 턴 종료 시 적이 전부 죽었다면 보상 받고 스테이지 클리어
                        if (s.isStageClear())
                        {
                            gm.endStage();

                            if (player.level > 10) // 얻은 보상으로 10레벨 초과하면 게임 클리어
                            {
                                Console.WriteLine("엄청난 업적을 세웠다!");
                                Console.WriteLine("호화스러운 은퇴를 했다.");
                                break;
                            }
                        }

                        // 상대턴은 반복문으로 진행(여러 적이 있을 수 있음)
                        for (int i = 0; i < s.monsters.Count; i++)
                        {
                            if (s.monsters[i].creatureState == CreatureState.ALIVE)
                            {
                                Console.WriteLine($"[{s.monsters[i].name} 님의 턴]");
                                s.monsters[i].attack(player);

                                // 상대턴에 공격을 맞고 죽으면 게임 루프 탈출
                                if (player.hitPoint <= 0)
                                {
                                    Console.WriteLine("장렬하게 전사했다...");
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (pm.getPlayerChoice() == 2)
                {
                    player.inventory.veiwInventory();
                    Console.WriteLine();
                    Console.WriteLine("상점에 어서오시오. 무슨 물건을 사려고 하시나?");
                    Console.WriteLine();
                    merchant.inventory.veiwInventory();

                    Console.Write("index 입력: ");
                    int index = int.Parse(Console.ReadLine());
                    Console.Write("수량 입력: ");
                    int amount = int.Parse(Console.ReadLine());

                    Trade t = new Trade(merchant.inventory, player.inventory);

                    bool deal = t.tradeItems(index, amount);

                    if (deal)
                    {
                        Console.WriteLine();
                        player.inventory.veiwInventory();
                        Console.WriteLine();
                        merchant.inventory.veiwInventory();
                        Console.WriteLine("좋은 거래였소. 또 봅시다.");
                    }
                    else
                    {
                        Console.WriteLine("꺼져");
                    }
                }
                else if (pm.getPlayerChoice() == 3)
                {
                    Console.WriteLine();
                    player.viewStat();
                    Console.WriteLine();
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
