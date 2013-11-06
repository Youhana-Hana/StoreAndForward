﻿namespace Tests
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

        private IMessage ReceivedMessage { get; set; }

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

        [TestMethod]
        public void WhenCallingAddShouldNotifySubscrivers()
        {
            var headers = new WebHeaderCollection();
            headers["K1"] = "V1";
            headers["K2"] = "V2";

            var endPoint = new Uri("http://www.google.com/1/api");
            var message = new Message("application/json", "Body", endPoint, headers);
            this.Store.MessageAdded += Store_MessageAdded;

            var resultMessage = this.Store.Add(message);

            Assert.AreEqual(message.Body, this.ReceivedMessage.Body);
            Assert.AreEqual(message.ContentType, this.ReceivedMessage.ContentType);
            Assert.AreEqual(message.EndPoint.ToString(), this.ReceivedMessage.EndPoint.ToString());
            Assert.AreEqual(message.EndPoint.Host, this.ReceivedMessage.EndPoint.Host);
            Assert.AreEqual(1, resultMessage.Id);
            Assert.AreEqual(2, ReceivedMessage.Headers.Count);
            Assert.AreEqual("K1", this.ReceivedMessage.Headers.AllKeys.First());
            Assert.AreEqual("V1", this.ReceivedMessage.Headers["k1"]);
            Assert.AreEqual("K2", this.ReceivedMessage.Headers.AllKeys.Last());
            Assert.AreEqual("V2", this.ReceivedMessage.Headers["k2"]);
        }

        [TestMethod]
        public void WhenCallingRemoveShouldRemoveMessageAndHeaders()
        {
            var headers = new WebHeaderCollection();
            headers["K1"] = "V1";
            headers["K2"] = "V2";

            var endPoint = new Uri("http://www.google.com/1/api");
            var message = new Message("application/json", "Body", endPoint, headers);

            var resultMessage = this.Store.Add(message);
            var count = this.Store.Count;
            this.Store.Remove(resultMessage);
            
            var messages = from m in this.Store.DataContext.Messages
                           where m.Url == "http://www.google.com/1/api"
                           select m;

            var storedMessage = messages.FirstOrDefault();

            Assert.AreEqual(1, count);
            Assert.IsNull(storedMessage);
        }

        [TestMethod]
        public void WhenCallingGetShouldReturnMessagesInOrder()
        {
            var headers = new WebHeaderCollection();
            headers["K1"] = "V1";
            headers["K2"] = "V2";

            var message = new Message("application/json", "Body", new Uri("http://www.google.com/1/api"), headers);
            this.Store.Add(message);

            message = new Message("application/json", "Body", new Uri("http://www.google.com/2/api"), headers);
            this.Store.Add(message);

            message = new Message("application/json", "Body", new Uri("http://www.google.com/3/api"), headers);
            this.Store.Add(message);

            message = new Message("application/json", "Body", new Uri("http://www.google.org/1/api"), headers);
            this.Store.Add(message);

            message = new Message("application/json", "Body", new Uri("http://www.google.org/2/api"), headers);
            this.Store.Add(message);

            message = new Message("application/json", "Body", new Uri("http://www.google.org/3/api"), headers);
            this.Store.Add(message);

            var count = this.Store.Count;
            Assert.AreEqual(6, count);

            var messages = this.Store.Get();
            Assert.AreEqual("http://www.google.com/1/api", messages.First().EndPoint.ToString());
            Assert.AreEqual(1, messages.First().Id);
            
            Assert.AreEqual("http://www.google.com/2/api", messages.Skip(1).First().EndPoint.ToString());
            Assert.AreEqual(2, messages.Skip(1).First().Id);
            
            Assert.AreEqual("http://www.google.com/3/api", messages.Skip(2).First().EndPoint.ToString());
            Assert.AreEqual(3, messages.Skip(2).First().Id);

            Assert.AreEqual("http://www.google.org/1/api", messages.Skip(3).First().EndPoint.ToString());
            Assert.AreEqual(4, messages.Skip(3).First().Id);
            
            Assert.AreEqual("http://www.google.org/2/api", messages.Skip(4).First().EndPoint.ToString());
            Assert.AreEqual(5, messages.Skip(4).First().Id);
            
            Assert.AreEqual("http://www.google.org/3/api", messages.Skip(5).First().EndPoint.ToString());
            Assert.AreEqual(6, messages.Skip(5).First().Id);
        }

        void Store_MessageAdded(object sender, MessageAddedEventArgs e)
        {
            this.ReceivedMessage = e.Message;
        }
    }
}
