namespace Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;
    using System.Threading;

    [TestClass]
    public class GivenAnNetworkStateMonitorService
    {
        private NetworkStateMonitorService NetworkStateMonitorService { get; set; }

        private bool Called { get; set; }

        private ConnectionState Connection { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.NetworkStateMonitorService = new NetworkStateMonitorService();
            this.Called = false;
            this.Connection = ConnectionState.Unknown;
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            Assert.IsNotNull(this.NetworkStateMonitorService);
        }

        [TestMethod]
        public void WhenCallingStartShouldSubscribeInDeviceDeviceNetworkInformation()
        {
            this.NetworkStateMonitorService.NetworkStatusChanged += NetworkStateMonitorService_NetworkStatusChanged;
            this.NetworkStateMonitorService.Start();
            Thread.Sleep(100);
            this.NetworkStateMonitorService.Stop();

            Assert.IsTrue(this.Called);
            Assert.AreEqual(ConnectionState.Connected, this.Connection);
        }

        [TestMethod]
        public void WhenCallingStartAndNoSubscribersShouldNotThrow()
        {
            this.NetworkStateMonitorService.Start();
            Thread.Sleep(100);
            this.NetworkStateMonitorService.Stop();

            Assert.IsFalse(this.Called);
            Assert.AreEqual(ConnectionState.Unknown, this.Connection);
        }
        void NetworkStateMonitorService_NetworkStatusChanged(object sender, NetworkStateEventArgs e)
        {
            this.Called = true;
            this.Connection = e.ConnectionState; 
        }
    }
}
