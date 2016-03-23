using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace eas.modules.tests
{
    [TestClass]
    public class PowerBIHtmlNormalization2Test
    {
        [TestMethod]
        public void NormalizeTest()
        {
            using (var fs = new FileStream(@"data\poerbi.com_detail_page.html", FileMode.Open, FileAccess.Read))
            {
                var normalization = new PowerBIHtmlNormalization2();

                normalization.Normalize(fs);
            }
        }
    }
}
