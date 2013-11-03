namespace Tests
{
    using System;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using StoreAndForward;
    
    [TestClass]
    public class GivenAHttpClient
    {
        private HttpClient HttpClient { get; set; }
 
        [TestInitialize]
        public void Setup()
        {
            this.HttpClient = new HttpClient();
        }

        [TestMethod]
        public void WhenConstructingShouldNotThrow()
        {
            Assert.IsNotNull(this.HttpClient);
        }

        [TestMethod]
        public void WhenCallingPostWithNullMessageShouldThrow()
        {
            var excep = Assert.ThrowsException<ArgumentNullException>(() => this.HttpClient.Post(null));
            Assert.AreEqual("message", excep.ParamName);
        }

        [TestMethod]
        public void WhenCallingPostForValidUrlShouldReturnOK()
        {
            var message = new Message("application/josn", "{\"name\": \"testing post\"}", new Uri("http://httpbin.org/post"), null);

            var actual = this.HttpClient.Post(message);

            Assert.AreEqual(PostResult.OK, actual);
        }

        [TestMethod]
        public void WhenCallingPostForInvalidValidUrlShouldReturnPermanentError()
        {
            var message = new Message("application/josn", "{\"name\": \"testing post\"}", new Uri("http://httpbin.org/get"), null);

            var actual = this.HttpClient.Post(message);

            Assert.AreEqual(PostResult.PermanentError, actual);
        }
    }
}
