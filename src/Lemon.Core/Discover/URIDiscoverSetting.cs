using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public class URIDiscoverSetting
    {
        public string UriTemplate { get; set; }

        public string Encoding { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public int Step { get; set; }
    }
}
