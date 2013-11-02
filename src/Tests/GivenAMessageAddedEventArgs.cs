namespace Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;

    [TestClass]
    public class GivenAMessageAddedEventArgs
    {
        [TestInitialize]
        public void Setup()
        {
            var message = new Message("ContentType", "Body", new System.Uri("http://www.test.com"), null);
            this.MessageAddedEventArgs = new MessageAddedEventArgs(message);
        }

        private MessageAddedEventArgs MessageAddedEventArgs { get; set; }

        [TestMethod]
        public void WhenCallingMessageShouldReturnExpected()
        {
            var message = this.MessageAddedEventArgs.Message;

            Assert.AreEqual("ContentType", message.ContentType);
            Assert.AreEqual("Body", message.Body);
            Assert.AreEqual("http://www.test.com/", message.EndPoint.ToString());
            Assert.IsNull(message.Headers);
        }
    }
}
