namespace Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;
    using System;
    using System.Net;
    using System.Linq;

    [TestClass]
    public class GivenAStore
    {
        private Store Store { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Store = new Store();
        }

        [TestCleanup]
        public void TearDown()
        {
            this.Store.Delete();
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrough()
        {
            Assert.IsNotNull(this.Store);
        }

        [TestMethod]
        public void WhenConstructingShouldAllocateDataContext()
        {
            Assert.IsNotNull(this.Store.DataContext);
        }

        [TestMethod]
        public void WhenCallingAddShouldInsertRecord()
        {
            var headers = new WebHeaderCollection();
            headers["K1"] = "V1";
            headers["K2"] = "V2";

            var endPoint = new Uri("http://www.google.com/1/api");
            var message = new Message("application/json", "Body", endPoint, headers);
            
            var resultMessage = this.Store.Add(message);
            var count = this.Store.Count;
          
            var messages = from m in this.Store.DataContext.Messages
                           where m.Url == "http://www.google.com/1/api" 
                           select m;

            var storedMessage = messages.FirstOrDefault();

            Assert.AreEqual(1, count);
            Assert.AreEqual(message.Body, storedMessage.Body);
            Assert.AreEqual(message.ContentType, storedMessage.ContentType);
            Assert.AreEqual(message.EndPoint.ToString(), storedMessage.Url);
            Assert.AreEqual(message.EndPoint.Host, storedMessage.Host);
            Assert.AreEqual(1, resultMessage.Id);
        }

        [TestMethod]
        public void WhenCallingAddWithHeadersShouldInsertRecordWithHeaders()
        {
            var headers = new WebHeaderCollection();
            headers["K1"] = "V1";
            headers["K2"] = "V2";

            var endPoint = new Uri("http://www.google.com/1/api");
            var message = new Message("application/json", "Body", endPoint, headers);

            var resultMessage = this.Store.Add(message);
            var count = this.Store.Count;

            var messages = from m in this.Store.DataContext.Messages
                           where m.Url == "http://www.google.com/1/api"
                           select m;

            var storedMessage = messages.FirstOrDefault();

            Assert.AreEqual(1, count);
            Assert.AreEqual(message.Body, storedMessage.Body);
            Assert.AreEqual(message.ContentType, storedMessage.ContentType);
            Assert.AreEqual(message.EndPoint.ToString(), storedMessage.Url);
            Assert.AreEqual(message.EndPoint.Host, storedMessage.Host);
            Assert.AreEqual(1, resultMessage.Id);
            Assert.AreEqual(2, storedMessage.Headers.Count);
            Assert.AreEqual("K1", storedMessage.Headers.First().Key);
            Assert.AreEqual("V1", storedMessage.Headers.First().Value);
            Assert.AreEqual("K2", storedMessage.Headers.Last().Key);
            Assert.AreEqual("V2", storedMessage.Headers.Last().Value);
        }
    }
}
