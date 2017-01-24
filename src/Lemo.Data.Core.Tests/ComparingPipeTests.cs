using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemon.Data.Core;
using System.Collections.Generic;
using System.Linq;

namespace Lemo.Data.Core.Tests
{
    [TestClass]
    public class ComparingPipeTests
    {
        [TestMethod]
        public void CompareSortedList()
        {
            var options = new CompareOptions {
                PrimaryKey = "Id",
                ColumnsToCompare = new string[] { "Title" }
            };
            var observer = new AssertObserver<DataRow>();
            var list1 = new List<DataRow>
            {
                new DataRow { Id = 1, Title = "Data Row #1" },
                new DataRow { Id = 2, Title = "Data Row #2" }
            };

            var list2 = new List<DataRow>
            {
                new DataRow { Id = 1, Title = "Data Row #1-" },
                new DataRow { Id = 3, Title = "Data Row #3" }
            };

            var pipe = new ComparingPipe<DataRow>(options, observer);
            pipe.Compare(list1, list2);

            Assert.AreEqual(1, observer.Adds.Count());
            Assert.AreEqual(1, observer.Deletes.Count());
            Assert.AreEqual(1, observer.Changes.Count());
        }
    }
}
