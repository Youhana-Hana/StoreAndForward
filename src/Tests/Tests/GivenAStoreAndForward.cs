namespace Tests
{
    using System;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;

    [TestClass]
    public class GivenAStoreAndForward
    {
        private StoreAndForward StoreAndForward { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.StoreAndForward = new StoreAndForward();
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            Assert.IsNotNull(this.StoreAndForward);
        }
    }
}
