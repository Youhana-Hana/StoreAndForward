namespace StoreAndForward
{
    using System.Threading;

    internal class ForwardService : IService
    {
        public ForwardService(IStore store, INetworkStateMonitorService networkStateMonitorService)
        {
        //    this.Store = store;
        //    this.NetworkStateMonitorService = networkStateMonitorService;

        //    this.PrepareWaitHandles();
        }

        //internal Thread Thread { get; set; }

        //internal ManualResetEvent[] WaitHandles { get; set; }

        //private IStore Store { get; set; }

        //private INetworkStateMonitorService NetworkStateMonitorService { get; set; }

        public void Start()
        {
            //this.ResetEvents();
            //this.StartThread();

            //this.Store.MessageAdded += Store_MessageAdded;
            //this.NetworkStateMonitorService.NetworkStatusChanged += NetworkStateMonitorService_NetworkStatusChanged;
        }


        public void Stop()
        {
            //this.Store.MessageAdded -= Store_MessageAdded;
            //this.NetworkStateMonitorService.NetworkStatusChanged -= NetworkStateMonitorService_NetworkStatusChanged;
 
            //this.WaitHandles[0].Reset();
            //this.WaitHandles[1].Set();
            //this.Thread.Join();
        }

//        private void NetworkStateMonitorService_NetworkStatusChanged(object sender, NetworkStateEventArgs e)
//        {
//            if (e.ConnectionState != ConnectionState.Connected)
//            {
//                return;
//            }

//            this.StartForwardStoredMessages();
//        }

//        private void Store_MessageAdded(object sender, MessageAddedEventArgs args)
//        {
//            this.StartForwardStoredMessages();
//        }

//        private void StartForwardStoredMessages()
//        {
//            this.WaitHandles[0].Set();
//        }

//        private void PrepareWaitHandles()
//        {
//            this.WaitHandles = new ManualResetEvent[]
//            {
//                new ManualResetEvent(false),
//                new ManualResetEvent(false)
//            };
//        }

//        private void ResetEvents()
//        {
//            this.WaitHandles[0].Reset();
//            this.WaitHandles[1].Reset();
//        }

//        private void StartThread()
//        {
//            this.Thread = new Thread(this.ThreadStart);
//            this.Thread.Start();
//        }

//        private void ThreadStart()
//        {
//////            IMessage message = null;

//            while (true)
//            {
//                int waitExit = WaitHandle.WaitAny(WaitHandles);

//                if (waitExit == 1)
//                {
//                    break;
//                }


//            //    while ((message = this.QueueStore.Peek()) != null)
//            //    {
//            //        this.Store.Add(message);
//            //        this.QueueStore.Dequeue();
//            //    }
//            }
//        }
    }
}
