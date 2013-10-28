namespace StoreAndForward
{
    using System;
    using System.Net;

    public class Message : IMessage
    {
        public Message(string contentType, string body, Uri endPoint, WebHeaderCollection headers)
        {
            ValidateParams(contentType, body);
            this.ContentType = contentType;
            this.Body = body;
            this.EndPoint = endPoint;
            this.Headers = headers;
        }

        internal Message(string contentType, string body, Uri endPoint, WebHeaderCollection headers, int id)
            : this(contentType, body, endPoint, headers)
        {
            this.Id = id;
        }

        private static void ValidateParams(string contentType, string body)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException("contentType");
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentException("Invalid param", "contentType");
            }

            if (body == null)
            {
                throw new ArgumentNullException("body");
            }
        }
        
        public int Id { get; private set; }
        
        public string ContentType { get; private set; }

        public string Body { get; private set; }

        public Uri EndPoint { get; private set; }

        public WebHeaderCollection Headers { get; private set; }
    }
}
