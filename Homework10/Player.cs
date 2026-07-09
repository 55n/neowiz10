using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework10
{
    class Player
    {
        public PlayingCardDeck hand { get; private set; }
        protected int stake;
        protected string name;

        public Player(string name, int stake)
        {
            this.name = name;
            this.stake = stake;
            hand = new PlayingCardDeck();
            hand.createHand();
        }

        public int bet(int bettingCost)
        {
            if (bettingCost > stake)
            {
                Console.WriteLine($"소지금이 적어서 베팅을 못 함");
                return 0;
            }

            stake -= bettingCost;

            Console.WriteLine($"{name} 베팅 (소지금:${stake})");

            return bettingCost;
        }

        public void takeCardFromDeck(PlayingCardDeck deck)
        {
            hand.pushCardIntoDeck(deck.popCardFromDeck());
            Console.WriteLine($"{name} 님 카드 뽑음");
        }
    }
}
