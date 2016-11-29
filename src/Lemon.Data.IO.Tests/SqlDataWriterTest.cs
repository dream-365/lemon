using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lemon.Data.IO.Tests
{
    [TestClass]
    public class SqlDataWriterTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sqlWriter = new SqlDataWriter<Message>("connection", "table_name", WriteMode.Update);
        }
    }
}
