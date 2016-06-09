using System.Collections.Generic;

namespace Lemon.Transform
{
    public class DataOutputModel
    {
        public string TargetType { get; set; }

        public string ObjectName { get; set; }

        public IEnumerable<string> ColumnNames { get; set; }

        public string Connection { get; set; }

        public string PrimaryKey { get; set; }

        public bool IsUpsert { get; set; }
    }
}
