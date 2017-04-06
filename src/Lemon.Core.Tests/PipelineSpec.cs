using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Lemon.Core.Tests
{
    [TestClass]
    public class PipelineSpec
    {
        [TestMethod]
        public void ShouldTransformInMemoryListSuccessfully()
        {
            var pipeline = new Pipeline { BoundedCapacity = 10 };
            var source = new List<string>
            {
                "message#1",
                "message#2",
                "message#3"
            };
            var result = new List<string>();
            var target = new ListWriter<string>(result);
            pipeline.Read(source)
                    .Transform(item => item.Replace("#", "@"))
                    .Write(target);
            var exe = pipeline.Build();
            var runningResult = exe.RunAsync().Result;
            Assert.IsTrue(runningResult, "should run without error");
            Assert.AreEqual(source.Count, result.Count, "the count should be the same");
            Assert.AreEqual("message@1", result[0], "transform should work");
        }
    }
}
