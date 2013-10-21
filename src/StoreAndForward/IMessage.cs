namespace StoreAndForward
{
    using System;
    using System.Net;

    public interface IMessage
    {
        int Id { get; }

        string ContentType { get; }

        string Body { get; }

        Uri EndPoint { get; }

        WebHeaderCollection Headers { get; } 
    }
}
