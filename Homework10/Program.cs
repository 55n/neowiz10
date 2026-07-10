using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 블랙잭 만들기
// 규칙은 https://2010hhh.tistory.com/2446 참고
// 소지금을 한쪽이 전부 잃으면 게임 종료

namespace Homework10
{

    class Program
    {
        public void GameLoop()
        {
            RoundManager rm = new RoundManager();

            

            Player player = new Player("아무개", 10000);
            Player dealer = new Player("아귀", 10000);

            while (true)
            {
                if (player.stake  < 0|| dealer.stake < 0)
                {
                    Console.WriteLine($"플레이어 소지금: {player.stake}");
                    Console.WriteLine($"딜러 소지금: {dealer.stake}");
                    Console.WriteLine("게임을 종료합니다");
                    break;
                }

                PlayingCardDeck deck = new PlayingCardDeck();

                deck.createDeck();
                deck.shuffle(1000);

                bool roundEndFlag = false;
                Random random = new Random();

                player.hand.makeEmpty();
                dealer.hand.makeEmpty();

                // 1. 베팅
                Console.WriteLine("베팅 할 시간!");
                Console.Write($"베팅 금액 (플레이어 소지금:{player.stake} | 딜러 소지금:{dealer.stake}) : ");
                int bettingCost = int.Parse(Console.ReadLine());
                bool isBettingSuccess = rm.betting(player, dealer, bettingCost);

                while (!isBettingSuccess)
                {
                    Console.Write("베팅 금액: ");
                    bettingCost = int.Parse(Console.ReadLine());
                    isBettingSuccess = rm.betting(player, dealer, bettingCost);
                }

                Console.WriteLine($"베팅금: {bettingCost}");

                // 2. 플레이어와 딜러 각각 2장씩 뽑기. 딜러는 1장 선택해서 공개.
                player.takeCardFromDeck(deck);
                player.takeCardFromDeck(deck);

                dealer.takeCardFromDeck(deck);
                dealer.takeCardFromDeck(deck);

                PlayingCard dealerCard1 = dealer.hand.playingCards[random.Next() % 2];

                Console.Write("딜러 카드: ");
                dealerCard1.showCard();

                // 4. 플레이어 뽑기 턴. 패의 합이 21에 가까워지도록 뽑고 언제든 멈출 수 있음. 21을 넘으면 패배
                player.hand.showAllCards();

                while (true)
                {
                    Console.WriteLine("카드를 뽑으시겠습니까?");
                    Console.WriteLine("| 1. 예 | 2. 아니요 |");
                    Console.Write("답변: ");
                    int pickMore = int.Parse(Console.ReadLine());

                    if(pickMore == 1)
                    {
                        player.takeCardFromDeck(deck);
                        player.hand.showAllCards();
                        bool burst = rm.isBurst(player.hand);

                        if(burst)
                        {
                            Console.WriteLine("패배하셨습니다!");
                            dealer.plusStake(rm.totalBetting[0]);
                            dealer.plusStake(rm.totalBetting[1]);
                            rm.totalBetting[0] = 0;
                            rm.totalBetting[1] = 0;

                            roundEndFlag = true;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (roundEndFlag)
                { 
                    continue;
                }

                // 5. 딜러 뽑기 턴. 플레이어와 마찬가지지만 패의 합이 17이상이 될 때까지는 멈출 수 없음.
                Console.WriteLine("이제 제 차례군요!");
                while (true)
                {
                    bool decision = rm.makeDealerDecision(dealer.hand);
                    if (decision)
                    {
                        dealer.takeCardFromDeck(deck);
                        Console.WriteLine("딜러는 고민도 하지 않고 뽑아 버렸다...");
                        bool burst = rm.isBurst(dealer.hand);
                        
                        if(burst)
                        {
                            Console.WriteLine("아이고 져버렸군요!");
                            player.plusStake(rm.totalBetting[0]);
                            player.plusStake(rm.totalBetting[1]);
                            rm.totalBetting[0] = 0;
                            rm.totalBetting[1] = 0;

                            roundEndFlag = true;
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("전 여기까지 하겠습니다");
                        break;
                    }
                }

                if (roundEndFlag)
                {
                    continue;
                }

                // 6. 패 공개하고 21에 가까운 쪽이 베팅 금액을 가져감. 비기면 베팅한 금액은 돌려받음.
                Console.WriteLine("이제 공개할 시간");

                int playerScoreDiff = 0;

                player.hand.showAllCards();

                if (rm.hasHandAce(player.hand))
                {
                    Console.WriteLine("에이스 하나를 11로 계산하시겠습니까?");
                    Console.Write("| 1. 예 | 2. 아니요 |");
                    int plus = int.Parse(Console.ReadLine());

                    if (plus == 1)
                    {
                        int playerScore = rm.handScoring(player.hand, true);
                        Console.WriteLine($"플레이어 점수: {playerScore}");
                        playerScoreDiff = 21 - playerScore;
                    }
                    else
                    {
                        int playerScore = rm.handScoring(player.hand, false);
                        Console.WriteLine($"플레이어 점수: {playerScore}");
                        playerScoreDiff = 21 - playerScore;
                    }
                }
                else
                {
                    int playerScore = rm.handScoring(player.hand, false);
                    Console.WriteLine($"플레이어 점수: {playerScore}");
                    playerScoreDiff = 21 - playerScore;
                }


                dealer.hand.showAllCards();
                int dealerScoreDiff = 0;

                int score = rm.handScoring(dealer.hand, false);
                int highScore = rm.handScoring(dealer.hand, true);

                if (rm.hasHandAce(dealer.hand))
                {
                    if(highScore > 21)
                    {
                        Console.WriteLine($"딜러 점수: {score}");
                        dealerScoreDiff = 21 - score;
                    }
                    else
                    {
                        if(highScore > score)
                        {
                            Console.WriteLine($"딜러 점수: {highScore}");
                            dealerScoreDiff = 21 - highScore;
                        }
                        else
                        {
                            Console.WriteLine($"딜러 점수: {score}");
                            dealerScoreDiff = 21 - score;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"딜러 점수: {score}");
                    dealerScoreDiff = 21 - score;
                }

                if(playerScoreDiff > dealerScoreDiff)
                {
                    Console.WriteLine("패배하셨습니다");
                    dealer.plusStake(rm.totalBetting[0]);
                    dealer.plusStake(rm.totalBetting[1]);
                    rm.totalBetting[0] = 0;
                    rm.totalBetting[1] = 0;
                }
                else
                {
                    Console.WriteLine("아이고 져버렸군요!");
                    player.plusStake(rm.totalBetting[0]);
                    player.plusStake(rm.totalBetting[1]);
                    rm.totalBetting[0] = 0;
                    rm.totalBetting[1] = 0;
                }
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.GameLoop();
        }
    }
}
