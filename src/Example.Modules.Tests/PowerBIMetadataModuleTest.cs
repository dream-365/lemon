using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Example.Modules.Tests
{
    [TestClass]
    public class PowerBIMetadataModuleTest
    {
        [TestMethod]
        public void PorccessBasicTest()
        {
            var mod = new PowerBIMeatadataModule();

            using (var fs = new FileStream("test2.html", FileMode.Open, FileAccess.Read))
            {
                var dict = new Dictionary<string, object>();

                mod.OnProcess(dict, fs);
            }
        }
    }
}
