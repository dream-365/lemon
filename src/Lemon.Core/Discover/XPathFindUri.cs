using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public class XPathFindUri : IFindUri
    {
        private XPathAttributeMap _map;

        public XPathFindUri(XPathAttributeMap map)
        {
            _map = map;
        }

        public IEnumerable<string> Find(string text)
        {
            var htmlAttributes = new HtmlAttributeParse(_map.XPath, _map.Attribute);

            var attributes = htmlAttributes.Parse(text);

            return attributes;
        }
    }
}
