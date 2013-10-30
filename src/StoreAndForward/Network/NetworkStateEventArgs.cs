namespace StoreAndForward
{
    using System;

    public sealed class NetworkStateEventArgs : EventArgs
    {
        public NetworkStateEventArgs(ConnectionState connectionState)
        {
            this.ConnectionState = connectionState;
        }

        public ConnectionState ConnectionState { get; set; }
    }
}
