using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework6
{
    class Program
    {
        enum Operator // 연산자 카드
        {
            ADD,
            SUBTRACT,
            MULTIPLY,
            DIVIDE
        }
            
        static void Main(string[] args)
        {
            // 숫자연산게임
            // 첫번째 수 카드 0~9
            // 연산자 카드 * / - +
            // 두번째 수 카드 0~9
            // 소지금은 100000. 보유금액의 10% 이상만 베팅 가능
            // 1. 첫번째 카드를 컴퓨터와 플레이어에게 나눠주고 베팅
            // 2. 연산 카드를 나눠주고 베팅
            // 3. 두번째 카드를 나눠주고 베팅
            // 4. 패를 까고 결과를 연산
            // 연산 결과가 큰 수면 승리. 연산 결과가 7이면 무조건 승리.
            // 한명이 모든 돈을 잃으면 종료
            // 비기면 판을 종료하고 이번판 베팅금은 다음 판에 합친다!


            // 초기 세팅
            int playerMoney = 100000; // 플레이어 소지금
            int computerMoney = 100000; // 컴퓨터 소지금

            int[] playerHand = new int[2]; // 플레이어 손패
            int[] computerHand = new int[2]; // 컴퓨터 손패

            Operator playerOp;
            Operator computerOp;

            int playerBet = 0; // 플레이어 베팅 금액
            int computerBet = 0; // 컴퓨터  베팅 금액

            int[] numberDeck1 = new int[10]; // 숫자덱 1번
            int[] numberDeck2 = new int[10]; // 숫자덱 2번
            for (int i = 0; i < numberDeck1.Length; i++) numberDeck1[i] = i; // 0~9까지 첫번째 숫자덱에 넣기
            for (int i = 0; i < numberDeck2.Length; i++) numberDeck2[i] = i; // 0~9까지 두번째 숫자덱에 넣기

            Operator[] operatorDeck = new Operator[4]; // 연산자 카드 덱에 넣기
            operatorDeck[0] = Operator.ADD;
            operatorDeck[1] = Operator.SUBTRACT;
            operatorDeck[2] = Operator.MULTIPLY;
            operatorDeck[3] = Operator.DIVIDE;

            int round = 1; // 게임 라운드 카운터

            Console.WriteLine("숫자 연산 게임을 시작합니다~!");

            // 게임 루프
            while(true)
            {
                Random random = new Random();

                // 1. 게임 승패 분기 검사
                if (playerMoney <= 0 && computerMoney <= 0)
                {
                    Console.WriteLine("둘 다 소지금이 없습니다. 거지는 나가세요.");
                    break;
                }
                else if(playerMoney <=0 && computerMoney > 0)
                {
                    Console.WriteLine("이 게임은 컴퓨터 님의 승리입니다!");
                    break;
                }
                else if(playerMoney > 0 && computerMoney <= 0)
                {
                    Console.WriteLine("이 게임은 플레이어 님의 승리입니다!");
                    break;
                }

                Console.WriteLine($"{round} 번째 라운드를 시작합니다!");

                // 2. 첫번째 숫자덱 섞고 패 돌리기
                Console.WriteLine("첫 번째 숫자를 뽑을 시간입니다.");
                Console.WriteLine("패를 섞는 중...");
                for(int i = 0; i < 100; i++)
                {
                    int destIndex = random.Next() % numberDeck1.Length;
                    int sourIndex = random.Next() % numberDeck1.Length;
                    int tmpNum = numberDeck1[destIndex];
                    numberDeck1[destIndex] = numberDeck1[sourIndex];
                    numberDeck1[sourIndex] = tmpNum;
                }
                Console.WriteLine("첫번째 숫자패를 나눠드리겠습니다");

                playerHand[0] = numberDeck1[0];
                computerHand[0] = numberDeck1[1];

                // 3. 첫번째 베팅 페이즈 (10번의 기회)
                Console.WriteLine("이제 첫번째 베팅할 시간입니다");
                int count = 0;
                while (count < 10)
                {
                    int playerBetTmp = 0;
                    int computerBetTmp = 0;

                    if(playerBet > 0 && playerMoney <= 0)
                    {
                        Console.WriteLine("베팅에 금액을 모두 사용했군요! 넘어가겠습니다.");
                        break;
                    }

                    Console.Write($"얼마를 거시겠습니까? (소지금: ${playerMoney}) : ");
                    playerBetTmp = int.Parse(Console.ReadLine());

                    if (playerBetTmp > 0 && playerMoney / playerBetTmp <= 10) // 베팅 금액이 플레이어 소지금의 10% 이상인지 검사
                    {
                        if(playerBetTmp > playerMoney) // 플레이어 소지금이 충분한지 검사
                        {
                            playerBetTmp = playerMoney;
                            Console.WriteLine("플레이어 올 인~!");
                        }

                        if (computerMoney < playerBetTmp) // 컴퓨터 소지금이 충분한지 검사
                        {
                            Console.WriteLine("컴퓨터의 소지금이 부족합니다. 맞추겠습니다.");
                            playerBetTmp = computerMoney;
                            Console.WriteLine("컴퓨터 올 인~!");
                        }

                        computerBetTmp = playerBetTmp;

                        playerMoney -= playerBetTmp;
                        computerMoney -= computerBetTmp;

                        playerBet = playerBet + playerBetTmp;
                        computerBet = computerBet + computerBetTmp;

                        break;
                    }
                    else
                    {
                        Console.WriteLine($"너무 적은 금액입니다. (남은 기회: {9 - count})");
                        count++;
                    }
                }
                if(count >= 10)
                {
                    Console.WriteLine("우리 가게에 겁쟁이는 필요 없어요! 꺼지세욧");
                    break;
                }
                Console.WriteLine($"첫번째 베팅 완료) 플레이어 베팅: ${playerBet} | 컴퓨터 베팅: ${computerBet}");


                // 4. 연산 카드 섞고 패 돌리기
                for (int i = 0; i < 100; i++)
                {
                    int destIndex = random.Next() % 4;
                    int sourIndex = random.Next() % 4;
                    Operator tmpOp = operatorDeck[destIndex];
                    operatorDeck[destIndex] = operatorDeck[sourIndex];
                    operatorDeck[sourIndex] = tmpOp;
                }
                playerOp = operatorDeck[0];
                computerOp = operatorDeck[1];

                // 5. 두번째 베팅 페이즈
                Console.WriteLine("이제 두번째 베팅할 시간입니다");
                while (count < 10)
                {
                    int playerBetTmp = 0;
                    int computerBetTmp = 0;

                    if (playerBet > 0 && playerMoney <= 0)
                    {
                        Console.WriteLine("베팅에 금액을 모두 사용했군요! 넘어가겠습니다.");
                        break;
                    }

                    Console.Write($"얼마를 거시겠습니까? (소지금: ${playerMoney}) : ");
                    playerBetTmp = int.Parse(Console.ReadLine());

                    if (playerBetTmp > 0 && playerMoney / playerBetTmp <= 10) // 베팅 금액이 플레이어 소지금의 10% 이상인지 검사
                    {
                        if (playerBetTmp > playerMoney) // 플레이어 소지금이 충분한지 검사
                        {
                            playerBetTmp = playerMoney;
                            Console.WriteLine("플레이어 올 인~!");
                        }

                        if (computerMoney < playerBetTmp) // 컴퓨터 소지금이 충분한지 검사
                        {
                            Console.WriteLine("컴퓨터의 소지금이 부족합니다. 맞추겠습니다.");
                            playerBetTmp = computerMoney;
                            Console.WriteLine("컴퓨터 올 인~!");
                        }

                        computerBetTmp = playerBetTmp;

                        playerMoney -= playerBetTmp;
                        computerMoney -= computerBetTmp;

                        playerBet = playerBet + playerBetTmp;
                        computerBet = computerBet + computerBetTmp;

                        break;
                    }
                    else
                    {
                        Console.WriteLine($"너무 적은 금액입니다. (남은 기회: {9 - count})");
                        count++;
                    }
                }
                if (count >= 10)
                {
                    Console.WriteLine("우리 가게에 겁쟁이는 필요 없어요! 꺼지세욧");
                    break;
                }
                Console.WriteLine($"두번째 베팅 완료) 플레이어 베팅: ${playerBet} | 컴퓨터 베팅: ${computerBet}");


                // 6. 두번째 숫자덱 섞고 패 돌리기
                Console.WriteLine("두 번째 숫자를 뽑을 시간입니다.");
                Console.WriteLine("패를 섞는 중...");
                for (int i = 0; i < 100; i++)
                {
                    int destIndex = random.Next() % numberDeck2.Length;
                    int sourIndex = random.Next() % numberDeck2.Length;
                    int tmpNum = numberDeck2[destIndex];
                    numberDeck2[destIndex] = numberDeck2[sourIndex];
                    numberDeck2[sourIndex] = tmpNum;
                }
                Console.WriteLine("두번째 숫자패를 나눠드리겠습니다");

                playerHand[1] = numberDeck2[0];
                computerHand[1] = numberDeck2[1];

                // 7. 세번째 베팅 페이즈
                Console.WriteLine("이제 세번째 베팅할 시간입니다");
                while (count < 10)
                {
                    int playerBetTmp = 0;
                    int computerBetTmp = 0;

                    if (playerBet > 0 && playerMoney <= 0)
                    {
                        Console.WriteLine("베팅에 금액을 모두 사용했군요! 넘어가겠습니다.");
                        break;
                    }

                    Console.Write($"얼마를 거시겠습니까? (소지금: ${playerMoney}) : ");
                    playerBetTmp = int.Parse(Console.ReadLine());

                    if (playerBetTmp > 0 && playerMoney / playerBetTmp <= 10) // 베팅 금액이 플레이어 소지금의 10% 이상인지 검사
                    {
                        if (playerBetTmp > playerMoney) // 플레이어 소지금이 충분한지 검사
                        {
                            playerBetTmp = playerMoney;
                            Console.WriteLine("플레이어 올 인~!");
                        }

                        if (computerMoney < playerBetTmp) // 컴퓨터 소지금이 충분한지 검사
                        {
                            Console.WriteLine("컴퓨터의 소지금이 부족합니다. 맞추겠습니다.");
                            playerBetTmp = computerMoney;
                            Console.WriteLine("컴퓨터 올 인~!");
                        }

                        computerBetTmp = playerBetTmp;

                        playerMoney -= playerBetTmp;
                        computerMoney -= computerBetTmp;

                        playerBet = playerBet + playerBetTmp;
                        computerBet = computerBet + computerBetTmp;

                        break;
                    }
                    else
                    {
                        Console.WriteLine($"너무 적은 금액입니다. (남은 기회: {9 - count})");
                        count++;
                    }
                }
                if (count >= 10)
                {
                    Console.WriteLine("우리 가게에 겁쟁이는 필요 없어요! 꺼지세욧");
                    break;
                }
                Console.WriteLine($"세번째 베팅 완료) 플레이어 베팅: ${playerBet} | 컴퓨터 베팅: ${computerBet}");


                // 8. 연산 후 크기 비교 페이즈
                Console.WriteLine("이제 패와 연산자 카드를 공개해주십시오.");

                float playerResult = 0f;

                switch (playerOp)
                {
                    case Operator.ADD:
                        playerResult = playerHand[0] + playerHand[1];
                        Console.WriteLine($"플레이어) {playerHand[0]} + {playerHand[1]}");
                        break;
                    case Operator.SUBTRACT:
                        playerResult = playerHand[0] - playerHand[1];
                        Console.WriteLine($"플레이어) {playerHand[0]} - {playerHand[1]}");
                        break;
                    case Operator.MULTIPLY:
                        playerResult = playerHand[0] * playerHand[1];
                        Console.WriteLine($"플레이어) {playerHand[0]} * {playerHand[1]}");
                        break;
                    case Operator.DIVIDE:
                        if (playerHand[1] != 0) playerResult = playerHand[0] / playerHand[1];
                        else playerResult = 0;
                        Console.WriteLine($"플레이어) {playerHand[0]} / {playerHand[1]}");
                        break;
                }

                float computerResult = 0f;

                switch (computerOp)
                {
                    case Operator.ADD:
                        computerResult = computerHand[0] + computerHand[1];
                        Console.WriteLine($"컴퓨터) {computerHand[0]} + {computerHand[1]}");
                        break;
                    case Operator.SUBTRACT:
                        computerResult = computerHand[0] - computerHand[1];
                        Console.WriteLine($"컴퓨터) {computerHand[0]} - {computerHand[1]}");
                        break;
                    case Operator.MULTIPLY:
                        computerResult = computerHand[0] * computerHand[1];
                        Console.WriteLine($"컴퓨터) {computerHand[0]} * {computerHand[1]}");
                        break;
                    case Operator.DIVIDE:
                        if (computerHand[1] != 0) computerResult = computerHand[0] / computerHand[1];
                        else computerResult = 0;
                        Console.WriteLine($"컴퓨터) {computerHand[0]} / {computerHand[1]}");
                        break;
                }

                Console.WriteLine($"결과는~ 플레이어 님은 {playerResult}. 컴퓨터 님은 {computerResult} 입니다.");

                if(playerResult == 7)
                {
                    Console.WriteLine("이 판은 플레이어 님의 승리!");
                    playerMoney = playerMoney + playerBet + computerBet;
                    playerBet = 0;
                    computerBet = 0;
                }
                else if(computerResult == 7)
                {
                    Console.WriteLine("이 판은 컴퓨터 님의 승리!");
                    computerMoney = computerMoney + playerBet + computerBet;
                    playerBet = 0;
                    computerBet = 0;
                }
                else if(computerResult < playerResult)
                {
                    Console.WriteLine("이 판은 플레이어 님의 승리!");
                    playerMoney = playerMoney + playerBet + computerBet;
                    playerBet = 0;
                    computerBet = 0;
                }
                else if(computerResult > playerResult)
                {
                    Console.WriteLine("이 판은 컴퓨터 님의 승리!");
                    computerMoney = computerMoney + playerBet + computerBet;
                    playerBet = 0;
                    computerBet = 0;
                }
                else if(computerResult == playerResult)
                {
                    Console.WriteLine("이 판은 비겼습니다! 베팅 금액은 그대로 남겨놓겠습니다~");
                }

                Console.WriteLine($"플레이어 님의 소지금: {playerMoney} | 컴퓨터 님의 소지금: {computerMoney}");

                round++;
            }

            Console.WriteLine("게임이 종료되었습니다.");
        }
    }
}
