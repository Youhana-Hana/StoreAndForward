namespace StoreAndForward
{
    using System.Data.Linq;
    using System.Linq;
    using System.Net;

    internal class Store
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

        internal MessageEntriesDataContext DataContext { get; set; }
 
        public int Count
        {
            get 
            { 
                return this.DataContext.Messages.Count();
            }
        }

        public IMessage Add(IMessage message)
        {
            var entry = this.GetMessageEntry(message);
            this.AddHeaders(entry, message.Headers);

            this.DataContext.Messages.InsertOnSubmit(entry);
            this.DataContext.SubmitChanges();
            return new Message(message.ContentType, message.Body, message.EndPoint, message.Headers, entry.MessageId);
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
                Host = message.EndPoint.Host
            };
        }
    }
}
