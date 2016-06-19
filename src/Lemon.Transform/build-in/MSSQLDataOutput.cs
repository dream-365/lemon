using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Lemon.Transform.build_in
{
    public class MSSQLDataOutput : AbstractDataOutput, IDisposable
    {
        private string _connectionString;

        private string _tableName;

        private string _primaryKey;

        private IEnumerable<string> _columnNames;

        private SqlConnection _connection;

        private string _sql;

        private SQLInserOrUpdateQueryBuilder _builder;

        public MSSQLDataOutput(DataOutputModel model)
        {
            _connectionString = model.Connection;

            _tableName = model.ObjectName;

            _primaryKey = model.PrimaryKey;

            _columnNames = model.ColumnNames;

            _builder = new SQLInserOrUpdateQueryBuilder(_tableName, _primaryKey);

            _sql = _builder.Build(model.ColumnNames, model.IsUpsert);

            Connect();
        }

        private void Connect()
        {
            _connection = new SqlConnection(_connectionString);
        }


        private static object CastBsonValueToDotNetObject(BsonValue value)
        {
            object dotNetObject;

            switch (value.BsonType)
            {
                case BsonType.Int32: dotNetObject = value.AsInt32; break;
                case BsonType.Int64: dotNetObject = value.AsInt64; break;
                case BsonType.String: dotNetObject = value.AsString; break;
                case BsonType.Double: dotNetObject = value.AsDouble; break;
                case BsonType.Boolean: dotNetObject = value.AsBoolean; break;
                case BsonType.DateTime: dotNetObject = value.ToUniversalTime(); break;
                case BsonType.Null: dotNetObject = null; break;
                default: throw new NotSupportedException(string.Format("the bson type [{0}] is not supported", value.BsonType));
            }

            return dotNetObject;
        }

        private void Input(BsonDataRow inputRow)
        {
            var dict = new Dictionary<string, object>();

            foreach (var columName in _columnNames)
            {
                dict.Add(columName, CastBsonValueToDotNetObject(inputRow.GetValue(columName)));
            }

            _connection.Execute(_sql, dict);
        }

        public void Dispose()
        {
            _connection.Close();
        }

        protected override void OnReceive(BsonDataRow row)
        {
            Input(row);
        }
    }
}
