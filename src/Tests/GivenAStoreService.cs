namespace Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;

    [TestClass]
    public class GivenAStoreService
    {
        [TestInitialize]
        public void Setup()
        {
            this.QueueStore = new QueueStore();
            this.StoreService = new StoreService(this.QueueStore); 
        }

        private QueueStore QueueStore { get; set; }
 
        private StoreService StoreService { get; set; }

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
    }
}
