using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core
{
    public class CompareOptions
    {
        public string PrimaryKey { get; set; }

        public string[] ColumnsToCompare { get; set; }
    }
}
