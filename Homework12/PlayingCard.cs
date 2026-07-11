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
    using System.Runtime.CompilerServices;
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

        class CardFigure // 싱글턴으로 만들 것
        {
            private Dictionary<Suit, Dictionary<Rank, string>> cardMap;

            public CardFigure()
            {
                cardMap = new Dictionary<Suit, Dictionary<Rank, string>>();

                foreach(Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    cardMap[suit]  = new Dictionary<Rank, string>();

                    foreach(Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        cardMap[suit][rank] = $"{Enum.GetName(typeof (Suit), suit)} {Enum.GetName(typeof (Rank), rank)}";
                    }
                }
            }

            public Dictionary<Suit, Dictionary<Rank, string>> getCardFigure()
            {
                return this.cardMap;
            }
        }

        class CardDeck
        {
            public List<Card> deck { get; private set; }
            public Dictionary<Suit, Dictionary<Rank, string>> cardFigure;

            public CardDeck()
            {
                cardFigure = new CardFigure().getCardFigure();
                deck = new List<Card>();
            }

            public void createCompleteDeck()
            {
                int index = 0;

                foreach (int suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (int rank in Enum.GetValues(typeof(Rank)))
                    {
                        deck[index] = new Card((Suit)suit, (Rank)rank);
                    }
                }
            }

            public void push(Card card)
            {
                deck.Add(card);
            }

            public Card pop()
            {
                Card tmp = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
                return tmp;
            }

            public int getCount()
            {
                return deck.Count;
            }

            public void shuffle()
            {
                Random random = new Random();

                for (int i = 0; i < deck.Count; i++)
                {
                    int destIndex = random.Next() % deck.Count;
                    int sourIndex = random.Next() % deck.Count;
                    Card tmp = deck[destIndex];
                    deck[destIndex] = deck[sourIndex];
                    deck[sourIndex] = tmp;
                }
            }

            public void showAllCards()
            {
                int count = deck.Count;

                Console.WriteLine("======================================================================================");
                for (int i = 0; i < count; i++)
                {
                    Console.Write($"| {cardFigure[deck[i].suit][deck[i].rank]} |");
                }
                Console.WriteLine("=======================================================================================");
            }
        }
    }

}
