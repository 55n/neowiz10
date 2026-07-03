using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/* 변수의 종류
* 1. 글로벌, 정적, 전역 변수 - 프로그램 내 모든 객체가 R/W 가능한 변수
* 2. 멤버 변수
* 3. 지역 변수 - 메서드 안에서 할당해서 사용
* 4. 매개 변수
*/

namespace Class5
{
    class Program
    {
        static int staticInt = 5; // 글로벌 컨텍스트에서 관리 중
        int memberInt = 7; // Program 컨텍스트에서 관리 중

        static void Sub(string[] args) // 글로벌 컨텍스트에서 관리 중
        {
            Console.WriteLine(staticInt);

            staticInt = 8;

            Console.WriteLine(staticInt);

            Program p = new Program();

            Console.WriteLine(p.memberInt);

            p.memberInt = 5;

            Console.WriteLine(p.memberInt);

            Console.WriteLine(Sum(1, 2));

            PrintName("개");

            Console.WriteLine(Mul(5, 9));
            Console.WriteLine(Mul(7, 7));
            Console.WriteLine(Mul(1, 2));
        }

        // 두 수의 합을 구하는 메서드
        static int Sum(int a, int b)
        {
            int sum = 0;
            sum = a + b;
            return sum;
        }

        static void PrintName(string name)
        {
            if (name == "개")
            {
                Console.WriteLine("사람이름 아님!");
                return;
            }
            Console.WriteLine($"내이름은 {name}");
        }

        // 두 수의 곱을 구하는 메서드
        // 두 수가 같다면 0을 반환
        static int Mul(int a, int b)
        {
            if(a == b)
            {
                return 0;
            }
            else
            {
                int result = a * b;
                return result;
            }
        }
    }

    enum Operator
    {
        ADD, SUBTRACT, MULTIPLY, DIVIDE
    }

    class NumberGame
    {
        Random random = new Random();

        int[] deck1 = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] deck2 = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int playerMoney = 100000;
        int computerMoney = 100000;

        int playerCard1;
        int playerCard2;
        int playerOp;

        int computerCard1;
        int computerCard2;
        int computerOp;

        int totalBettingMoney;

        int[] ShuffleDeck(int[] deck, int shuffleCount)
        {
            for(int i = 0; i< shuffleCount; i++)
            {
                int destIndex = random.Next() % deck.Length;
                int sourIndex = random.Next() % deck.Length;
                int tmp = deck[destIndex];
                deck[destIndex] = deck[sourIndex];
                deck[sourIndex] = tmp;
            }
            return deck;
        }

        bool Betting_Check(int bettingMoney, int currentMoney)
        {
            if (bettingMoney > 0 && currentMoney / bettingMoney <= 10) // 베팅 금액이 플레이어 소지금의 10% 이상인지 검사
            {
                if (bettingMoney > currentMoney) // 플레이어 소지금이 충분한지 검사
                {
                    Console.WriteLine("돈이 없어요 ㅠㅠ");
                    return false;
                }

                Console.WriteLine($"플레이어의 판돈: {bettingMoney}");
                return true;
            }
            else
            {
                Console.WriteLine($"너무 적은 금액입니다.");
                return false;
            }
        }

        float Calculation(int card1, Operator op, int card2)
        {
            switch (op)
            {
                case Operator.ADD:
                    return card1 + card2;
                case Operator.SUBTRACT:
                    return card1 - card2;
                case Operator.MULTIPLY:
                    return card1 * card2;
                case Operator.DIVIDE:
                    return card1 / card2;
                default:
                    return -10;
            }
        }

        string OperToString(Operator op)
        {
            switch (op)
            {
                case Operator.ADD:
                    return "+";
                case Operator.SUBTRACT:
                    return "-";
                case Operator.MULTIPLY:
                    return "*";
                case Operator.DIVIDE:
                    return "÷";
                default:
                    return "";
            }
        }

        void StartGame()
        {
            while(playerMoney > 0 && computerMoney > 0)
            {
                int[] shuffledDeck1 = ShuffleDeck(deck1, 100);
                playerCard1 = shuffledDeck1[0]; 
                computerCard1 = shuffledDeck1[1];

                int bettingMoney = int.Parse(Console.ReadLine());
                while(!Betting_Check(bettingMoney, playerMoney))
                {
                    bettingMoney = int.Parse(Console.ReadLine());
                }

                playerOp = random.Next() % 4;
                computerOp = random.Next() % 4;
                while(playerOp == computerOp)
                {
                    computerOp = random.Next() % 4;
                }

                bettingMoney = int.Parse(Console.ReadLine());
                while (!Betting_Check(bettingMoney, playerMoney))
                {
                    bettingMoney = int.Parse(Console.ReadLine());
                }

                int[] shuffledDeck2 = ShuffleDeck(deck2, 100);
                playerCard2 = shuffledDeck2[0];
                computerCard2 = shuffledDeck2[1];

                bettingMoney = int.Parse(Console.ReadLine());
                while (!Betting_Check(bettingMoney, playerMoney))
                {
                    bettingMoney = int.Parse(Console.ReadLine());
                }

                
            }
        }


        static void Main(string[] args)
        {
            NumberGame numberGame = new NumberGame();
            numberGame.StartGame();
        }
    }
}
