namespace StoreAndForward
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    [Table(Name = "Headers")]
    internal class HeaderEntry
    {
        private EntityRef<MessageEntry> message;

        [Column(DbType="INT NOT NULL Identity", CanBeNull = false, IsDbGenerated = true, IsPrimaryKey = true)]
        public int HeaderId { get; set; }

        [Column(CanBeNull = false)]
        public string Key { get; set; }

        [Column(CanBeNull=false)]
        public string Value { get; set; }

        [Column(DbType = "INT", CanBeNull = false)]
        public int MessageId { get; set; }

        [Association(Storage = "message", IsForeignKey = true, ThisKey = "MessageId")]
        public MessageEntry Message
        {
            get
            {
                return message.Entity;
            }
            set
            {
                message.Entity = value;
                MessageId = value.MessageId;
            }
        }
    }
}
