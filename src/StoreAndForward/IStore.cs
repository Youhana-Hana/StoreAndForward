namespace StoreAndForward
{
    public interface IStore
    {
        IMessage Add(IMessage message);

        IMessage Remove(IMessage message);
    }
}
