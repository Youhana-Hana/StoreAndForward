namespace StoreAndForward
{
    public interface IHttpClient
    {
        PostResult Post(IMessage message);
    }
}
