namespace StoreAndForward
{
    using System;
    using System.Collections.Generic;

    public class QueueStore
    {
        public QueueStore()
        {
            this.Store = new Queue<IMessage>();
        }

        public int Count
        {
            get
            {
                return this.Store.Count;
            }
        }

        private Queue<IMessage> Store { get; set; }

        public IMessage Enqueue(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.Store.Enqueue(message);

            return message;
        }

        public IMessage Dequeue()
        {
            if (this.Store.Count == 0)
            {
                return null;
            }
            
            return this.Store.Dequeue();
        }
    }
}
