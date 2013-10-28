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
        public void WhenCallingEnqueueWithNullMessageShouldThrow()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => this.Store.Enqueue(null));
            Assert.AreEqual("message", exception.ParamName);
        }

        [TestMethod]
        public void WhenCallingEnqueueShouldAddMessage()
        {
            var message = this.GetMessage("http://www.test.com");
            this.Store.Enqueue(message);

            Assert.AreEqual(1, this.Store.Count);
        }

        [TestMethod]
        public void WhenCallingDequeueAndStoreIsEmptyShouldReturnNull()
        {
            var actual = this.Store.Dequeue();

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WhenCallingEnqueueThenDequeueShouldRemoveCorrectMessage()
        {
            var message1 = this.GetMessage("http://www.test1.com");
            var message2 = this.GetMessage("http://www.test2.com");
            var message3 = this.GetMessage("http://www.test3.com");

            this.Store.Enqueue(message1);
            this.Store.Enqueue(message2);
            this.Store.Enqueue(message3);

            Assert.AreEqual(3, Store.Count);
            Assert.AreEqual(message1, this.Store.Dequeue());
            Assert.AreEqual(message2, this.Store.Dequeue());
            Assert.AreEqual(message3, this.Store.Dequeue());
      
            Assert.AreEqual(0, this.Store.Count);
        }

        [TestMethod]
        public void WhenCallingPeekAndStoreIsEmptyShouldReturnNull()
        {
            var actual = this.Store.Peek();

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WhenCallingPeekThenDequeueShouldRemoveCorrectMessage()
        {
            var message1 = this.GetMessage("http://www.test1.com");
            var message2 = this.GetMessage("http://www.test2.com");
            var message3 = this.GetMessage("http://www.test3.com");

            this.Store.Enqueue(message1);
            this.Store.Enqueue(message2);
            this.Store.Enqueue(message3);

            Assert.AreEqual(3, Store.Count);
            Assert.AreEqual(message1, this.Store.Peek());
            Assert.AreEqual(message1, this.Store.Peek());
         }

        private IMessage GetMessage(string endpoint)
        {
            return new Message("Content-Type", "", new Uri(endpoint), null);
        }
    }
}
