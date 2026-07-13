using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework12
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

    class CardFigure
    {
        private Dictionary<Suit, Dictionary<Rank, string>> cardMap;

        public CardFigure()
        {
            cardMap = new Dictionary<Suit, Dictionary<Rank, string>>();

            foreach(Suit suit in Enum.GetValues(typeof(Suit)))
            {
                cardMap[suit]  = new Dictionary<Rank, string>();
            }

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    string suitSymbol = "";
                    string rankText = "";

                    // 문양 결정
                    switch (suit)
                    {
                        case Suit.SPADE:
                            suitSymbol = "♠";
                            break;

                        case Suit.HEART:
                            suitSymbol = "♥";
                            break;

                        case Suit.DIAMOND:
                            suitSymbol = "♦";
                            break;

                        case Suit.CLOVER:
                            suitSymbol = "♣";
                            break;
                    }

                    // 숫자/그림 결정
                    switch (rank)
                    {
                        case Rank.ACE:
                            rankText = "A";
                            break;

                        case Rank.TWO:
                            rankText = "2";
                            break;

                        case Rank.THREE:
                            rankText = "3";
                            break;

                        case Rank.FOUR:
                            rankText = "4";
                            break;

                        case Rank.FIVE:
                            rankText = "5";
                            break;

                        case Rank.SIX:
                            rankText = "6";
                            break;

                        case Rank.SEVEN:
                            rankText = "7";
                            break;

                        case Rank.EIGHT:
                            rankText = "8";
                            break;

                        case Rank.NINE:
                            rankText = "9";
                            break;

                        case Rank.TEN:
                            rankText = "10";
                            break;

                        case Rank.JACK:
                            rankText = "J";
                            break;

                        case Rank.QUEEN:
                            rankText = "Q";
                            break;

                        case Rank.KING:
                            rankText = "K";
                            break;
                    }

                    cardMap[suit][rank] =
                        "┏━━━━━━━━┓\n" +
                        "┃" + rankText.PadRight(2) + "      ┃\n" +
                        "┃        ┃\n" +
                        "┃   " + suitSymbol + "    ┃\n" +
                        "┃        ┃\n" +
                        "┃      " + rankText.PadLeft(2) + "┃\n" +
                        "┗━━━━━━━━┛";
                }
            }
        }

        public Dictionary<Suit, Dictionary<Rank, string>> getCardFigure()
        {
            return cardMap;
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
            foreach (int suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (int rank in Enum.GetValues(typeof(Rank)))
                {
                    deck.Add(new Card((Suit)suit, (Rank)rank));
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

        public void shuffle(Random random, int count)
        {
            for (int i = 0; i < count; i++)
            {
                int destIndex = random.Next() % deck.Count;
                int sourIndex = random.Next() % deck.Count;
                Card tmp = deck[destIndex];
                deck[destIndex] = deck[sourIndex];
                deck[sourIndex] = tmp;
            }
        }

        private void PrintCards(params string[] cards)
        {
            string[][] cardLines = new string[cards.Length][];

            for (int i = 0; i < cards.Length; i++)
            {
                cardLines[i] = cards[i].Split('\n');
            }

            for (int line = 0; line < cardLines[0].Length; line++)
            {
                for (int card = 0; card < cardLines.Length; card++)
                {
                    Console.Write(cardLines[card][line]);

                    if (card != cardLines.Length - 1)
                    {
                        Console.Write("  ");
                    }
                }

                Console.WriteLine();
            }
        }

        public void showAllCards()
        {
            string[] cards = new string[deck.Count];

            for (int i = 0; i < deck.Count; i++)
            {
                cards[i] = cardFigure[deck[i].suit][deck[i].rank];
            }

            PrintCards(cards);
        }
    }

}
