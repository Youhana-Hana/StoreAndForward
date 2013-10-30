namespace StoreAndForward
{
    using System;

    public interface INetworkStateMonitorService
    {
        event EventHandler<NetworkStateEventArgs> NetworkStatusChanged;

        void Start();

        void Stop();
    }
}
