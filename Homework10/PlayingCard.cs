using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework10
{
    enum Suit
    {
        SPADE, DIAMOND, HEART, CLOVER
    }

    enum Card
    {
        ACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING
    }

    class PlayingCard
    {
        public Suit suit { get; }
        public Card card { get; }

        public PlayingCard(Suit suit, Card card)
        {
            this.suit = suit;
            this.card = card;
        }
    }

    class PlayingCardDeck // 나중에 스택 관련 메서드들을 따로 분리할 것
    {
        private PlayingCard[] playingCards;

        public void createDeck()
        {
            playingCards = new PlayingCard[52];

            int playingCardsIndex = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    playingCards[playingCardsIndex] = new PlayingCard((Suit)i, (Card)j);
                    playingCardsIndex++;
                }
            }
        }

        public void createHand()
        {
            playingCards = new PlayingCard[12];
        }

        public void shuffle(int count)
        {
            Random random = new Random();
            for(int i = 0; i < count; i++)
            {
                int destIndex = random.Next() % playingCards.Length;
                int sourIndex = random.Next() % playingCards.Length;
                PlayingCard tmp = playingCards[destIndex];
                playingCards[destIndex] = playingCards[sourIndex];
                playingCards[sourIndex] = tmp;
            }
        }

        public int getLastIndex()
        {
            int lastIndex = -1;

            if (playingCards[0] == null) lastIndex = -1; // 스택에 아무 것도 없으면 -1 반환
            else if (playingCards[playingCards.Length - 1] != null) lastIndex = playingCards.Length - 1;
            else
            {
                for (int i = 0; i < playingCards.Length; i++)
                {
                    if (playingCards[i] == null)
                    {
                        lastIndex = i - 1;
                        break;
                    }
                }
            }

            return lastIndex;
        }

        public bool pushCardIntoDeck(PlayingCard card)
        {
            int index = getLastIndex();
            if (index == playingCards.Length - 1)
            {
                return false;
            }

            if (index < 0)
            {
                playingCards[0] = card;
                return true;
            }
            else
            {
                playingCards[index + 1] = card;
                return true;
            }
        }

        public PlayingCard popCardFromDeck()
        {
            int index = getLastIndex();

            if (index < 0)
            {
                return null;
            }
            else
            {
                PlayingCard tmp = playingCards[index];
                playingCards[index] = null;
                return tmp;
            }
        }
    }
}
