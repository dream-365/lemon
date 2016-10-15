using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemon.Data.Core;

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

            public Observer()
            {
                AddMessages = new List<Message>();

                DeleteMessages = new List<Message>();
            }

            public void OnAdd(Message message)
            {
                AddMessages.Add(message);
            }

            public void OnChange(Message previous, Message current)
            {
                
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
                new Message { Id = "1", Value = 1 },
                new Message { Id = "2", Value = 2 }
            };

            var ds1 = new DataSet<Message>(list1);
            var ds2 = new DataSet<Message>(list2);

            var exe = new ComareExecutor<Message>(ds1, ds2, new CompareOptions { PrimaryKey = "Id" });

            exe.Observer = new Observer();

            exe.RunAsync().Wait();
        }
    }
}
