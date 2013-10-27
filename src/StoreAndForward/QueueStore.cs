namespace StoreAndForward
{
    using System;
    using System.Collections.Generic;

    internal class QueueStore
    {
        internal QueueStore()
        {
            this.Store = new Queue<IMessage>();
        }

        internal int Count
        {
            get
            {
                return this.Store.Count;
            }
        }

        private Queue<IMessage> Store { get; set; }

        internal void Enqueue(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.Store.Enqueue(message);
        }

        internal IMessage Dequeue()
        {
            if (this.Store.Count == 0)
            {
                return null;
            }
            
            return this.Store.Dequeue();
        }
    }
}
