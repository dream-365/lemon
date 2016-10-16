using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemon.Data.Core;
using System.Linq;

namespace Lemon.Transform.Tests
{
    /// <summary>
    /// Summary description for ComareExecutorTest
    /// </summary>
    [TestClass]
    public class ComareExecutorTest
    {
        public class Message
        {
            public string Id { get; set; }

            public int Value { get; set; }
        }


        public class Observer : ICompareObserver<Message>
        {
            public List<Message> AddMessages { get; set; }

            public IList<Message> DeleteMessages { get; set; }

            public IList<Message> ChangedMessages { get; set; }

            public Observer()
            {
                AddMessages = new List<Message>();

                DeleteMessages = new List<Message>();

                ChangedMessages = new List<Message>();
            }

            public void OnAdd(Message message)
            {
                AddMessages.Add(message);
            }

            public void OnChange(Message previous, Message current)
            {
                ChangedMessages.Add(previous);
                ChangedMessages.Add(current);
            }

            public void OnDelete(Message message)
            {
                DeleteMessages.Add(message);
            }
        }

        public ComareExecutorTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CompareTest()
        {
            var list1 = new List<Message> {
                new Message { Id = "1", Value = 1 },
                new Message { Id = "3", Value = 1 }
            };

            var list2 = new List<Message> {
                new Message { Id = "1", Value = 3 },
                new Message { Id = "2", Value = 2 }
            };

            var ds1 = new DataSet<Message>(list1);
            var ds2 = new DataSet<Message>(list2);

            var exe = new ComareExecutor<Message>(ds1, ds2, new CompareOptions {
                PrimaryKey = "Id",
                ColumnsToCompare = new string[] { "Value" } });

            var observer = new Observer();

            exe.Observer = observer;

            exe.RunAsync().Wait();

            Assert.AreEqual("2", observer.AddMessages.First().Id);
            Assert.AreEqual("3", observer.DeleteMessages.First().Id);
            Assert.AreEqual("1", observer.ChangedMessages.First().Id);
        }
    }
}
