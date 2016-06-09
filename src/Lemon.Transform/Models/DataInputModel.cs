using Lemon.Transform.Models;
using System.Collections.Generic;
using System;

namespace Lemon.Transform
{
    public class DataInputModel : NamedParameterObjectModel
    {
        public string SourceType { get; set; }

        public string Filter { get; set; }

        public string ObjectName { get; set; }

        public IEnumerable<string> ColumnNames { get; set; }

        public string Connection { get; set; }

        public string PrimaryKey { get; set; }

        public override void RepalceWithNamedParameters(IDictionary<string, string> parameters)
        {
            ObjectName = RepalceWithNamedParameters(ObjectName, parameters);

            Filter = RepalceWithNamedParameters(Filter, parameters);
        }
    }
}
