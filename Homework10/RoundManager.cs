using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework10
{
    class RoundManager
    {
        public int scoring(PlayingCard playingCard)
        {
            if(playingCard.card == Card.ACE)
            {
                return 1;
            }
            else if(
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
            else if(
                playingCard.card == Card.TEN ||
                playingCard.card == Card.JACK ||
                playingCard.card == Card.QUEEN ||
                playingCard.card == Card.KING
                )
            {
                return 10;
            }
        }
    }
}
