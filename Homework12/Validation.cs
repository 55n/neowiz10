using Homework12.Homework10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework12
{
    class Validation
    {
        private CardDeck hand;

        public Validation(CardDeck hand)
        {
            this.hand = hand;
        }

        public bool OnePair() { return false; }

        public bool TwoPair() { return false; }

        public bool Triple() 
        { 
            
        }

        public bool Straight() 
        {
            if ((int)hand.deck[0].rank + 1 == (int)hand.deck[1].rank &&
                (int)hand.deck[1].rank + 1 == (int)hand.deck[2].rank &&
                (int)hand.deck[2].rank + 1 == (int)hand.deck[3].rank &&
                (int)hand.deck[3].rank + 1 == (int)hand.deck[4].rank)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BackStraight() 
        {
            if (hand.deck[0].rank == Rank.ACE &&
                hand.deck[0].rank == Rank.TWO &&
                hand.deck[0].rank == Rank.THREE &&
                hand.deck[0].rank == Rank.FOUR &&
                hand.deck[0].rank == Rank.FIVE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Mountain() 
        {
            if (hand.deck[0].rank == Rank.TEN &&
                hand.deck[0].rank == Rank.JACK &&
                hand.deck[0].rank == Rank.QUEEN &&
                hand.deck[0].rank == Rank.KING &&
                hand.deck[0].rank == Rank.ACE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Flush() {
            Suit suit = hand.deck[0].suit;

            // 1. 전부 같은 모양인지 확인
            for (int i = 0; i < hand.getCount(); i++)
            {
                if (hand.deck[i].suit != suit) return false; // 아니면 false
            }
            return true;
        }

        public bool FourCard() {
            Rank rank = hand.deck[0].rank;
            int count = 0;
            // 1. 4장이 같은 숫자인지 확인
            for (int i = 0; i < hand.getCount(); i++)
            {
                if(hand.deck[i].rank == rank)
                {
                    count++;
                }
            }

            if(count < 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool StraightFlush() {
            Suit suit = hand.deck[0].suit;

            // 1. 전부 같은 모양인지 확인
            for (int i = 0; i < hand.getCount(); i++)
            {
                if (hand.deck[i].suit != suit) return false; // 아니면 false
            }

            // 2. 전부 같은 모양이면 숫자 확인
            if ((int)hand.deck[0].rank + 1 == (int)hand.deck[1].rank &&
                (int)hand.deck[1].rank + 1 == (int)hand.deck[2].rank &&
                (int)hand.deck[2].rank + 1 == (int)hand.deck[3].rank &&
                (int)hand.deck[3].rank + 1 == (int)hand.deck[4].rank)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BackStraightFlush()
        {
            Suit suit = hand.deck[0].suit;

            // 1. 전부 같은 모양인지 확인
            for (int i = 0; i < hand.getCount(); i++)
            {
                if (hand.deck[i].suit != suit) return false; // 아니면 false
            }

            // 2. 전부 같은 모양이면 숫자 확인
            if (hand.deck[0].rank == Rank.ACE &&
                hand.deck[0].rank == Rank.TWO &&
                hand.deck[0].rank == Rank.THREE &&
                hand.deck[0].rank == Rank.FOUR &&
                hand.deck[0].rank == Rank.FIVE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RoyalStraightFlush() 
        {
            Suit suit = hand.deck[0].suit;

            // 1. 전부 같은 모양인지 확인
            for (int i = 0; i < hand.getCount(); i++)
            {
                if (hand.deck[i].suit != suit) return false; // 아니면 false
            }

            // 2. 전부 같은 모양이면 숫자 확인
            if(hand.deck[0].rank == Rank.TEN &&
                hand.deck[0].rank == Rank.JACK &&
                hand.deck[0].rank == Rank.QUEEN &&
                hand.deck[0].rank == Rank.KING &&
                hand.deck[0].rank == Rank.ACE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
