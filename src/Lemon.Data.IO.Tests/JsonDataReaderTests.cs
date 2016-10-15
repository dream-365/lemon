using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lemon.Data.IO.Tests
{
    [TestClass]
    public class JsonDataReaderTests
    {
        public class Record
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        [TestMethod]
        public void JsonDataReaderReadAll()
        {
            var jsonReader = new JsonDataReader<Record>(@"test_data.json");

            int count = 0;

            while(jsonReader.Next())
            {
                var value = jsonReader.Read();

                count++;
            }

            Assert.AreEqual(6, count);
        }
    }
}
