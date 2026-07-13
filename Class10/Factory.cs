using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 팩토리
 * 객체 생성 로직을 클라이언트와 분리하여 따로 관리하는 디자인 패턴
 */

namespace Class10
{
    public interface IChocolate // 팩토리 메서드 패턴을 사용해서 행동을 정의한 뒤 각 객체마다 재정의를 하게끔 만들었다.
    {
        void Made();
    }

    public class MilkChocolate : IChocolate
    {
        public void Made()
        {
            Console.WriteLine("밀크 초콜릿 생성");
        }
    }

    public class DarkChocolate : IChocolate
    {
        public void Made()
        {
            Console.WriteLine("다크 초콜릿 생성");
        }
    }

    public class WhiteChocolate : IChocolate
    {
        public void Made()
        {
            Console.WriteLine("화이트 초콜릿 생성");
        }
    }

    public abstract class ChocolateFactory // 초콜릿 팩토리 추상 클래스...
    {
        public abstract IChocolate CreateChocolate();

        public void MakeChocolate()
        {
            IChocolate chocolate = CreateChocolate();
            chocolate.Made();
        }
    }

    public class MilkChocolateFactory : ChocolateFactory
    {
        public override IChocolate CreateChocolate()
        {
            return new MilkChocolate();
        }
    }
    
    public class DarkChocolateFactory : ChocolateFactory
    {
        public override IChocolate CreateChocolate()
        {
            return new DarkChocolate();
        }
    }
    
    public class WhiteChocolateFactory : ChocolateFactory
    {
        public override IChocolate CreateChocolate()
        {
            return new WhiteChocolate();
        }
    }
}
