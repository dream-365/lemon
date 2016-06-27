using Lemon.Transform.Models;
using System.Collections.Generic;
using System;

namespace Lemon.Transform
{
    public class DataOutputModel : NamedParameterObjectModel, ICloneable
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

        public object Clone()
        {
            return new DataOutputModel
            {
                TargetType = TargetType,
                ObjectName = ObjectName,
                ColumnNames = ColumnNames,
                Connection = Connection,
                PrimaryKey = PrimaryKey,
                IsUpsert = IsUpsert
            };
        }
    }
}
