using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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

namespace Homework9
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
            int playerAnswer = 0;

            while (true)
            {
                // 조건 검사
                if (player.hitPoint <= 0) 
                {
                    Console.WriteLine("게임오버");
                    break; 
                }
                else if (player.level > 10)
                {
                    Console.WriteLine("게임 클리어");
                    break;
                }

                // 1. 전투와 상점 중 선택지가 주어짐
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("1. 전투 | 2. 상점 | 3. 상태창");
                playerAnswer = int.Parse(Console.ReadLine());

                // 2. 전투를 선택한 경우 몬스터와 전투
                if (playerAnswer == 1)
                {
                    Random random = new Random();
                    gm.createStage(); // GM이 새로운 스테이지 생성
                    Stage s = gm.getStage(); 

                    Console.WriteLine("몬스터가 나타났다!");
                    // 3. 전투는 내턴 상대턴으로 번갈아가며 진행
                    while (player.hitPoint > 0)
                    {
                        for (int i = 0; i < s.monsters.Length; i++)
                        {
                            if (s.monsters[i].creatureState == CreatureState.LIVING)
                            {
                                s.monsters[i].viewStat();
                            }
                        }

                        
                        Console.WriteLine($"[{player.name} 님의 턴]");
                        Console.Write("[1. 공격 | 2. 아이템 사용]    선택하세요: ");
                        playerAnswer = int.Parse(Console.ReadLine());

                        // 공격/아이템 사용 분기
                        if(playerAnswer == 1)
                        {
                            Console.WriteLine($"누굴 공격할까?");
                            for (int i = 0; i < s.monsters.Length; i++)
                            {
                                if (s.monsters[i].creatureState == CreatureState.LIVING)
                                {
                                    Console.WriteLine($"{i}){s.monsters[i].name}    ");
                                }
                            }
                            Console.Write("선택하세요: ");
                            int playerSelect = int.Parse(Console.ReadLine());

                            player.attack(s.monsters[playerSelect]);
                        }
                        else if(playerAnswer == 2)
                        {
                            player.inventory.veiwInventory();
                            Console.WriteLine();
                            Console.Write("어떤 아이템을 사용할까? 인덱스: ");
                            playerAnswer = int.Parse(Console.ReadLine());
                            Item[] items = player.inventory.pick(playerAnswer, 1);
                            if (items == null) Console.WriteLine("다음엔 잘 골라라");
                            items[0].use(player);
                        }

                        // 내 턴 종료 시 적이 전부 죽었다면 보상 받고 스테이지 클리어
                        if (s.isStageClear())
                        {
                            gm.endStage();

                            break;

                            if (player.level > 10) // 얻은 보상으로 10레벨 초과하면 게임 클리어
                            {
                                Console.WriteLine("엄청난 업적을 세웠다!");
                                Console.WriteLine("호화스러운 은퇴를 했다.");
                                break;
                            }
                        }

                        // 상대턴은 반복문으로 진행(여러 적이 있을 수 있음)
                        for (int i = 0; i < s.monsters.Length; i++)
                        {
                            if (s.monsters[i].creatureState == CreatureState.LIVING)
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
                else if (playerAnswer == 2)
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
                        Console.WriteLine("좋은 거래였소. 또 봅시다.");
                        player.inventory.veiwInventory();
                        Console.WriteLine();
                        merchant.inventory.veiwInventory();
                    }
                    else
                    {
                        Console.WriteLine("꺼져");
                    }
                }
                else if (playerAnswer == 3)
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

