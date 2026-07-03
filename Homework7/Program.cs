using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 자리와 숫자가 맞으면 스트라이크
// 숫자만 맞으면 볼
// 숫자는 4자리
// 주어진 숫자가 1234
// 1회차 7895 -> 0볼 0스트라이크
// 2회차 1532 -> 1볼 2스트라이크
// 3회차 1254 -> 0볼 3스트라이크
// 이런 식으로 9회차
// 랜덤 숫자. 중복 불가.

namespace Homework7
{
    class Program
    {
        // 플레이어가 입력한 4개의 숫자를 자리수 별로 배열에 넣음
        int[] intToArray(int number)
        {
            int[] result = new int[4];

            result[0] = number % 10000 / 1000;
            result[1] = number % 1000 / 100;
            result[2] = number % 100 / 10;
            result[3] = number % 10;

            return result;
        }

        // 플레이어가 입력한 숫자에 중복이 있는지 확인
        bool dupCheck(int[] playerNumber)
        {
            return
                playerNumber[0] != playerNumber[1] &&
                playerNumber[0] != playerNumber[2] &&
                playerNumber[0] != playerNumber[3] &&
                playerNumber[1] != playerNumber[2] &&
                playerNumber[1] != playerNumber[3] &&
                playerNumber[2] != playerNumber[3];
        }

        // 플레이어가 입력한 4개 숫자로 볼과 스트라이크를 계산
        int[] Call(int[] targetNumber, int[] playerNumber)
        {
            int[] call = new int[2]; // [볼, 스트라이크]
            
            
        }

        void GameLoop()
        {   
            // 1. 숫자야구에 사용할 4개 숫자를 정함
            Random random = new Random();

            int[] numberPool = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for(int i = 0; i < 300; i++)
            {
                int destIndex = random.Next() % 10;
                int sourIndex = random.Next() % 10;
                int tmp = numberPool[destIndex];
                numberPool[destIndex] = numberPool[sourIndex];
                numberPool[sourIndex] = tmp;
            }

            int[] targetNumber = new int[4] { numberPool[0], numberPool[1], numberPool[2], numberPool[3] };

            int life = 9;

            while (true)
            {
                Console.WriteLine("숫자야구 시작");

                // 2. 플레이어가 숫자를 입력
                Console.Write("서로 다른 4개의 숫자를 입력하세요: ");
                int playerNumber = int.Parse(Console.ReadLine());
                int[] playerNumberArray = intToArray(playerNumber);

                while (dupCheck(playerNumberArray))
                {
                    Console.Write("중복된 숫자를 사용했습니다. 서로 다른 4개의 숫자를 입력하세요: ");
                    playerNumber = int.Parse(Console.ReadLine());
                    playerNumberArray = intToArray(playerNumber);
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
