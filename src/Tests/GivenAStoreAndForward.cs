﻿namespace Tests
{
    using System;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;
    using MoqaLate.Autogenerated;

    [TestClass]
    public class GivenAStoreAndForward
    {
        private StoreAndForward StoreAndForward { get; set; }

        private ServiceMoqaLate StoreService { get; set; }

        private ServiceMoqaLate ForwardService { get; set; }

        private ServiceMoqaLate NewtorkService { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.StoreAndForward = new StoreAndForward();
            this.StoreService = new ServiceMoqaLate();
            this.ForwardService = new ServiceMoqaLate();
            this.NewtorkService = new ServiceMoqaLate();

            this.StoreAndForward.StoreService = this.StoreService;
            this.StoreAndForward.ForwardService = this.ForwardService;
            this.StoreAndForward.NetworkMonitorService = this.NewtorkService;
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            Assert.IsNotNull(this.StoreAndForward);
        }

        [TestMethod]
        public void WhenConstructingShouldAllocateDependentServices()
        {
            this.StoreAndForward = new StoreAndForward();

            Assert.IsNotNull(this.StoreAndForward.QueueStore);
            Assert.IsNotNull(this.StoreAndForward.StoreService);
            Assert.IsNotNull(this.StoreAndForward.ForwardService);
            Assert.IsNotNull(this.StoreAndForward.NetworkMonitorService);
        }

        [TestMethod]
        public void WhenCallingStoreAndServiceNotStartedShouldThrow()
        {
            var message = new MessageMoqaLate();
            var exception = Assert.ThrowsException<InvalidOperationException>(()=> this.StoreAndForward.Store(message));
            Assert.AreEqual("Service not started.", exception.Message);
        }

        [TestMethod]
        public void WhenCallingStopAndServiceNotStartedShouldThrow()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(() => this.StoreAndForward.Stop());
            Assert.AreEqual("Service not started.", exception.Message);
        }

        [TestMethod]
        public void WhenCallingStoreShouldAddMessageToQueue()
        {
            this.StoreAndForward.Start();

            var countBeforeAdd = this.StoreAndForward.QueueStore.Count;
            var message = new MessageMoqaLate();
            this.StoreAndForward.Store(message);
            var countAfterAdd = this.StoreAndForward.QueueStore.Count;

            Assert.AreEqual(0, countBeforeAdd);
            Assert.AreEqual(1, countAfterAdd);
        }

        [TestMethod]
        public void WhenCallingStartShouldStartDependentServices()
        {
            this.StoreAndForward.Start();

            Assert.IsTrue(this.StoreService.StartWasCalled());
            Assert.IsTrue(this.ForwardService.StartWasCalled());
            Assert.IsTrue(this.NewtorkService.StartWasCalled());
            Assert.AreEqual(1, this.StoreService.StartTimesCalled());
            Assert.AreEqual(1, this.ForwardService.StartTimesCalled());
            Assert.AreEqual(1, this.NewtorkService.StartTimesCalled());
        }

        [TestMethod]
        public void WhenCallingStopShouldStoptDependentServices()
        {
            this.StoreAndForward.Start();
            this.StoreAndForward.Stop();

            Assert.IsTrue(this.StoreService.StopWasCalled());
            Assert.IsTrue(this.ForwardService.StopWasCalled());
            Assert.IsTrue(this.NewtorkService.StopWasCalled());
            Assert.AreEqual(1, this.StoreService.StopTimesCalled());
            Assert.AreEqual(1, this.ForwardService.StopTimesCalled());
            Assert.AreEqual(1, this.NewtorkService.StopTimesCalled());
        }
    }
}
