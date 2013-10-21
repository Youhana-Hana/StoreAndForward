using System;
using System.Collections.Generic;
namespace StoreAndForward
{
    public class QueueStore : IStore
    {

        public QueueStore()
        {
            this.Store = new Queue<IMessage>();
        }
        
        private Queue<IMessage> Store { get; set; }

        public IMessage Add(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            return null;
        }

        public IMessage Remove(IMessage message)
        {
            return null;
        }
    }
}
