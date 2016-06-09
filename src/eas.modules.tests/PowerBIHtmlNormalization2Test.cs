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
            using (var fs = new FileStream(@"data\Desktop_Custom-Query-Error_m-p_40827.html", FileMode.Open, FileAccess.Read))
            {
                var normalization = new PowerBIHtmlNormalization2();

                normalization.Normalize(fs, new System.Collections.Generic.Dictionary<string, object> { { "url", "http://community.powerbi.com/t5/Desktop/Custom-Query-Error/m-p/40827/" } });
            }
        }
    }
}
