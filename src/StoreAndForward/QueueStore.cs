namespace StoreAndForward
{
    using System;
    using System.Collections.Generic;

    public class QueueStore
    {
        public delegate void NewMessageAdded(IMessage message);

        public event NewMessageAdded MessageAdded;

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

            this.BroadcastNewMessagReceived(message);
        }

        internal IMessage Dequeue()
        {
            if (this.Store.Count == 0)
            {
                return null;
            }
            
            return this.Store.Dequeue();
        }

        internal IMessage Peek()
        {
            if (this.Store.Count == 0)
            {
                return null;
            }

            return this.Store.Peek();
        }

        private void BroadcastNewMessagReceived(IMessage message)
        {
            var messageHandler = this.MessageAdded;
         
            if (messageHandler == null)
            {
                return;
            }

            messageHandler.Invoke(message);
        }
    }
}
