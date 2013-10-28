namespace StoreAndForward
{
    using System.Collections.Generic;

    public interface IStore
    {
        int Count { get; }

        IMessage Add(IMessage message);

        void Remove(IMessage message);

        IList<IMessage> Get();
   }
}
