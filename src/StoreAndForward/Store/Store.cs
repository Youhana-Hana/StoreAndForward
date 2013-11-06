namespace StoreAndForward
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Net;
using System.Threading;

    internal class Store : IStore
    {
        public Store()
        {
            this.OperationEvent = new AutoResetEvent(true);

            this.DataContext = new MessageEntriesDataContext("Data Source=isostore:/StoreAndForward.sdf");
            
            if (DataContext.DatabaseExists())
            {
                return;
            }

            this.DataContext.CreateDatabase();
        }

        public event EventHandler<MessageAddedEventArgs> MessageAdded;

        public int Count
        {
            get
            {
                try
                {
                    this.OperationEvent.WaitOne();

                    return this.DataContext.Messages.Count();
                }
                finally
                {
                    this.OperationEvent.Set();
                }
            }
        }

        internal MessageEntriesDataContext DataContext { get; set; }

        private AutoResetEvent OperationEvent { get; set; }

        public IMessage Add(IMessage message)
        {
            try
            {
                this.OperationEvent.WaitOne();

                var entry = this.GetMessageEntry(message);
                this.AddHeaders(entry, message.Headers);

                this.DataContext.Messages.InsertOnSubmit(entry);
                this.DataContext.SubmitChanges();
                var addedMessage = new Message(message.ContentType, message.Body, message.EndPoint, message.Headers, entry.MessageId);
                this.BroadcastNewMessagAdded(addedMessage);
                return addedMessage;
            }
            finally
            {
                this.OperationEvent.Set();
            }
        }

        public void Remove(IMessage message)
        {
            try
            {
                this.OperationEvent.WaitOne();

                var messages = from item in this.DataContext.Messages
                               where item.MessageId == message.Id
                               select item;

                var entry = messages.SingleOrDefault();
                if (entry == null)
                {
                    return;
                }

                this.DataContext.Messages.DeleteAllOnSubmit(messages);
                this.DataContext.SubmitChanges();
            }
            finally
            {
                this.OperationEvent.Set();
            }
        }

        public IList<IMessage> Get()
        {
            try
            {
                this.OperationEvent.WaitOne();

                var items = new List<IMessage>();

                var entries = from item in this.DataContext.Messages
                              orderby item.MessageId ascending
                              select item;

                var messages = entries.ToList();

                foreach (var entry in messages)
                {
                    var message = this.GetMessageFromEntry(entry);
                    items.Add(message);
                }

                return items;
            }
            finally
            {
                this.OperationEvent.Set();
            }
        }

        internal void Delete()
        {
            try
            {
                this.OperationEvent.WaitOne();
                this.DataContext.DeleteDatabase();
            }
            finally
            {
                this.OperationEvent.Set();
            }
        }

        private void AddHeaders(MessageEntry messageEntry, WebHeaderCollection headers)
        {
            if (headers == null)
            {
                return;
            }

            foreach (var key in headers.AllKeys)
            {
                var header = this.AddHeader(key, headers[key], messageEntry);

                messageEntry.Headers.Add(header);
            }
        }

        private HeaderEntry AddHeader(string key, string value,  MessageEntry messageEntry)
        {
            return new HeaderEntry()
            {
                Key = key,
                Value = value,
                Message = messageEntry
            };
        }
        
        private MessageEntry GetMessageEntry(IMessage message)
        {
            return new MessageEntry()
            {
                Body = message.Body,
                Url = message.EndPoint.ToString(),
                ContentType = message.ContentType,
                Host = message.EndPoint.Host,
                MessageId = message.Id
            };
        }

        private IMessage GetMessageFromEntry(MessageEntry entry)
        {
            var headers = new WebHeaderCollection();
            var storedHeaders = entry.Headers;

            foreach (var header in storedHeaders)
            {
                headers[header.Key] = header.Value;
            }

            return new Message(entry.ContentType, entry.Body, new Uri(entry.Url), headers, entry.MessageId);
        }

        private void BroadcastNewMessagAdded(IMessage message)
        {
            var messageHandler = this.MessageAdded;

            if (messageHandler == null)
            {
                return;
            }

            messageHandler.Invoke(this, new MessageAddedEventArgs(message));
        }

        public void Dispose()
        {
            this.DataContext.Dispose();
        }
    }
}
