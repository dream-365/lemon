using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lemon.Transform.Tests
{
    /// <summary>
    /// Summary description for PipelineTest
    /// </summary>
    [TestClass]
    public class PipelineTest
    {
        public PipelineTest()
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
        public void BuildPipeline()
        {
            var pipeline = new Pipeline();

            var action1 = new PrefixTransformBlock("a");

            var action2 = new PrefixTransformBlock("b");

            var action3 = new PrefixTransformBlock("c");

            var writer1 = new ConsoleDataWriter();
            var writer2 = new ConsoleDataWriter();

            var broadcast = pipeline.DataSource(new RandomDataReader())
                    .Next(action1)
                    .Next(action2)
                    .Next(action3)
                    .Broadcast();

            broadcast.Branch().Next(action1).Output(writer1);
            broadcast.Branch().Next(action2).Output(writer2);

            var exe = pipeline.Build();

            exe.RunAsync(null).Wait();
        }
    }
}
