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

        public void showCard()
        {
            string suit = "";
            string card = "";

            switch (this.suit)
            {
                case Suit.SPADE:
                    suit = "SPADE";
                    break;
                case Suit.DIAMOND:
                    suit = "DIAMOND";
                    break;
                case Suit.HEART:
                    suit = "HEART";
                    break;
                case Suit.CLOVER:
                    suit = "CLOVER";
                    break;
            }

            switch (this.card)
            {
                case Card.ACE:
                    card = "ACE";
                    break;
                case Card.TWO:
                    card = "2";
                    break;
                case Card.THREE:
                    card = "3";
                    break;
                case Card.FOUR:
                    card = "4";
                    break;
                case Card.FIVE:
                    card = "5";
                    break;
                case Card.SIX:
                    card = "6";
                    break;
                case Card.SEVEN:
                    card = "7";
                    break;
                case Card.EIGHT:
                    card = "8";
                    break;
                case Card.NINE:
                    card = "9";
                    break;
                case Card.TEN:
                    card = "10";
                    break;
                case Card.JACK:
                    card = "J";
                    break;
                case Card.QUEEN:
                    card = "Q";
                    break;
                case Card.KING:
                    card = "K";
                    break;
            }

            Console.WriteLine($"{suit} {card}");
        }
    }

    class PlayingCardDeck // 나중에 스택 관련 메서드들을 따로 분리할 것
    {
        public PlayingCard[] playingCards { get; private set; }

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

        public void makeEmpty()
        {
            int count = getLastIndex() + 1;
            for (int i = 0; i < count; i++)
            {
                playingCards[i] = null;
            }
        }


        public void showAllCards()
        {
            int count = getLastIndex() + 1;
            Console.WriteLine("=================");
            for(int i = 0; i < count; i++)
            {
                playingCards[i].showCard();
            }
            Console.WriteLine("=================");
        }
    }
}
