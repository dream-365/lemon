using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public interface IFindUri
    {
        IEnumerable<string> Find(string text);
    }
}
