using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 1. 트럼프 카드 객체 만들기
 * 2. 덱 만들기?
 * 3. 셔플
 * 4. 카드 뽑기
 * 5. 뽑은 카드 확인
 * 6. 족보 판정을 함
 * 7. 4번부터 7번까지 6번 반복
 * 8. 마지막 족보 출력
 * 9. 리트라이?
 * 10 리트라이면 2번, 종료면 종료
 */



namespace Class12
{
    public enum Pattern
    {
        Spade = 0, Diamond, Heart, Clover
    }

    class Card
    {
        public int Num;
        public Pattern Pattern;

        public Card(int num, Pattern pattern)
        {
            Num = num;
            Pattern = pattern;
        }

        public void ViewingCard()
        {
            string Pattern_s = string.Empty;
            switch (Pattern)
            {
                case Pattern.Spade:
                    Pattern_s = "spade";
                    break;
                case Pattern.Diamond:
                    Pattern_s = "diamond";
                    break;
                case Pattern.Heart:
                    Pattern_s = "heart";
                    break;
                case Pattern.Clover:
                    Pattern_s = "clover";
                    break;
            }

            string Number_s = string.Empty;
            switch (Num)
            {
                case 1:
                    Number_s = "A";
                    break;
                case 11:
                    Number_s = "J";
                    break;
                case 12:
                    Number_s = "Q";
                    break;
                case 13:
                    Number_s = "K";
                    break;
                default:
                    Number_s = Num.ToString();
                    break;
            }

            Console.WriteLine($"[{Pattern_s} {Number_s}]");
        }
    }

    class Result
    {
        /*
         * 족보 판독기
         * 1. 정렬 -> 선택정렬알고리즘
         * 2. 특정 카드 검사
         * 3. 모든 무늬 같은지 검사
         * 4. 숫자가 연속되는지 검사
         */

        public List<Card> SortCard(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = i + 1; j < cards.Count; j++)
                {
                    if (cards[i].Num > cards[j].Num) // 부등호 > 면 오름차순 < 면 내림차순
                    {
                        Card tmp = cards[i];
                        cards[i] = cards[j];
                        cards[j] = tmp;
                    }
                }
            }

