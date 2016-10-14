using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Data.Core
{
    public class DataSet<T>
    {
        public DataSet(IEnumerable<T> rows)
        {

        }

        public T Current()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }
    }
}
