using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework12
{
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

        enum Rank
        {
            ACE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING
        }

        class Card
        {
            public Suit suit { get; }
            public Rank rank { get; }

            public Card(Suit suit, Rank rank)
            {
                this.suit = suit;
                this.rank = rank;
            }
        }

        class CardDeck
        {
            private List<Card> deck;

            public CardDeck()
            {
                deck = new List<Card>();

                int index = 0;

                foreach (int suit in Enum.GetValues(typeof (Suit)))
                {
                    foreach (int rank in Enum.GetValues(typeof(Rank)))
                    {
                        deck[index] = new Card((Suit)suit, (Rank)rank);
                    }
                } 
            }

            public void showAllCards()
            {
                int count = deck.Count;

                Console.WriteLine("=================");
                for (int i = 0; i < count; i++)
                {
                    deck[i];
                }
                Console.WriteLine("=================");
            }
        }
    }

}
