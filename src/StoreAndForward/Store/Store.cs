namespace StoreAndForward
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Net;

    internal class Store : IStore
    {
        public Store()
        {
            DataLoadOptions loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<MessageEntry>(m => m.Headers);
            this.DataContext = new MessageEntriesDataContext("Data Source=isostore:/StoreAndForward.sdf");
            this.DataContext.LoadOptions = loadOptions;
            
            if (DataContext.DatabaseExists())
            {
                return;
            }

            this.DataContext.CreateDatabase();
        }

        public int Count
        {
            get
            {
                return this.DataContext.Messages.Count();
            }
        }

        internal MessageEntriesDataContext DataContext { get; set; }
 
        public IMessage Add(IMessage message)
        {
            var entry = this.GetMessageEntry(message);
            this.AddHeaders(entry, message.Headers);

            this.DataContext.Messages.InsertOnSubmit(entry);
            this.DataContext.SubmitChanges();
            return new Message(message.ContentType, message.Body, message.EndPoint, message.Headers, entry.MessageId);
        }

        public void Remove(IMessage message)
        {
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

        public IList<IMessage> Get()
        {
            var items = new List<IMessage>();

            var entries = from item in this.DataContext.Messages
                          orderby item.MessageId ascending
                         select item;

            foreach (var entry in entries)
            {
                var message = this.GetMessageFroEntry(entry);
                items.Add(message);
            }

            return items;
        }

        internal void Delete()
        {
            this.DataContext.DeleteDatabase();
        }
        
        private void AddHeaders(MessageEntry messageEntry, WebHeaderCollection headers)
        {
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

        private IMessage GetMessageFroEntry(MessageEntry entry)
        {
            var headers = new WebHeaderCollection();
            var storedHeaders = entry.Headers;

            foreach (var header in storedHeaders)
            {
                headers[header.Key] = header.Value;
            }

            return new Message(entry.ContentType, entry.Body, new Uri(entry.Url), headers);
        }
    }
}
