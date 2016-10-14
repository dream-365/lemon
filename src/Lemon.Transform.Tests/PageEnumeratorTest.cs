using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemon.Data.Core;

namespace Lemon.Transform.Tests
{
    /// <summary>
    /// Summary description for PageEnumeratorTest
    /// </summary>
    [TestClass]
    public class PageEnumeratorTest
    {
        public PageEnumeratorTest()
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
        public void LessThanDefaultLength()
        {
            var datasource = new StringDataSource("string_{0}", 1, 5);

            var pageEnum = new PageEnumerator<string>(datasource.GetEnumerator(), 1024);

            int count = 0;

            while(pageEnum.MoveNext())
            {
                count++;
            }

            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public void EqualToLength()
        {
            List<string> list = new List<string> { "a", "b", "c", "d", "e" };

            var pageEnum = new PageEnumerator<string>(list.GetEnumerator(), 5);

            int count = 0;

            while (pageEnum.MoveNext())
            {
                count++;
            }

            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public void MoreThanLength()
        {
            var datasource = new StringDataSource("string_{0}", 1, 1024);

            var pageEnum = new PageEnumerator<string>(datasource.GetEnumerator(), 5);

            int count = 0;

            while (pageEnum.MoveNext())
            {
                count++;
            }

            Assert.AreEqual(1024, count);
        }
    }
}
