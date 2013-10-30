namespace Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;

    [TestClass]
    public class GivenANetworkStateEventArgs
    {
        [TestMethod]
        public void WhenCallingConnectionStateWithConnectedShouldReturnExpected()
        {
            var networkStateEventArgs = new NetworkStateEventArgs(ConnectionState.Connected);

            Assert.AreEqual(ConnectionState.Connected, networkStateEventArgs.ConnectionState);
        }

        [TestMethod]
        public void WhenCallingConnectionStateWithDisConnectedShouldReturnExpected()
        {
            var networkStateEventArgs = new NetworkStateEventArgs(ConnectionState.DisConnected);

            Assert.AreEqual(ConnectionState.DisConnected, networkStateEventArgs.ConnectionState);
        }
    }
}
