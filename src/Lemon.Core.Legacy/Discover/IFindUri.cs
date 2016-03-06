using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    // decrepted
    public interface IFindUri
    {
        IEnumerable<string> Find(string text);
    }
}
