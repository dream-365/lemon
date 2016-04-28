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
            using (var fs = new FileStream(@"data\powerbi.com_t5_Desktop_unexpected-error_m-p_31104.html", FileMode.Open, FileAccess.Read))
            {
                var normalization = new PowerBIHtmlNormalization2();

                normalization.Normalize(fs, new System.Collections.Generic.Dictionary<string, object> { { "url", "http://community.powerbi.com/t5/Desktop/unexpected-error/m-p/31104/" } });
            }
        }
    }
}
