using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace Lemon.Transform.Tests
{
    [TestClass]
    public class TransformActionTests
    {
        [TestMethod]
        public void TransformActionBasicTest01()
        {
            TransformAction action1 = new FakeTransformAction1();

            var row = new BsonDataRow();

            row.SetValue("Id", new BsonString("Hello"));
            row.SetValue("TransformField", new BsonString("World"));

            action1.Output = (outputRow) => {
                Assert.AreEqual("Hello", outputRow.GetValue("PropertyId").AsString);
                Assert.AreEqual("World_", outputRow.GetValue("TransformField").AsString);
                Assert.AreEqual("Hello_World", outputRow.GetValue("CalculateField").AsString);
            };

            action1.Input(row);
        }
    }
}
