namespace Tests
{
    using System;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;
    using System.Net;

    [TestClass]
    public class GivenAMessage
    {
        private Message Message { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Message = new Message("Content_Type", "Body", new Uri("http://www.test.com"), null);
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            Assert.IsNotNull(this.Message);
        }

        [TestMethod]
        public void WhenConstructingWithNullContentTypeShouldThrow()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(()=> new Message(null, "Body", new Uri("http://www.test.com"), null));
            Assert.AreEqual("contentType", exception.ParamName);
        }

        [TestMethod]
        public void WhenConstructingWithEmptyContentTypeShouldThrow()
        {
            var exception = Assert.ThrowsException<ArgumentException>(() => new Message("", "Body", new Uri("http://www.test.com"), null));
            Assert.AreEqual("contentType", exception.ParamName);
        }

        [TestMethod]
        public void WhenConstructingWithSpacesContentTypeShouldThrow()
        {
            var exception = Assert.ThrowsException<ArgumentException>(() => new Message("   ", "Body", new Uri("http://www.test.com"), null));
            Assert.AreEqual("contentType", exception.ParamName);
        }

        [TestMethod]
        public void WhenConstructingWithNullBodyShouldThrow()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => new Message("Content_Type", null, new Uri("http://www.test.com"), null));
            Assert.AreEqual("body", exception.ParamName);
        }

        [TestMethod]
        public void GettingMessagePropertiesShouldReturnExpected()
        {
            var headers = new WebHeaderCollection();
            headers["H"] = "V";
            var endPoint = new Uri("http://www.test.com");

            this.Message = new Message("Content_Type", "Body", endPoint, headers);

            Assert.AreEqual("Content_Type", this.Message.ContentType);
            Assert.AreEqual("Body", this.Message.Body);
            Assert.AreEqual(endPoint, this.Message.EndPoint.ToString());
            Assert.AreEqual(headers, this.Message.Headers);
            Assert.AreEqual(0, this.Message.Id);
        }
    }
}
