interface HpObserver
{
    void OnHpChange();
}

class Sound : HpObserver
{
    public void OnHpChange()
    {
        Console.WriteLine("쾅!");
    }
}

class Effect : HpObserver
{
    public void OnHpChange()
    {
        Console.WriteLine("[대충 아픈 이펙트]");
    }
}

class Player
{
    List<HpObserver> observers = new List<HpObserver>();

    // 옵저버를 붙이고 때는 메서드
    public void addObserver(HpObserver observer)
    {
        observers.Add(observer);
    }

    public void removeObserver(HpObserver observer)
    {
        observers.Remove(observer);
    }

    public void takeDamage()
    {
        foreach (var observer in observers)
        {
            observer.OnHpChange();
        }
    }
}


/*
 * 구독 - 구독자 간 결합도를 낮출 수 있음. 즉 [느슨한 결합도]를 만들 수 있다.
 * 다른 말로 낮은 커플링을 갖게 된다.
 * 유니티에는 이미 만들어져 있다.
 */

class Program
{
    static void Main()
    {
        Player player = new Player();
        HpObserver sound = new Sound();
        HpObserver effect = new Effect();

        player.addObserver(sound);
        player.addObserver(effect);

        player.takeDamage();

        player.removeObserver(sound);

        player.takeDamage();
    }
}