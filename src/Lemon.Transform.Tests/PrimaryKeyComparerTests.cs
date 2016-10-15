using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemon.Data.Core;

namespace Lemon.Transform.Tests
{
    /// <summary>
    /// Summary description for PrimaryKeyComparerTests
    /// </summary>
    [TestClass]
    public class PrimaryKeyComparerTests
    {
        public class Message
        {
            public string Id { get; set; }

            public int Value { get; set; }
        }


        public PrimaryKeyComparerTests()
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
        public void CompareStringPrimaryKey()
        {
            var comparer = new PrimaryKeyComparer<Message>("Id");

            var left = new Message { Id = "123456", Value = 123456 };

            var right = new Message { Id = "abcdefg", Value = 123 };

            var result = comparer.Compare(left, right);

            Assert.AreEqual(-1, result);
        }
    }
}
