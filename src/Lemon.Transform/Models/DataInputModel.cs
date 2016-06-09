using System.Collections.Generic;

namespace Lemon.Transform
{
    public class DataInputModel
    {
        public string SourceType { get; set; }

        public string Filter { get; set; }

        public string ObjectName { get; set; }

        public IEnumerable<string> ColumnNames { get; set; }

        public string Connection { get; set; }

        public string PrimaryKey { get; set; }
    }
}
