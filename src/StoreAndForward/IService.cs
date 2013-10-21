namespace StoreAndForward
{
    public interface IService
    {
        void Start();
        
        IMessage Store(IMessage message);

        void Stop();
    }
}
