﻿namespace Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using MoqaLate.Autogenerated;
    using StoreAndForward;
    using System.Threading;

    [TestClass]
    public class GivenAStoreService
    {
        private QueueStore QueueStore { get; set; }

        private StoreService StoreService { get; set; }

        private StoreMoqaLate Store { get; set; }

        private Store RealStore { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.QueueStore = new QueueStore();
            this.Store = new StoreMoqaLate();
            this.RealStore = null;

            this.StoreService = new StoreService(this.QueueStore);
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            Assert.IsNotNull(this.StoreService);
        }

        [TestMethod]
        public void WhenConstructingShouldInitializeStore()
        {
            this.StoreService = new StoreService(this.QueueStore);

            Assert.IsNull(this.StoreService.Store);
            Assert.IsNotNull(this.StoreService.QueueStore);
            Assert.IsNotNull(this.StoreService.WaitHandles);
            Assert.AreEqual(2, this.StoreService.WaitHandles.Length);
        }

        [TestMethod]
        public void WhenCallingStartShouldAllocateStore()
        {
            this.StoreService.Start();
            Thread.Sleep(200);
            this.StoreService.Stop();

            Assert.IsNotNull(this.StoreService.Store);
        }

        [TestMethod]
        public void WhenCallingStartShouldResetEvents()
        {
            this.StoreService.Start();
            Thread.Sleep(200);
            var wait0 = this.StoreService.WaitHandles[0].WaitOne(0);
            var wait1 = this.StoreService.WaitHandles[1].WaitOne(0);
            this.StoreService.Stop();

            Assert.IsFalse(wait0);
            Assert.IsFalse(wait1);
        }

        [TestMethod]
        public void WhenCallingStartShouldStartThread()
        {
            this.StoreService.Start();
            Thread.Sleep(200);
            this.StoreService.Stop();

            Assert.IsNotNull(this.StoreService.Thread);
        }

        [TestMethod]
        public void WhenCallingStopShouldResetEvents()
        {
            this.StoreService.Start();
            this.StoreService.Store = this.Store;
            this.StoreService.QueueStore.Enqueue(new MessageMoqaLate());
            Thread.Sleep(200);
            this.StoreService.Stop();

            Assert.IsFalse(this.StoreService.WaitHandles[0].WaitOne(0));
            Assert.IsTrue(this.StoreService.WaitHandles[1].WaitOne(0));
        }

        [TestMethod]
        public void WhenCallingStopShouldUnregisterFromMessagesEvents()
        {
            this.StoreService.Start();
            this.StoreService.Store = this.Store;
            this.StoreService.QueueStore.Enqueue(new MessageMoqaLate());
            Thread.Sleep(200);
            this.StoreService.Stop();
            this.StoreService.QueueStore.Enqueue(new MessageMoqaLate());

            Assert.IsFalse(this.StoreService.WaitHandles[0].WaitOne(0));
            Assert.IsTrue(this.StoreService.WaitHandles[1].WaitOne(0));
            Assert.AreEqual(1, this.Store.AddTimesCalled());
            Assert.AreEqual(1, this.StoreService.QueueStore.Count);
        }

        [TestMethod]
        public void WhenCallingStartStopStartShouldResetEvents()
        {
            this.StoreService.Start();
            this.StoreService.Stop();
            this.StoreService.Start();
           
            Assert.IsFalse(this.StoreService.WaitHandles[0].WaitOne(0));
            Assert.IsFalse(this.StoreService.WaitHandles[1].WaitOne(0));
        }

        [TestMethod]
        public void WhenCallingStartAndMessageAddedShouldStoreIt()
        {
            this.StoreService.Start();
            this.StoreService.Store = this.Store;
            this.StoreService.QueueStore.Enqueue(new MessageMoqaLate());
            Thread.Sleep(200);
            this.StoreService.Stop();

            Assert.AreEqual(1, this.Store.AddTimesCalled());
            Assert.AreEqual(0, this.StoreService.QueueStore.Count);
         }
    }
}
