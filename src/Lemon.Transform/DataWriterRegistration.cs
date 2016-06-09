using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class DataWriterRegistration
    {
        public string Name { get; set; }

        public Func<DataOutputModel, ITransformDataWritter> CreateNew { get; set; }
    }
}
