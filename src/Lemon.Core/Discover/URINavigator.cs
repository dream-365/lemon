using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public class URINavigator
    {
        private readonly string _uriTemplate;

        private int _step = 1;

        public URINavigator(string uriTemplate)
        {
            _uriTemplate = uriTemplate;
        }

        public URINavigator(string uriTemplate, int step) : this(uriTemplate)
        {
            _step = step;
        }

        public string Goto(int index)
        {
            var uri = string.Format(_uriTemplate, index * _step);

            return uri;
        }
    }
}
