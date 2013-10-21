namespace Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;
    using System;

    [TestClass]
    public class GivenAQueueStore
    {
        private QueueStore Store { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Store = new QueueStore();
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            Assert.IsNotNull(this.Store);
        }

        [TestMethod]
        public void WhenCallingAddWithNullMessageShouldThrow()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => this.Store.Add(null));
            Assert.AreEqual("message", exception.ParamName);
        }

    }
}
