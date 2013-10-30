namespace StoreAndForward
{
    public interface INewMessageHandler
    {
        void OnMessageAdded(IMessage message);
    }
}
