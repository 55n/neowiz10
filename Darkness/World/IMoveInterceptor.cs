namespace Darkness
{
    public interface IMoveInterceptor
    {
        MoveInterceptionResult TryIntercept(
            MoveInterceptionContext context);
    }
}
