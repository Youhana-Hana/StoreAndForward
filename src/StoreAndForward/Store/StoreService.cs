namespace StoreAndForward
{
    using System.Threading;

    public class StoreService: IService
    {
        public StoreService(QueueStore queueStore)
        {
            this.WaitHandles = new ManualResetEvent[]
            {
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };

            this.QueueStore = queueStore;
        }

        internal IStore Store { get; set; }

        internal QueueStore QueueStore { get; set; }

        internal Thread Thread { get; set; }

        internal ManualResetEvent[] WaitHandles { get; set; }

        public void Start()
        {
            this.Store = new Store();
            this.ResetEvents();
            this.StartThread();
            this.QueueStore.MessageAdded += this.OnMessageAdded;
        }

        public void Stop()
        {
            this.QueueStore.MessageAdded -= this.OnMessageAdded;
            this.WaitHandles[0].Reset();
            this.WaitHandles[1].Set();
            this.Thread.Join();
        }

        public void OnMessageAdded(object sender, MessageAddedEventArgs args)
        {
            this.WaitHandles[0].Set();
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
            IMessage message = null;

            while(true)
            {
                int waitExit = WaitHandle.WaitAny(WaitHandles);

                if (waitExit == 1)
                {
                    break;
                }

                while ((message = this.QueueStore.Peek()) != null)
                {
                    this.Store.Add(message);

                    this.QueueStore.Dequeue();
               }
            }
        }
    }
}
