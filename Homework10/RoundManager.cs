using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework10
{
    class RoundManager
    {
        public int BLACKJACK = 21;
        public int ACE_TEN = 10;

        public int handScoring(PlayingCardDeck hand)
        {
            if (hand.playingCards[0] == null) return -1;

            int sum = 0;
            int ace = 0;

            for(int i = 0; i < (hand.getLastIndex() + 1); i++)
            {
                if (hand.playingCards[i].card == Card.ACE) ace++;
                sum += cardScoring(hand.playingCards[i]);
            }

            if()
            return sum + 10;
            return sum;
        }

        public int cardScoring(PlayingCard playingCard)
        {
            if (playingCard.card == Card.ACE)
            {
                return 1;
            }
            else if (
                playingCard.card == Card.TWO ||
                playingCard.card == Card.THREE ||
                playingCard.card == Card.FOUR ||
                playingCard.card == Card.FIVE ||
                playingCard.card == Card.SIX ||
                playingCard.card == Card.SEVEN ||
                playingCard.card == Card.EIGHT ||
                playingCard.card == Card.NINE
                )
            {
                return (int)playingCard.card + 1;
            }
            else if (
                playingCard.card == Card.TEN ||
                playingCard.card == Card.JACK ||
                playingCard.card == Card.QUEEN ||
                playingCard.card == Card.KING
                )
            {
                return 10;
            }
            else
            {
                return -1;
            }
        }
    }
}
