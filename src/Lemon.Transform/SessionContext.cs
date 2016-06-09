using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class SessionContext
    {
        public ITransformDataReader2 DataReader { get; set; }

        public ITransformDataWritter2 DataWritter { get; set; }
    }
}
