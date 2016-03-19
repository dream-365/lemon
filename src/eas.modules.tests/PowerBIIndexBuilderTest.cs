using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace eas.modules.tests
{
    [TestClass]
    public class PowerBIIndexBuilderTest
    {
        [TestMethod]
        public void BuildIndexTest()
        {
            var text = File.ReadAllText(@"data\powerbi.com_t5_Desktop_bd-p_power-bi-designer.html");

            PowerBIIndexBuilder builder = new PowerBIIndexBuilder();

            var items = builder.Build(text);
        }
    }
}
