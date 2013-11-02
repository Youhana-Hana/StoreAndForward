namespace StoreAndForward
{
    using System;

    public class MessageAddedEventArgs : EventArgs
    {
        public MessageAddedEventArgs(IMessage message)
        {
            this.Message = message;
        }

        public IMessage Message { get; set; }
    }
}
