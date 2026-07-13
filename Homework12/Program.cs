using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
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

            CardDeck hand = new CardDeck();

            Console.WriteLine("검사할 무작위 패의 표본을 생성합니다");
            Console.Write("표본이 몇 개 필요하신가요? (숫자 입력) : ");

            int count = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine($"{count} 번 검사합니다");
            Console.WriteLine();

            for (int i = 0; i < count; i++)
            {
                // 1. 덱 생성
                deck.createCompleteDeck();
                deck.shuffle(random, 1000);

                // 2. 패 뽑기
                for (int j = 0; j < 5; j++)
                {
                    hand.push(deck.pop());
                }

                // 3. 패 검사

                // 디버깅용
                //hand.deck[0] = new Card(Suit.SPADE, Rank.TWO);
                //hand.deck[1] = new Card(Suit.SPADE, Rank.THREE);
                //hand.deck[2] = new Card(Suit.SPADE, Rank.FOUR);
                //hand.deck[3] = new Card(Suit.SPADE, Rank.FIVE);
                //hand.deck[4] = new Card(Suit.SPADE, Rank.SIX);

                Validation validation = new Validation(hand);
                HandRank handRank = validation.validate();

                // 디버깅용
                /*if (handRank == HandRank.NO_PAIR ||
                    handRank == HandRank.ONE_PAIR ||
                    handRank == HandRank.TWO_PAIR ||
                    handRank == HandRank.TRIPLE ||
                    handRank == HandRank.FLUSH ||
                    handRank == HandRank.FULL_HOUSE ||
                    handRank == HandRank.FOUR_CARD ||
                    handRank == HandRank.STRAIGHT ||
                    handRank == HandRank.BACK_STRAIGHT ||
                    handRank == HandRank.MOUNTAIN)
                {
                    hand.deck.Clear();
                    deck.deck.Clear();
                    continue;
                }*/

                Console.WriteLine($"{i}번째:[{handRank.ToString()}]");
                hand.showAllCards();
                Console.WriteLine();
                Console.WriteLine();

                // 4. 패 비우기
                hand.deck.Clear();

                // 5. 덱 비우기
                deck.deck.Clear();
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Program p = new Program();
            p.GameLoop();
        }
    }
}
