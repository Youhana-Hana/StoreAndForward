namespace StoreAndForward
{
    using Microsoft.Phone.Net.NetworkInformation;
    using System;

    internal class NetworkStateMonitorService : INetworkStateMonitorService
    {
        public event EventHandler<NetworkStateEventArgs> NetworkStatusChanged;

        public void Start()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged += NetworkAvailabilityChanged;

            this.UpdateNetworkState();
        }

        public void Stop()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged -= this.NetworkAvailabilityChanged;
        }

        private void UpdateNetworkState()
        {
            if (DeviceNetworkInformation.IsNetworkAvailable)
            {
                this.NotifyNetworkStateChanged(ConnectionState.Connected);

            }
            else
            {
                this.NotifyNetworkStateChanged(ConnectionState.DisConnected);
            }
        }

        private void NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            switch (e.NotificationType)
            {
                case NetworkNotificationType.InterfaceConnected:
                    this.NotifyNetworkStateChanged(ConnectionState.Connected);
                    break;

                case NetworkNotificationType.InterfaceDisconnected:
                    this.NotifyNetworkStateChanged(ConnectionState.DisConnected);
                    break;
            }
        }

        private void NotifyNetworkStateChanged(ConnectionState connectionState)
        {
            var networkStatusChanged = NetworkStatusChanged;

            if (networkStatusChanged == null)
            {
                return;
            }

            networkStatusChanged.Invoke(this, new NetworkStateEventArgs(connectionState));
        }
    }
}