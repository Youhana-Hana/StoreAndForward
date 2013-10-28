using System.Threading;
namespace StoreAndForward
{
    public class StoreService: IService, INewMessageHandler
    {
        public StoreService(QueueStore queueStore)
        {
           this.WaitHandles = new WaitHandle[]
            {
                new ManualResetEvent(false),
                new ManualResetEvent(false)
            };

            this.QueueStore = queueStore;
        }

        internal IStore Store { get; set; }

        internal QueueStore QueueStore { get; set; }

        internal Thread Thread { get; set; }

        internal WaitHandle[] WaitHandles { get; set; }

        public void Start()
        {
            //this.Store = new Store();
            //this.ExitEvent.Reset();
            //this.Thread = new Thread(this.ThreadStart);
            //this.Thread.Start();
            //this.QueueStore.MessageAdded += this.OnMessageAdded;
        }

        public void Stop()
        {
            //this.QueueStore.MessageAdded -= this.OnMessageAdded;
            //this.ExitEvent.Set();
            //this.Thread.Join();
        }

        public void OnMessageAdded(IMessage message)
        {
            //this.ResetEvent.Set();
        }

        private void ThreadStart()
        {
            //IMessage message = null;

            //while(true)
            //{
            //    int waitExit = WaitHandle.WaitAny(WaitHandles);

            //    if (waitExit == 1)
            //    {
            //        break;
            //    }

            //    while ((message = this.QueueStore.Peek()) != null)
            //    {
            //        this.Store.Add(message);

            //        this.QueueStore.Dequeue();
            //    }
            //}
        }
    }
}
