namespace StoreAndForward
{
    using System;

    public sealed class StoreAndForward : IService
    {
        public StoreAndForward()
        {
            this.VolatileStore = new QueueStore();
            this.PersistStore = new Store();
            this.HttpClient = new HttpClient();
            this.StoreService = new StoreService(this.VolatileStore, this.PersistStore);
            this.NetworkMonitorService = new NetworkStateMonitorService();
            this.ForwardService = new ForwardService(this.PersistStore, this.NetworkMonitorService, this.HttpClient);
        }

        internal IHttpClient HttpClient { get; set; }

        internal QueueStore VolatileStore { get; set; }

        internal IStore PersistStore { get; set; }

        internal IService StoreService { get; set; }

        internal INetworkStateMonitorService NetworkMonitorService { get; set; }

        internal IService ForwardService { get; set; }

        private bool Started { get; set; }

        public void Start()
        {
            this.StoreService.Start();
            this.ForwardService.Start();
            this.NetworkMonitorService.Start();

            this.Started = true;
        }

        public void Store(IMessage message)
        {
            this.EnsureServiceStarted();

            this.VolatileStore.Enqueue(message);
        }
        
        public void Stop()
        {
            this.EnsureServiceStarted();

            this.NetworkMonitorService.Stop();
            this.ForwardService.Stop();
            this.StoreService.Stop();
            this.Started = false;
         }

        private void EnsureServiceStarted()
        {
            if (!this.Started)
            {
                throw new InvalidOperationException("Service not started.");
            }
        }
    }
}
