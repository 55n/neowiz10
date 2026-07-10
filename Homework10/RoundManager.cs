using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework10
{
    class RoundManager
    {
        private int BLACKJACK = 21;
        private int DEALER_THRESHOLD = 17;
        private int ACE_PLUS = 10;
        public int[] totalBetting = new int[2];

        public bool betting(Player player1, Player player2, int bettingCost)
        {
            int playerBetting = player1.bet(bettingCost);
            int dealerBetting = player2.bet(bettingCost);

            if(playerBetting == 0 || dealerBetting == 0)
            {
                return false;
            }
            else
            {
                totalBetting[0] = playerBetting;
                totalBetting[1] = dealerBetting;
                return true;
            }
        }

        public bool hasHandAce(PlayingCardDeck hand)
        {
            for(int i = 0; i < hand.getLastIndex() + 1;i++)
            {
                if(hand.playingCards[i].card == Card.ACE)
                {
                    return true;
                }
            }

            return false;
        }

        public int handScoring(PlayingCardDeck hand, bool acePlus)
        {
            if (hand.playingCards[0] == null) return -1;

            int sum = 0;
            int ace = 0;

            for(int i = 0; i < (hand.getLastIndex() + 1); i++)
            {
                if (hand.playingCards[i].card == Card.ACE) ace++;
                sum += cardScoring(hand.playingCards[i]);
            }

            if(acePlus)
            {
                return sum + ACE_PLUS;
            }
            else
            {
                return sum;
            }
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

        public bool makeDealerDecision(PlayingCardDeck hand)
        {
            int score = handScoring(hand, false);
            int highScore = 0;
            
            if (hasHandAce(hand)) highScore = handScoring(hand, true);

            int highScoreDiff = BLACKJACK - highScore;
            int scoreDiff = BLACKJACK - score;

            if (score < DEALER_THRESHOLD)
            {
                return true;
            }
            else
            {
                if (highScoreDiff == 0 || scoreDiff == 0)
                {
                    return false;
                }
                
                if(highScoreDiff > scoreDiff)
                {
                    if(highScoreDiff < 3)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if(scoreDiff > 5)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        public bool isBurst(PlayingCardDeck hand)
        {
            int score = handScoring(hand, false);
            if(score > BLACKJACK)
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
