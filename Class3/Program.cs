using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class3
{
    class Program
    {
        static void Main(string[] args)
        {

            // switch-case
            // break 쓰면 빠져나올 때 좋음
            int num = 10;

            switch(num)
            {
                case 0:
                    Console.WriteLine("첫번째 케이스");
                    break;
                case 10:
                    Console.WriteLine("두번째 케이스");
                    break;
                case 100:
                    Console.WriteLine("세번째 케이스");
                    break;
            }

            Console.WriteLine("어디로 이동할 지 선택하세요");
            Console.WriteLine("|1. 로비|2. 상점|3. 전쟁터|4. 투기장|");

            Console.WriteLine("입력하세요: ");
            int locationInput = int.Parse(Console.ReadLine());

            switch (locationInput)
            {
                case 1:
                    Console.WriteLine("로비로 이동");
                    break;
                case 2:
                    Console.WriteLine("상점으로 이동");
                    break;
                case 3:
                    Console.WriteLine("전쟁터로 이동");
                    break;
                case 4:
                    Console.WriteLine("투기장으로 이동");
                    break;
                default:
                    Console.WriteLine("잘못 입력하셨습니다");
                    break;
            }

            // 난수 생성
            // 확률 기반 시스템 혹은 특정 알고리즘에 활용
            // 클래스
            // new를 활용해서 객체를 생성할 수 있다
            Random random = new Random();
            int result = random.Next();
            int answer = result % 1000;


            // for - 인덱스 기반 순회 탐색
            /* 초기화식: 반복 실행 전에 가장 먼저 딱 한번 실행됨
             * 조건식: 반복조건. false일 시 반복 중단
             * 반복식: 실행부 종료 시 실행
             */
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
            }

            // break 실험
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
                if (i >= 5)
                {
                    break;
                }
            }

            // 반복식 실험
            for (int i = 0; i < 10; i += 2)
            {
                Console.WriteLine(i);
            }

            // 반복문 종합 실험
            int total = 0;
            for (int i = 1; i <= 100; i++)
            {
                total += i;
            }
            Console.WriteLine(total);

            // continue 명령문 실험
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 1)
                {
                    continue;
                }
                Console.WriteLine(i);
            }

            // 이중 for 문
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    Console.WriteLine($"i:{i} k:{k}");
                }
            }

            // 구구단 실습
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    Console.WriteLine($"{i} x {j} = {i * j}");
                }
            }

            // while
            int s = 0;
            while (s < 10)
            {
                Console.WriteLine(s);
                s++;
            }

            int a = 0;
            int sum = 0;
            while (a <= 100)
            {
                sum += a;
                a++;
            }
            Console.WriteLine(sum);

            int playerMoney = 100000;

            Console.WriteLine("구세군이다~");
            while(playerMoney > 0)
            {
                Console.Write("기부 안하면 죽는다! : ");
                int inputMoney = int.Parse(Console.ReadLine());

                if(playerMoney - inputMoney < 0)
                {
                    playerMoney -= playerMoney;
                    Console.WriteLine("ㅂㅂ ㅋㅋㅋ");
                    break;
                }
                else
                {
                    playerMoney -= inputMoney;

                }
            }
        }
    }
}
