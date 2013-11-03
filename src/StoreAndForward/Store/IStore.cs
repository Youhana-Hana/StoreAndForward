namespace StoreAndForward
{
    using System;
    using System.Collections.Generic;

    public interface IStore
    {
        event EventHandler<MessageAddedEventArgs> MessageAdded;

        int Count { get; }

        IMessage Add(IMessage message);

        void Remove(IMessage message);

        IList<IMessage> Get();
   }
}
