namespace StoreAndForward
{
    using System;

    public sealed class StoreAndForward : IService
    {
        public StoreAndForward()
        {
            this.QueueStore = new QueueStore();
            this.StoreService = new StoreService(this.QueueStore);
            this.ForwardService = new ForwardService();
            this.NetworkMonitorService = new NetworkStateMonitorService();
        }

        internal QueueStore QueueStore { get; set; }

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
            
            this.QueueStore.Enqueue(message);
        }
        
        public void Stop()
        {
            this.EnsureServiceStarted();

            this.NetworkMonitorService.Stop();
            this.ForwardService.Stop();
            this.StoreService.Stop();
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
