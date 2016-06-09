using Lemon.Transform.Models;
using System.Collections.Generic;
using System;

namespace Lemon.Transform
{
    public class DataOutputModel : NamedParameterObjectModel
    {
        public string TargetType { get; set; }

        public string ObjectName { get; set; }

        public IEnumerable<string> ColumnNames { get; set; }

        public string Connection { get; set; }

        public string PrimaryKey { get; set; }

        public bool IsUpsert { get; set; }

        public override void RepalceWithNamedParameters(IDictionary<string, string> parameters)
        {
            ObjectName = RepalceWithNamedParameters(ObjectName, parameters);
        }
    }
}
