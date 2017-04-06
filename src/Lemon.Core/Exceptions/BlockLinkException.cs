using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Exceptions
{
    public class BlockLinkException : Exception
    {
        public BlockLinkException(string message) : base(message)
        {

        }
    }
}