            return cards;
        }
        public bool isSearchCard(int num, List<Card> card)
        {
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i].Num.Equals(num))
                {
                    return true;
                }
            }

            return false;
        }
        public Card isSearchCard_c(int num, List<Card> card)
        {
            for (int i = 0; i < card.Count; i++)
            {
                if (card[i].Num.Equals(num))
                {
                    return card[i];
                }
            }

            return new Card(0, 0);
        }
        public bool isSameSimbol(List<Card> card)
        {
            for (int i = 1; i < card.Count; i++)
            {
                if (!(card[i - 1].Pattern.Equals(card[i].Pattern)))
                {
                    return false;
                }
            }

            return true;
        }
        public bool isCountinuityNum(List<Card> card, out List<Card> madeCard)
        // out: 매개변수로 변수나 자료구조를 메서드 밖으로 빼내는 법
        // ref: 어떠한 변수나 자료구조를 참조할 때 사용, 메서드 안에서 변경되면 ref 또한 변경
        // 기본적으로는 매개변수가 복사를 하기 때문에 메서드 완료하고 덮어 써야 하는데 쓰기 귀찮으니까 만든듯
        {
            int CardStartIndex = 0; // 몇번째 카드부터 검사할거야?
            int Straight_index = 0; // 연속되면 카운트를 올려서 5가 되면 스트레이트

            madeCard = new List<Card>();

            while (CardStartIndex <= card.Count - 5) // 5개의 숫자가 연속될 때 -> 비교할 카드가 5장 이상 남아야 성립함
            {
                if (CardStartIndex > (card.Count - 5))
                {
                    if (madeCard.Count > 0)
                    {
                        madeCard.Clear();
                    }
                    return false;
                }

                int current_num = card[CardStartIndex].Num;
                int pastNum = 0;

                for (int i = CardStartIndex; i < card.Count; i++)
                {
                    if (i == CardStartIndex)
                    {
                        pastNum = current_num + 1;
                        Straight_index += 1;
                        madeCard.Add(card[i]);
                    }
                    else if (!(card[i].Num.Equals(pastNum)) || pastNum > 13)
                    {
                        CardStartIndex += 1;
                        Straight_index = 0;
                        pastNum = 0;
                        if (madeCard.Count > 0)
                        {
                            madeCard.Clear();
                        }
                        break;
                    }
                    else
                    {
                        Straight_index += 1;
                        pastNum += 1;
                        madeCard.Add(card[i]);
                    }
                }

                if (Straight_index.Equals(5))
                {
                    return true;
                }
            }

            if (madeCard.Count > 0)
            {
                madeCard.Clear();
            }
            return false;
        }
        private bool isRoyalStraightFlush(List<Card> card) // 로얄 스트레이트 플러쉬 : 10 J Q K A를 같은 무늬로 모은 경우
        {
            if (card.Count < 5) return false;

            List<Card> madeCard = new List<Card>();
            madeCard.Add(isSearchCard_c(10, card));
            madeCard.Add(isSearchCard_c(11, card));
            madeCard.Add(isSearchCard_c(12, card));
            madeCard.Add(isSearchCard_c(13, card));
            madeCard.Add(isSearchCard_c(1, card));

            for (int i = 0; i < madeCard.Count; i++)
            {
                if (madeCard[i].Num.Equals(0)) return false;
            }

            // 여기까지 왔다면 필요한 카드는 다 있다!

            if (isSameSimbol(madeCard)) return true;

            return false;
        }
        private bool isBackStraightFlush(List<Card> card) // 백 스트레이트 플러쉬 : A 2 3 4 5를 같은 무늬로 모은 경우
        {
            if (card.Count < 5) return false;

            List<Card> madeCard = new List<Card>();
            madeCard.Add(isSearchCard_c(1, card));
            madeCard.Add(isSearchCard_c(2, card));
            madeCard.Add(isSearchCard_c(3, card));
            madeCard.Add(isSearchCard_c(4, card));
            madeCard.Add(isSearchCard_c(5, card));

            for (int i = 0; i < madeCard.Count; i++)
            {
                if (madeCard[i].Num.Equals(0)) return false;
            }

            // 여기까지 왔다면 필요한 카드는 다 있다!

            if (isSameSimbol(madeCard)) return true;

            return false;
        }
        private bool isStraightFlush(List<Card> card) // 스트레이트 플러쉬 : 연속되는 숫자 5개를 같은 무늬로 모은 경우
        {
            if (card.Count < 5) return false;

            card = SortCard(card);

            if (card[0].Num > 10) return false; // 정렬된 첫카드가 10보다 크면 필요 없음
            else
            {
                if (isCountinuityNum(card, out List<Card> madeCard))
                {
                    if (!isSameSimbol(madeCard))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private bool isFourCard(List<Card> card) //  무늬가 다르지만 같은 숫자 4개를 모은 경우
        {
            if (card.Count < 4) return false;

            int count = 0;

            List<int> numList = new List<int>();

            for (int i = 0; i < card.Count; i++)
            {
                for (int j = 0; j < card.Count; j++)
                {
                    if (j != i && card[i].Num.Equals(card[j].Num))
                    {
                        count += 1;
                        numList.Add(card[j].Num);
                    }
                }
            }

            for (int i = 1; i < numList.Count; i++)
            {
                if (numList[i - 1].Equals(numList[i]))
                {
                    return false;
                }
            }

            if (count.Equals(12))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isFullHouse(List<Card> card) // 같은 숫자 3개와 2개의 조합
        {
            if (card.Count < 5) return false;

            card = SortCard(card);

            int count = 0;

            for (int i = 0; i < card.Count; i++)
            {
                for (int j = 0; j < card.Count; j++)
                {
                    if (j != i && card[i].Num.Equals(card[j].Num))
                    {
                        count += 1;
                    }
                }
            }

            if (count.Equals(8))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isFlush(List<Card> card) // 같은 무늬의 카드 5개
        {
            if (card.Count < 5) return false;

            int count = 0;

            for (int i = 0; i < card.Count; i++)
            {
                for (int j = 0; j < card.Count; j++)
                {
                    if (j != i && card[i].Pattern.Equals(card[j].Pattern))
                    {
                        count += 1;
                    }
                }
            }

            if (count >= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isBackStraight(List<Card> card) // A 2 3 4 5 를 모은 경우
        {
            if (card.Count < 5) return false;

            card = SortCard(card);

            if (isSearchCard(1, card) &&
                isSearchCard(2, card) &&
                isSearchCard(3, card) &&
                isSearchCard(4, card) &&
                isSearchCard(5, card))
            {
                return true;
            }
            return false;
        }
        private bool isMountain(List<Card> card) // 10 J Q K A
        {
            if (card.Count < 5) return false;

            card = SortCard(card);

            if (isSearchCard(10, card) &&
                isSearchCard(11, card) &&
                isSearchCard(12, card) &&
                isSearchCard(13, card) &&
                isSearchCard(1, card))
            {
                return true;
            }
            return false;
        }
        private bool isStraight(List<Card> card) // 연속되는 숫자 5개
        {
            if (card.Count < 5) return false;

            return isCountinuityNum(card, out List<Card> a);
        }
        private bool isTriple(List<Card> card) // 같은 숫자 3개
        {
            if (card.Count < 3) return false;

            int count = 0;
            int[] array_int = new int[3];

            for (int i = 0; i < card.Count; i++)
            {
                for (int j = 0; j < card.Count; j++)
                {
                    if (i != j && card[i].Num.Equals(card[j].Num))
                    {
                        count += 1;
                        if (count % 2 == 0)
                        {
                            array_int[(count / 2) - 1] = card[i].Num;
                        }
                    }
                }
            }

            if (count.Equals(6))
            {
                if (array_int[0].Equals(array_int[1]) && array_int[0].Equals(array_int[2]) && array_int[1].Equals(array_int[2]))
                {
                    return true;
                }
            }

            return false;
        }
        private bool isTwoPair(List<Card> card) // 같은 숫자 2개가 2개
        {
            if (card.Count < 4) return false;

            card = SortCard(card);
            int count = 0;
            int pastNum = 0;

            for (int i = 0; i < card.Count; i++)
            {
                for (int j = 0; j < card.Count; j++)
                {
                    if (i != j && card[i].Num.Equals(card[j].Num))
                    {
                        count += 1;
                        if (count.Equals(2))
                        {
                            pastNum = card[i].Num;
                            break;
                        }
                    }
                }
            }

            if (count >= 2)
            {
                count = 0;
            }
            else
            {
                return false;
            }

            // ===========================
            for (int i = 0; i < card.Count; i++)
            {
                for (int j = 0; j < card.Count; j++)
                {
                    if (i != j && card[i].Num.Equals(card[j].Num) && pastNum != card[i].Num)
                    {
                        count += 1;
                        if (count.Equals(2))
                        {
                            break;
                        }
                    }
                }

            }

            if (count.Equals(2) && pastNum != 0)
            {
                return true;
            }
            else
            {
                return false;

            }

        }
        private bool isPair(List<Card> card)
        {
            if (card.Count < 2) return false;

            card = SortCard(card);
            int count = 0;

            for (int i = 0; i < card.Count; i++)
            {
                for (int j = 0; j < card.Count; j++)
                {
                    if (i != j && card[i].Num.Equals(card[j].Num))
                    {
                        count += 1;
                        if (count.Equals(2))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public string Result_card(List<Card> card)
        {
            string result = string.Empty;

            if (isRoyalStraightFlush(card)) result = "RoyalStraightFlush";
            else
            {
                if (isBackStraight(card)) result = "BackStraight";
                else
                {
                    if (isStraightFlush(card)) result = "StraightFlush";
                    else
                    {
                        if (isFourCard(card)) result = "Fourcard";
                        else
                        {
                            if (isFullHouse(card)) result = "FullHouse";
                            else
                            {
                                if (isFlush(card)) result = "Flush";
                                else
                                {
                                    if (isBackStraight(card)) result = "BackStraight";
                                    else
                                    {
                                        if (isMountain(card)) result = "Mountain";
                                        else
                                        {
                                            if (isStraight(card)) result = "Straight";
                                            else
                                            {
                                                if (isTriple(card)) result = "Triple";
                                                else
                                                {
                                                    if (isTwoPair(card)) result = "TwoPair";
                                                    else
                                                    {
                                                        if (isPair(card)) result = "Pair";
                                                        else
                                                        {
                                                            result = "NoPair";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
    class Program
    {
        public List<Card> InitCard()
        {
            List<Card> trumpCard = new List<Card>();

            for (int i = 0; i < 4; i++)
            {
                for (int k = 1; k <= 13; k++)
                {
                    trumpCard.Add(new Card(k, (Pattern)i));
                }
            }

            Random rnd = new Random();

            for (int i = 0; i < 10000; i++)
            {
                int F_index = rnd.Next() % trumpCard.Count;
                int S_index = rnd.Next() % trumpCard.Count;

                // C# 비교연산자 (== : 우변에 nullptr를 생성함)
                // C# Equals : 매개변수로 받음. 가비지컬렉터가 돌지 않아 조금 빨라진다고 함.

                if (F_index.Equals(S_index))
                {
                    continue;
                }

                Card tmp = trumpCard[F_index];
                trumpCard[F_index] = trumpCard[S_index];
                trumpCard[S_index] = tmp;
            }

            return trumpCard;
        }

        public Card Draw_Card(ref List<Card> Trump)
        {
            Random rnd = new Random();

            int index = rnd.Next() % Trump.Count;

            Card returnCard = Trump[index];

            Trump.RemoveAt(index);

            return returnCard;
        }

        static void Main(string[] args)
        {

            Program p = new Program();
            Result r = new Result();

            List<Card> mainCard = p.InitCard();

            for (int i = 0; i < mainCard.Count; i++)
            {
                if (i % 13 == 0)
                {
                    Console.WriteLine();
                }
                mainCard[i].ViewingCard();
            }

            List<Card> PlayerCard = new List<Card>();

            while (true)
            {
                Console.Clear();

                if(PlayerCard.Count >= 7)
                {
                    Console.WriteLine("뽑은 카드가 7장이라서 게임을 초기화 합니다.");
                    Console.ReadLine();
                    Console.Clear();

                    mainCard = p.InitCard();
                    PlayerCard = new List<Card>();
                }

                PlayerCard.Add(p.Draw_Card(ref mainCard));
                PlayerCard = r.SortCard(PlayerCard);

                Console.WriteLine("PlayerCard: ");
                for (int i = 0; i < PlayerCard.Count; i++)
                {
                    PlayerCard[i].ViewingCard();
                }

                Console.WriteLine();
                Console.WriteLine($"Playermade: [{r.Result_card(PlayerCard)}]");
                Console.ReadLine();
            }
        }
    }
}
