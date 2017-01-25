using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lemon.Data.Core;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace Lemo.Data.Core.Tests
{
    [TestClass]
    public class ComparingPipeTests
    {
        [TestMethod()]
        public void CompareSortedList()
        {
            ServicesInstaller.Current.Rebuild();

            var options = new CompareOptions {
                PrimaryKey = "Id",
                ColumnsToCompare = new string[] { "Title" }
            };
            var observer = new AssertObserver<DataRow>();
            var list1 = PrepareList1();
            var list2 = PrepareList2();

            var pipe = new ComparingPipe<DataRow>(options, observer);
            pipe.Compare(list1, list2);

            Assert.AreEqual(1, observer.Adds.Count());
            Assert.AreEqual(1, observer.Deletes.Count());
            Assert.AreEqual(1, observer.Changes.Count());
        }

        [TestMethod]
        public void RegisterCusmtomKeyComparer()
        {
            ServicesInstaller.Current.Install<IComparer<DataRow>, CusmtomKeyComparer>();

            var options = new CompareOptions
            {
                PrimaryKey = "Id",
                ColumnsToCompare = new string[] { "Title" }
            };
            var observer = new AssertObserver<DataRow>();
            var list1 = PrepareList1();
            var list2 = PrepareList2();

            var pipe = new ComparingPipe<DataRow>(options, observer);

            pipe.Compare(list1, list2);

            Assert.AreEqual(0, observer.Adds.Count());
            Assert.AreEqual(0, observer.Deletes.Count());
            Assert.AreEqual(2, observer.Changes.Count());
        }


        [TestMethod]
        public void BsonDocumentCompare()
        {
            ServicesInstaller.Current.Rebuild();

            var list1 = new List<BsonDocument>
            {
                new BsonDocument { { "id", 1}, { "title", "title #1"} },
                new BsonDocument { { "id", 3}, { "title", "title #3"} }
            };

            var list2 = new List<BsonDocument>
            {
                new BsonDocument { { "id", 1}, { "title", "title $1"} },
                new BsonDocument { { "id", 2}, { "title", "title #2"} }
            };

            var options = new CompareOptions
            {
                PrimaryKey = "id",
                ColumnsToCompare = new string[] { "title" }
            };

            var observer = new AssertObserver<BsonDocument>();

            var pipe = new ComparingPipe<BsonDocument>(options, observer);

            pipe.Compare(list1, list2);

            Assert.AreEqual(1, observer.Adds.Count());
            Assert.AreEqual(1, observer.Deletes.Count());
            Assert.AreEqual(1, observer.Changes.Count());
        }

        private IList<DataRow> PrepareList2()
        {
            return new List<DataRow>
            {
                new DataRow { Id = 1, Title = "Data Row #1-" },
                new DataRow { Id = 3, Title = "Data Row #3" }
            };
        }

        private IList<DataRow> PrepareList1()
        {
            return new List<DataRow>
            {
                new DataRow { Id = 1, Title = "Data Row #1" },
                new DataRow { Id = 2, Title = "Data Row #2" }
            };
        }
    }
}
