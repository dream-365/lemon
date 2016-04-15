using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class SqlTransformDataReader : ITransformDataReader
    {
        private SqlConnection _connection;

        private string _sql;

        private string _primaryKey;

        private IDictionary<string, object> _parameters;

        public string PrimaryKey
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_primaryKey))
                {
                    return "Id";
                }

                return _primaryKey;
            }
        }

        public void SetPrimaryKey(string key)
        {
            _primaryKey = key;
        }

        public SqlTransformDataReader(SqlConnection connection, string sql, IDictionary<string, object> parameters = null)
        {
            _connection = connection;

            _parameters = parameters;

            _sql = sql;
        }

        private class FieldTypeMapping
        {
            public int Ordinal { get; set; }

            public string FieldName { get; set; }

            public Type DataType { get; set; }
        }

        public void ForEach(Action<IValueProvider> forEach)
        {
            SqlCommand command = new SqlCommand(_sql, _connection);

            foreach(var parameter in _parameters)
            {
                command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
            }

            _connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            IList<FieldTypeMapping> mappings = new List<FieldTypeMapping>();

            var table = reader.GetSchemaTable();

            foreach(DataRow row in table.Rows)
            {
                var name = row.Field<string>(0);

                var ordinal = row.Field<int>(1);

                var type = row.Field<Type>(12);

                mappings.Add(new FieldTypeMapping { Ordinal = ordinal, FieldName = name, DataType = type });
            }

            while (reader.Read())
            {
                var document = new BsonDocument();

                foreach (FieldTypeMapping column in mappings)
                {
                    if(reader.IsDBNull(column.Ordinal))
                    {
                        document.Add(column.FieldName, BsonNull.Value);
                    }
                    else
                    {
                        var value = reader.GetValue(column.Ordinal);

                        document.Add(column.FieldName, Cast(value, column.DataType));
                    }
                }

                forEach(new BsonDocumentValueProvider(document));
            }

            reader.Close();
        }

        private static BsonValue Cast(object dotNetValue,Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return new BsonString(dotNetValue as string);
            }
            else if (sourceType == typeof(int))
            {
                return new BsonInt32((int)dotNetValue);
            }
            else if (sourceType == typeof(bool))
            {
                return new BsonBoolean((bool)dotNetValue);
            }else if (sourceType == typeof(DateTime))
            {
                return new BsonDateTime((DateTime)dotNetValue);
            }

            throw new NotSupportedException();
        }
    }
}
