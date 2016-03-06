using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public class XPathFindUriSetting
    {
        public string BaseUri { get; set; }

        public string Filter { get; set; }

        public XPathAttributeMap XPathAttributeMap { get; set; }

        public RegexTransform Transform { get; set; }
    }
}
