namespace StoreAndForward
{
    public interface IStore
    {
        int Count { get; }

        IMessage Add(IMessage message);

        IMessage Remove(IMessage message);
    }
}
