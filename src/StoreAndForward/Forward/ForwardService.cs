namespace StoreAndForward
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    internal class ForwardService : IService
    {
        public ForwardService(IStore store, INetworkStateMonitorService networkStateMonitorService, IHttpClient httpClient)
        {
            this.Store = store;
            this.NetworkStateMonitorService = networkStateMonitorService;
            this.HttpClient = httpClient;

            this.PrepareWaitHandles();
        }

        internal Thread Thread { get; set; }

        internal ManualResetEvent[] WaitHandles { get; set; }

        internal IStore Store { get; set; }

        internal INetworkStateMonitorService NetworkStateMonitorService { get; set; }

        internal IHttpClient HttpClient { get; set; }

        public void Start()
        {
            this.ResetEvents();
            this.StartThread();

            this.Store.MessageAdded += Store_MessageAdded;
            this.NetworkStateMonitorService.NetworkStatusChanged += NetworkStateMonitorService_NetworkStatusChanged;

            this.StartForwardStoredMessages();
        }

        public void Stop()
        {
            this.Store.MessageAdded -= Store_MessageAdded;
            this.NetworkStateMonitorService.NetworkStatusChanged -= NetworkStateMonitorService_NetworkStatusChanged;
 
            this.WaitHandles[0].Reset();
            this.WaitHandles[1].Set();
            this.Thread.Join();
        }

        private void NetworkStateMonitorService_NetworkStatusChanged(object sender, NetworkStateEventArgs e)
        {
            if (e.ConnectionState != ConnectionState.Connected)
            {
                return;
            }

            this.StartForwardStoredMessages();
        }

        private void Store_MessageAdded(object sender, MessageAddedEventArgs args)
        {
            this.StartForwardStoredMessages();
        }

        private void StartForwardStoredMessages()
        {
            this.WaitHandles[0].Set();
        }

        private void PauseForwardStoredMessages()
        {
            this.WaitHandles[0].Reset();
        }

        private void PrepareWaitHandles()
        {
            this.WaitHandles = new ManualResetEvent[]
            {
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };
        }

        private void ResetEvents()
        {
            this.WaitHandles[0].Reset();
            this.WaitHandles[1].Reset();
        }

        private void StartThread()
        {
            this.Thread = new Thread(this.ThreadStart);
            this.Thread.Start();
        }

        private void ThreadStart()
        {
            while (true)
            {
                int waitExit = WaitHandle.WaitAny(WaitHandles);

                if (waitExit == 1)
                {
                    break;
                }

                var messages = this.Store.Get();
                
                foreach (var message in messages)
                {
                    var postResult = this.HttpClient.Post(message);
                       
                    if (postResult != PostResult.TemporaryError)
                    {
                        this.Store.Remove(message);
                    }
                }
                
                this.PauseForwardStoredMessages();
            }
        }
    }
}
