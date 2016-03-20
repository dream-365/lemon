using System;

namespace Lemon.Transform
{
    public class DataReaderRegistration
    {
        public string Name { get; set; }

        public Func<DataSource, ITransformDataReader> CreateNew { get; set; }
    }
}
