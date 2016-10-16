using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemon.Data.Core;

namespace Lemon.Transform.Tests
{
    [TestClass]
    public class EqualityComparerUnitTest
    {
        public class Message
        {
            public string Id { get; set; }

            public DateTime CreatedOn { get; set; }
        }


        [TestMethod]
        public void EqualTest()
        {
            EqualityComparer<Message> comparer = new EqualityComparer<Message>(new string[] { "Id", "CreatedOn" });

            var msg1 = new Message { Id = "1", CreatedOn = DateTime.Parse("2106-9-1") };
            var msg2 = new Message { Id = "1", CreatedOn = DateTime.Parse("2106-9-1") };

            Assert.AreEqual(true, comparer.Equals(msg1, msg2));
        }

        [TestMethod]
        public void NotEqualTest()
        {
            EqualityComparer<Message> comparer = new EqualityComparer<Message>(new string[] { "Id", "CreatedOn" });

            var msg1 = new Message { Id = "1", CreatedOn = DateTime.Parse("2106-9-1") };
            var msg2 = new Message { Id = "1", CreatedOn = DateTime.Parse("2106-9-2") };

            Assert.AreEqual(false, comparer.Equals(msg1, msg2));
        }
    }
}
