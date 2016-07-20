using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Lemon.Transform
{
    internal class FieldTypeMappingCache
    {
        public IEnumerable<FieldTypeMapping> Fields;
    }

    internal class SqlDataRowReader
    {
        private SqlDataReader _sqlDataReader;

        private IEnumerable<FieldTypeMapping> _fields;

        public SqlDataRowReader(SqlDataReader sqlDataReader, FieldTypeMappingCache cache)
        {
            _sqlDataReader = sqlDataReader;

            if(cache.Fields == null)
            {
                InitializeCache(cache);
            }

            _fields = cache.Fields;
        }

        /// <summary>
        /// initialze the cache
        /// </summary>
        /// <param name="cache"></param>
        private void InitializeCache(FieldTypeMappingCache cache)
        {
            lock (cache)
            {
                IList<FieldTypeMapping> mappings = new List<FieldTypeMapping>();

                var table = _sqlDataReader.GetSchemaTable();

                foreach (DataRow dataRow in table.Rows)
                {
                    var name = dataRow.Field<string>(0);

                    var ordinal = dataRow.Field<int>(1);

                    var type = dataRow.Field<Type>(12);

                    mappings.Add(new FieldTypeMapping { Ordinal = ordinal, FieldName = name, DataType = type });
                }

                cache.Fields = mappings;
            }
        }

        public bool Read()
        {
            return _sqlDataReader.Read();
        }

        /// <summary>
        /// get the bson data row
        /// </summary>
        /// <returns></returns>
        public BsonDataRow GetDataRow()
        {
            var document = new BsonDocument();

            foreach (FieldTypeMapping column in _fields)
            {
                if (_sqlDataReader.IsDBNull(column.Ordinal))
                {
                    document.Add(column.FieldName, BsonNull.Value);
                }
                else
                {
                    var value = _sqlDataReader.GetValue(column.Ordinal);

                    document.Add(column.FieldName, CastUtil.Cast(value, column.DataType));
                }
            }

            var bsonDataRow = new BsonDataRow(document);

            return bsonDataRow;
        }

        /// <summary>
        /// close the sql data reader
        /// </summary>
        public void Close()
        {
            _sqlDataReader.Close();
        }
    }
}
