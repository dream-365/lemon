using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lemon.Transform.Tests
{
    [TestClass]
    public class MongoDataInputTest
    {
        [TestMethod]
        public void InputTest001()
        {
            var model = new DataInputModel
            {
                SourceType = "mongo",
                ObjectName = "landing.office_365_threads",
                Connection = "mongodb://localhost:27017",
                Filter = "{}"
            };

            var dataInput = new MongoDataInput(model);

            var index = 0;

            dataInput.Output = (row) => {
                index++;
            };

            dataInput.Start();

            Assert.AreEqual(1694, index);
        }
    }
}
