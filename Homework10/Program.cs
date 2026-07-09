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
            // 1. 카드와 플레이어와 딜러가 존재
            PlayingCardDeck deck = new PlayingCardDeck();
            deck.shuffle(1000);

            // 2. 베팅
            

            // 3. 플레이어와 딜러에게 각각 2장씩 배분. 딜러는 1장 선택해서 공개.
            // 4. 플레이어 뽑기 턴. 패의 합이 21에 가까워지도록 뽑고 언제든 멈출 수 있음. 21을 넘으면 패배
            // 5. 딜러 뽑기 턴. 플레이어와 마찬가지지만 패의 합이 17이상이 될 때까지는 멈출 수 없음.
            // 6. 패 공개하고 21에 가까운 쪽이 베팅 금액을 가져감. 비기면 베팅한 금액은 돌려받음.
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.GameLoop();
        }
    }
}
