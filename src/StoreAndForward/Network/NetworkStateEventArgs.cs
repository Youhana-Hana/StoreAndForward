namespace StoreAndForward
{
    using System;

    internal class NetworkStateEventArgs : EventArgs
    {
        public NetworkStateEventArgs(ConnectionState connectionState)
        {
            this.ConnectionState = connectionState;
        }

        public ConnectionState ConnectionState { get; set; }
    }
}
