using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.MicroKernel.Registration;

namespace Lemon.Transform.Tests
{
    [TestClass]
    public class PackageTests
    {
        [TestMethod]
        public void BuildPackageTest001()
        {
            LemonTransform.RegisterServcie<IDataStoreService, FakeDataStoreService>();

            var builder = new PackageBuilder();

            var package = builder.Input("mongo_office_365")
                   .Action(new FakeTransformAction1())
                   .Output("json_office_365")
                   .Build();

            Assert.AreEqual("landing.office_365_threads", package.Input.ObjectName);
            Assert.AreEqual("landing.output_test", package.Output.ObjectName);
            Assert.AreEqual(1, package.Actions.Count);
        }


        [TestMethod]
        public void PackageContainterInstallTest001()
        {
            LemonTransform.RegisterServcie<IDataStoreService, FakeDataStoreService>();

            var container = new PackageContainer();

            container.InstallPackage<CustomPackageInstallation>("package001");

            var package = container.Resove("package001");

            Assert.AreEqual("landing.office_365_threads", package.Input.ObjectName);
            Assert.AreEqual("landing.output_test", package.Output.ObjectName);
            Assert.AreEqual(1, package.Actions.Count);
        }

        [TestMethod]
        public void RegisterDataInput()
        {
            LemonTransform.RegisterDataInput<FakeDataInput>("fakedb");

            var model = new DataInputModel
            {
                SourceType = "fakedb",
                ObjectName = "landing.office_365_threads",
                Connection = "mongodb://localhost:27017",
                Filter = "{}"
            };

            var factory = new DataInputOutputFactory();

            var input = factory.CreateDataInput(model);

            Assert.AreEqual(typeof(FakeDataInput), input.GetType());
        }

        [TestMethod]
        public void PackageExecutionTest001()
        {
            LemonTransform.RegisterServcie<IDataStoreService, FakeDataStoreService>();

            LemonTransform.RegisterDataInput<FakeDataInput>("fakedb");

            LemonTransform.RegisterDataOutput<FakeDataOutput>("fakedb");

            var builder = new PackageBuilder();

            var package = builder.Input("mongo_office_365")
                   .Action(new FakeTransformAction2())
                   .Output("json_office_365")
                   .Build();

            var engine = new CoreDocumentTransformEngine();

            engine.Execute(package);
        }
    }
}
