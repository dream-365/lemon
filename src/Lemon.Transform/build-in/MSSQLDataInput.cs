using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Lemon.Transform
{
    public class MSSQLDataInput : AbstractDataInput, IDisposable
    {
        private SqlConnection _connection;

        private string _connectionString;

        private string _sql;

        private IDictionary<string, object> _parameters;

        public MSSQLDataInput(DataInputModel model)
        {
            _connectionString = model.Connection;

            _sql = SqlNamedQueryProvider.Instance.Get(model.ObjectName);

            _parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(model.Filter);

            _connection = new SqlConnection(_connectionString);
        }

        private class FieldTypeMapping
        {
            public int Ordinal { get; set; }

            public string FieldName { get; set; }

            public Type DataType { get; set; }
        }

        public void ForEach(Action<BsonDataRow> forEach)
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

                forEach(new BsonDataRow(document));
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

        public override void Start()
        {
            ForEach(Post);
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
