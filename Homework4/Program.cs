using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 2. 가위바위보 게임을 만들기
            ㄴ 유저는 입력을 받습니다. ex ) 가위 바위 보 
            ㄴ 컴퓨터도 가위 바위 보 중 랜덤한 것을 냅니다. 
            ㄴ 가위바위보 룰을 따라갑니다. 
            ㄴ 베팅시스템이 있습니다. 
            ㄴ 유저도 컴퓨터도 베팅을 하지만 유저의 돈과 같은 금액을
                베팅하도록 합시다.
            ㄴ 만약 유저가 이겼다면
                 베팅금액의 2배를 받게 된다.
                 컴퓨터는 베팅한 금액을 잃게 됩니다. 
            ㄴ 만약 컴퓨터가 이겼다면
                 베팅금액을 잃게 됩니다. 
                컴퓨터가 가지고 있는 소지금은 늘어납니다. 
                베팅금액의 2배
            ㄴ 비겼을 경우는 둘다 베팅한 금액을 잃습니다. 
            ㄴ 이게임의 종료조건은 둘중 하나가 
                 소지금이 탈탈 털린경우 게임이 종료됩니다.
            */

            // 상수 선언
            int ROCK = 1;
            int SCISSORS = 2;
            int PAPER = 3;

            // 컴퓨터 변수 선언
            int aiHand; // 컴퓨터 손
            int aiMoney = 1000; // 컴퓨터 소지금
            int aiBet; // 컴퓨터 베팅 금액

            // 플레이어 변수 선언
            int playerHand; // 플레이어 손
            int playerMoney = 1000; // 플레이어 소지금
            int playerBet; // 플레이어 베팅 금액

            // 게임 루프
            while(true)
            {
                Console.WriteLine($"현재 상황)) 플레이어 소지금: ${playerMoney} | 컴퓨터 소지금: ${aiMoney}");

                // 1. 게임 승패 분기 판단
                if(aiMoney <= 0 && playerMoney <= 0)
                {
                    Console.WriteLine("둘 다 빈털털이 입니다. 거지는 나가십시오.");
                    break;
                }
                else if(playerMoney <= 0)
                {
                    Console.WriteLine("컴퓨터 님이 승리했습니다!");
                    break;
                }
                else if(aiMoney <= 0)
                {
                    Console.WriteLine("플레이어 님이 승리했습니다!");
                    break;
                }

                // 2. 베팅 페이즈
                Console.Write($"베팅할 시간입니다. 얼마를 거시겠어요? (소지금: ${playerMoney}) : ");
                playerBet = int.Parse(Console.ReadLine()); // 플레이어 베팅

                if (playerBet >= playerMoney) // 가진 돈보다 많이 건 경우 전부 걸기
                {
                    playerBet = playerMoney;
                    Console.WriteLine("플레이어 올 인!");
                }

                if(playerBet >= aiMoney) // 컴퓨터가 가진 돈보다 건 돈이 더 큰 경우 컴퓨터의 돈만큼 걸기
                {
                    playerBet = aiMoney;
                    Console.WriteLine("컴퓨터가 돈이 부족하군요. 베팅 금액을 맞추겠습니다.");
                }

                aiBet = playerBet; // 플레이어와 같은 베팅

                if (aiBet >= aiMoney) // 가진 돈보다 많이 건 경우 전부 걸기
                {
                    aiBet = aiMoney;
                    Console.WriteLine("컴퓨터 올 인!");
                }

                playerMoney -= playerBet; // 선 차감
                aiMoney -= aiBet; // 선 차감

                Console.WriteLine($"베팅 금액) 플레이어: ${playerBet} | 컴퓨터: ${aiBet}");


                // 3. 가위바위보 내기 페이즈
                Console.Write("그럼 준비하시고~! 가위 바위 보! : ");
                
                aiHand = (new Random().Next() + 1) % 4; // 컴퓨터 결정

                string playerAnswer = Console.ReadLine(); // 플레이어 문자열 입력

                switch(playerAnswer) // 플레이어 입력을 가위바위보 상수로 변환
                {
                    case "가위":
                        playerHand = SCISSORS;
                        break;
                    
                    case "바위":
                        playerHand = ROCK;
                        break;
                    
                    case "보":
                        playerHand = PAPER;
                        break;
                    default:
                        playerHand = 0;
                        break;
                }

                // 4. 승패 결정 페이즈
                if(playerHand == SCISSORS)
                {
                    if(aiHand == PAPER)
                    {
                        Console.WriteLine($"플레이어: 가위 | 컴퓨터: 보 : 플레이어 승!");
                        playerMoney = playerMoney + playerBet + aiBet;
                    }
                    else if(aiHand == ROCK)
                    {
                        Console.WriteLine($"플레이어: 가위 | 컴퓨터: 바위 : 컴퓨터 승!");
                        aiMoney = aiMoney + playerBet + aiBet;
                    }
                    else
                    {
                        Console.WriteLine($"플레이어: 가위 | 컴퓨터: 가위 : 비겼습니다");
                        playerBet = 0;
                        aiBet = 0;
                    }
                }
                else if(playerHand == ROCK)
                {
                    if (aiHand == SCISSORS)
                    {
                        Console.WriteLine($"플레이어: 바위 | 컴퓨터: 가위 : 플레이어 승!");
                        playerMoney = playerMoney + playerBet + aiBet;
                    }
                    else if (aiHand == PAPER)
                    {
                        Console.WriteLine($"플레이어: 바위 | 컴퓨터: 보 : 컴퓨터 승!");
                        aiMoney = aiMoney + playerBet + aiBet;
                    }
                    else
                    {
                        Console.WriteLine($"플레이어: 바위 | 컴퓨터: 바위 : 비겼습니다");
                        playerBet = 0;
                        aiBet = 0;
                    }
                }
                else if(playerHand == PAPER)
                {
                    if (aiHand == ROCK)
                    {
                        Console.WriteLine($"플레이어: 보 | 컴퓨터: 바위 : 플레이어 승!");
                        playerMoney = playerMoney + playerBet + aiBet;
                    }
                    else if (aiHand == SCISSORS)
                    {
                        Console.WriteLine($"플레이어: 보 | 컴퓨터: 가위 : 컴퓨터 승!");
                        aiMoney = aiMoney + playerBet + aiBet;
                    }
                    else
                    {
                        Console.WriteLine($"플레이어: 보 | 컴퓨터: 보 : 비겼습니다");
                        playerBet = 0;
                        aiBet = 0;
                    }
                }
                else
                {
                    Console.WriteLine("플레이어 님 정신 차리세요! 컴퓨터 승!");
                    aiMoney = aiMoney + playerBet + aiBet;
                }
            }
        }
    }
}
