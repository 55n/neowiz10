using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 자리와 숫자가 맞으면 스트라이크
// 숫자만 맞으면 볼
// 숫자는 4자리
// ex) 주어진 숫자가 1234
// 1회차 7895 -> 0볼 0스트라이크
// 2회차 1532 -> 1볼 2스트라이크
// 3회차 1254 -> 0볼 3스트라이크
// 이런 식으로 9회차
// 랜덤 숫자. 중복 불가.

namespace Homework7
{
    class Program
    {
        int[] setTargetNumber() // 플레이어가 맞춰야 하는 4개의 숫자를 생성하는 기능
        {
            Random random = new Random();

            int[] numberPool = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 0; i < 512; i++)
            {
                int destIndex = random.Next() % 10;
                int sourIndex = random.Next() % 10;
                int tmp = numberPool[destIndex];
                numberPool[destIndex] = numberPool[sourIndex];
                numberPool[sourIndex] = tmp;
            }

            int[] targetNumber = new int[4] { numberPool[0], numberPool[1], numberPool[2], numberPool[3] };

            return targetNumber;
        }

        int[] intToArray(int number) // 플레이어가 입력한 4개의 숫자를 자리수 별로 배열에 넣는 기능
        {
            int[] result = new int[4];

            result[0] = number % 10000 / 1000;
            result[1] = number % 1000 / 100;
            result[2] = number % 100 / 10;
            result[3] = number % 10;

            return result;
        }

        
        bool isNumberDup(int[] playerNumber) // 플레이어가 입력한 숫자에 중복이 있는지 확인하는 기능. 있으면 true
        {
            return
                playerNumber[0] == playerNumber[1] ||
                playerNumber[0] == playerNumber[2] ||
                playerNumber[0] == playerNumber[3] ||
                playerNumber[1] == playerNumber[2] ||
                playerNumber[1] == playerNumber[3] ||
                playerNumber[2] == playerNumber[3];
        }

        
        int[] call(int[] targetNumber, int[] playerNumber) // 플레이어가 입력한 4개 숫자와 생성된 4개 숫자를 대조하여 볼과 스트라이크를 계산하는 기능
        {
            int[] call = new int[2];
            int ball = 0;
            int strike = 0;
            
            for(int i = 0; i < targetNumber.Length; i++)
            {
                if (targetNumber[i] == playerNumber[i]) 
                { 
                    strike++; 
                }
                else
                {
                    for (int j = 0; j < playerNumber.Length; j++)
                    {
                        if (targetNumber[i] == playerNumber[j]) ball++;
                    }
                }
            }

            call[0] = ball; 
            call[1] = strike; 

            return call; // [볼, 스트라이크]
        }

        void GameLoop()
        {
            Console.WriteLine("숫자야구 시~~작!");
            
            int[] targetNumber = setTargetNumber(); // 플레이어가 맞춰야 하는 4개 숫자 생성

            int life = 9; // 플레이어 생명 인디케이터

            while (true)
            {
                // 1. 플레이어가 숫자를 입력
                Console.Write("서로 다른 4개의 숫자를 입력하세요: "); // 4개의 정상적인 숫자를 입력할거라고 플레이어에게 맡긴다. 이상한거 입력하는건 알아서 하슈 ㅋㅋ
                int playerInput = int.Parse(Console.ReadLine());
                int[] playerNumber = intToArray(playerInput);

                while (isNumberDup(playerNumber))
                {
                    Console.Write("중복된 숫자를 사용했습니다. 서로 다른 4개의 숫자를 입력하세요: ");
                    playerInput = int.Parse(Console.ReadLine());
                    playerNumber = intToArray(playerInput);
                }

                // 2. targetNumber 와 playerNumber 의 숫자를 대조하여 볼과 스트라이크 계산
                int[] result = call(targetNumber, playerNumber);

                Console.WriteLine($"볼: {result[0]} | 스트라이크: {result[1]}");

                // 3. 승리 조건 검사
                if (result[1] == 4)
                {
                    Console.WriteLine("전부 맞히셨군요 축하합니다! 플레이어님의 승리입니다!");
                    break;
                }

                // 4. 기회 차감
                life--;

                if (life > 0)
                {
                    Console.WriteLine($"아닙니다! (남은 기회: {life})");
                }
                else
                {
                    Console.WriteLine("나가세요.");
                    break;
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
