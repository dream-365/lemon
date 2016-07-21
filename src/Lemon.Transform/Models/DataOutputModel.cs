using Lemon.Transform.Models;
using System.Collections.Generic;
using System;

namespace Lemon.Transform
{
    public class WriteOnChangeConfiguration
    {
        public bool Enabled { get; set; }

        public string[] ExcludedColumNames {get; set;}
    }

    public class DataOutputModel : NamedParameterObjectModel, ICloneable
    {
        /// <summary>
        /// the type of data output, e.g. mssql, mongo
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// the object name in target data source, e.g. table name, collection name
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// the columns names to output
        /// </summary>
        public IEnumerable<string> ColumnNames { get; set; }

        /// <summary>
        /// connecton string to connect to data source
        /// </summary>
        public string Connection { get; set; }

        [Obsolete("PrimaryKey is obsoleted, use PrimaryKeys for instead", false)]
        public string PrimaryKey { get; set; }

        /// <summary>
        /// primary keys in target data source, used for data compare
        /// </summary>
        public string[] PrimaryKeys { get; set; }

        /// <summary>
        /// false: insert-only
        /// true: insert-or-update
        /// </summary>
        public bool IsUpsert { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WriteOnChangeConfiguration WriteOnChange { get; set; }

        /// <summary>
        /// replace the parameters with named value
        /// </summary>
        /// <param name="parameters"></param>
        public override void RepalceWithNamedParameters(IDictionary<string, string> parameters)
        {
            ObjectName = RepalceWithNamedParameters(ObjectName, parameters);
        }

        public object Clone()
        {
            var clone = new DataOutputModel
            {
                TargetType = TargetType,
                ObjectName = ObjectName,
                ColumnNames = ColumnNames,
                Connection = Connection,
                PrimaryKey = PrimaryKey,
                PrimaryKeys = PrimaryKeys,
                IsUpsert = IsUpsert
            };

            if(WriteOnChange != null)
            {
                clone.WriteOnChange.Enabled = WriteOnChange.Enabled;

                clone.WriteOnChange.ExcludedColumNames = WriteOnChange.ExcludedColumNames;
            }

            return clone;
        }
    }
}
