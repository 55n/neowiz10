using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework12
{
    enum HandRank
    {
        ROYAL_STRAIGHT_FLUSH,
        BACK_STRAIGHT_FLUSH,
        STRAIGHT_FLUSH,
        FOUR_CARD,
        FULL_HOUSE,
        FLUSH,
        MOUNTAIN,
        BACK_STRAIGHT,
        STRAIGHT,
        TRIPLE,
        TWO_PAIR,
        ONE_PAIR,
        NO_PAIR
    }

    class Validation
    {
        private Dictionary<Suit, int> suitCount;
        private Dictionary<Rank, int> rankCount;
        private CardDeck hand;

        public Validation(CardDeck hand)
        {
            this.hand = hand;
            this.suitCount = new Dictionary<Suit, int>();
            this.rankCount = new Dictionary<Rank, int>();

            suitCount[Suit.SPADE] = 0;
            suitCount[Suit.CLOVER] = 0;
            suitCount[Suit.DIAMOND] = 0;
            suitCount[Suit.HEART] = 0;

            rankCount[Rank.ACE] = 0;
            rankCount[Rank.TWO] = 0;
            rankCount[Rank.THREE] = 0;
            rankCount[Rank.FOUR] = 0;
            rankCount[Rank.FIVE] = 0;
            rankCount[Rank.SIX] = 0;
            rankCount[Rank.SEVEN] = 0;
            rankCount[Rank.EIGHT] = 0;
            rankCount[Rank.NINE] = 0;
            rankCount[Rank.TEN] = 0;
            rankCount[Rank.JACK] = 0;
            rankCount[Rank.QUEEN] = 0;
            rankCount[Rank.KING] = 0;
        }

        public HandRank validate()
        {
            cardCount();

            if (RoyalStraightFlush()) return HandRank.ROYAL_STRAIGHT_FLUSH;
            else if (BackStraightFlush()) return HandRank.BACK_STRAIGHT_FLUSH;
            else if (StraightFlush()) return HandRank.STRAIGHT_FLUSH;
            else if (FourCard()) return HandRank.FOUR_CARD;
            else if (FullHouse()) return HandRank.FULL_HOUSE;
            else if (Flush()) return HandRank.FLUSH;
            else if (Mountain()) return HandRank.MOUNTAIN;
            else if (BackStraight()) return HandRank.BACK_STRAIGHT;
            else if (Straight()) return HandRank.STRAIGHT;
            else if (Triple()) return HandRank.TRIPLE;
            else if (TwoPair()) return HandRank.TWO_PAIR;
            else if (OnePair()) return HandRank.ONE_PAIR;
            else return HandRank.NO_PAIR;
        }

        private void cardCount()
        {
            for (int i = 0; i < hand.deck.Count; i++)
            {
                suitCount[hand.deck[i].suit] += 1;
                rankCount[hand.deck[i].rank] += 1;
            }
        }

        private bool OnePair()
        {
            foreach (var item in rankCount)
            {
                if (item.Value == 2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool TwoPair()
        {
            int count = 0;

            foreach (var item in rankCount)
            {
                if (item.Value == 2)
                {
                    count++;
                }

                if (count == 2)
                {
                    return true;
                }
            }

            return false;
        }

        private bool Triple()
        {
            foreach (var item in rankCount)
            {
                if (item.Value == 3)
                {
                    return true;
                }
            }
            return false;
        }

        private bool Straight()
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

        private bool BackStraight()
        {
            if (hand.deck[0].rank == Rank.ACE &&
                hand.deck[1].rank == Rank.TWO &&
                hand.deck[2].rank == Rank.THREE &&
                hand.deck[3].rank == Rank.FOUR &&
                hand.deck[4].rank == Rank.FIVE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Mountain()
        {
            if (hand.deck[0].rank == Rank.TEN &&
                hand.deck[1].rank == Rank.JACK &&
                hand.deck[2].rank == Rank.QUEEN &&
                hand.deck[3].rank == Rank.KING &&
                hand.deck[4].rank == Rank.ACE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Flush()
        {
            // 1. 전부 같은 모양인지 확인
            foreach (var item in suitCount)
            {
                if (item.Value == 5)
                {
                    return true;
                }
            }
            return false;
        }

        private bool FullHouse()
        {
            bool tripleFlag = false;
            bool pairFlag = false;

            foreach (var item in rankCount)
            {
                if (item.Value == 3)
                {
                    tripleFlag = true;
                }

                if (item.Value == 2)
                {
                    pairFlag = true;
                }

                if (pairFlag && tripleFlag)
                {
                    return true;
                }
            }

            return false;
        }

        private bool FourCard()
        {
            foreach (var item in rankCount)
            {
                if (item.Value == 4)
                {
                    return true;
                }
            }
            return false;
        }

        private bool StraightFlush()
        {
            // 1. 전부 같은 모양인지 확인
            foreach (var item in suitCount)
            {
                if (item.Value == 5)
                {
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
            }

            return false;
        }

        private bool BackStraightFlush()
        {
            // 1. 전부 같은 모양인지 확인
            foreach (var item in suitCount)
            {
                if (item.Value == 5)
                {
                    // 2. 전부 같은 모양이면 숫자 확인
                    if (hand.deck[0].rank == Rank.ACE &&
                        hand.deck[1].rank == Rank.TWO &&
                        hand.deck[2].rank == Rank.THREE &&
                        hand.deck[3].rank == Rank.FOUR &&
                        hand.deck[4].rank == Rank.FIVE)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        private bool RoyalStraightFlush()
        {
            // 1. 전부 같은 모양인지 확인
            foreach (var item in suitCount)
            {
                if (item.Value == 5)
                {
                    // 2. 전부 같은 모양이면 숫자 확인
                    if (hand.deck[0].rank == Rank.TEN &&
                        hand.deck[1].rank == Rank.JACK &&
                        hand.deck[2].rank == Rank.QUEEN &&
                        hand.deck[3].rank == Rank.KING &&
                        hand.deck[4].rank == Rank.ACE)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
