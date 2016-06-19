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
            FakeTransformAction1 action1 = new FakeTransformAction1();

            var row = new BsonDataRow();

            row.SetValue("Id", new BsonString("Hello"));
            row.SetValue("TransformField", new BsonString("World"));

            var result = action1.Transform(row);
        }

        [TestMethod]
        public void DataFlowPipelineTest001()
        {
            LemonTransform.RegisterServcie<IDataStoreService, FakeDataStoreService>();

            LemonTransform.RegisterDataInput<FakeDataInput>("fakedb");

            LemonTransform.RegisterDataOutput<FakeDataOutput>("fakedb");

            var pipeline = new FakeDataFlowPipeline();

            pipeline.Run();
        }
    }
}
