using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using Lemon.Transform.Exceptions;

namespace Lemon.Transform.Tests
{
    /// <summary>
    /// Summary description for DataflowBlockReflectionWrapperTest
    /// </summary>
    [TestClass]
    public class DataflowBlockReflectionWrapperTest
    {
        public DataflowBlockReflectionWrapperTest()
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
        [ExpectedException(typeof(NotImplementedException))]
        public void NonSourceBlockLinkTo()
        {
            var actionBlock = new ActionBlock<int>((item) => { Task.Delay(100).Wait(); Console.WriteLine("action: {0}", item); }, new ExecutionDataflowBlockOptions { BoundedCapacity = 10 });

            var wrapper = new DataflowBlockReflectionWrapper(actionBlock);

            wrapper.LinkTo(new object(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(BlockLinkException))]
        public void TypeMismatchLinkTo()
        {
            var bufferBlock = new BufferBlock<int>(new DataflowBlockOptions { BoundedCapacity = 100 });

            var actionBlock = new ActionBlock<string>((item) => { Task.Delay(100).Wait(); Console.WriteLine("action: {0}", item); }, new ExecutionDataflowBlockOptions { BoundedCapacity = 10 });

            var wrapper = new DataflowBlockReflectionWrapper(bufferBlock);

            wrapper.LinkTo(actionBlock, new DataflowLinkOptions {  PropagateCompletion = true } );
        }
    }
}
