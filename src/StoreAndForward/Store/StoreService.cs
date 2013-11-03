namespace StoreAndForward
{
    using System;
    using System.Threading;

    public class StoreService: IService
    {
        public StoreService(QueueStore queueStore, IStore store)
        {
            this.QueueStore = queueStore;
            this.Store = store;

           this.PrepareWaitHandles();
        }

        internal IStore Store { get; set; }

        internal QueueStore QueueStore { get; set; }

        internal Thread Thread { get; set; }

        internal ManualResetEvent[] WaitHandles { get; set; }

        public void Start()
        {
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

        private void PrepareWaitHandles()
        {
            this.WaitHandles = new ManualResetEvent[]
            {
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };
        }

        private void OnMessageAdded(object sender, MessageAddedEventArgs args)
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
