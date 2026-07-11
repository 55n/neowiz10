using Homework12.Homework10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 포커판독기
// 7포커 기준
// 랜덤으로 카드를 받아서 그 카드의 족보 출력
// 족보기준 https://poker.hangame.com/gameguide/poker7/game_7poker2_2.html

namespace Homework12
{
    class Program
    {
        void GameLoop()
        {
            Console.WriteLine("포커판독기 가동");

            Random random = new Random();

            CardDeck deck = new CardDeck();
            deck.createCompleteDeck();
            deck.shuffle();

            CardDeck hand = new CardDeck();

            while (true)
            {
                Console.WriteLine("검사할 패의 표본을 생성합니다");
                Console.Write("표본이 몇 개 필요하신가요? (숫자 입력) : ");
                int count = int.Parse(Console.ReadLine());

                Console.WriteLine();
                Console.WriteLine($"{count} 번 검사합니다");

                for (int i = 0; i < count; i++)
                { 
                    // 1. 패 뽑기
                    for (int j = 0; j < 5; j++)
                    {
                        hand.push(deck.pop());
                    }

                    // 2. 패 검사
                    Validation validation = new Validation(hand);
                    if (validation.RoyalStraightFlush(hand))
                    {

                    }
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
