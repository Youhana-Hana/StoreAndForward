namespace StoreAndForward
{
    using System.Data.Linq;

    internal class MessageEntriesDataContext : DataContext
    {
        public Table<MessageEntry> Messages;
        
        public Table<HeaderEntry> Headers;

        public MessageEntriesDataContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
