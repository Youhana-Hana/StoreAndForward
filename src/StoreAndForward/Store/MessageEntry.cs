namespace StoreAndForward
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    [Table(Name = "Messages")]
    internal class MessageEntry
    {
        public MessageEntry()
        {
            this.Headers = new EntitySet<HeaderEntry>(
            delegate(HeaderEntry entity)
            {
                entity.Message = this;
            },
            delegate(HeaderEntry entity)
            {
                entity.Message = null;
            });
        }

        [Column(DbType = "INT NOT NULL Identity", IsDbGenerated = true, IsPrimaryKey = true, CanBeNull = false)]
        public int MessageId { get; internal set; }

        [Column(CanBeNull=false)]
        public string Url { get; set; }

        [Column(CanBeNull = false)]
        public string Host { get; set; }

        [Column(CanBeNull = false)]
        public string ContentType { get; set; }

        [Column(CanBeNull = false)]
        public string Body { get; set; }

        [Association(OtherKey = "MessageId", DeleteRule = "CASCADE")]
        public EntitySet<HeaderEntry> Headers { get; private set; }
    }
}
