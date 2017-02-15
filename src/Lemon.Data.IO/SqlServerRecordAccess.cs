using System;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace Lemon.Data.IO
{
    public class SqlServerRecordAccess<T> : IRecordAccess<T>
    {
        private readonly string _connectionString;
        private readonly string _table;
        private readonly string _selectSql;
        private readonly string _upsertSql;

        public SqlServerRecordAccess(string connectionString, string table)
        {
            _connectionString = connectionString;
            _table = table;
            var schema = Util.BuildSchemaFromType(typeof(T));
            schema.Name = table;
            _selectSql = Util.BuildSelectSql(schema, null, schema.PrimaryKeys[0] + " = @PrimaryKey");
            _upsertSql = Util.BuildInsertSql(schema, true);
        }

        public T Load(object key)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>(_selectSql, new { PrimaryKey = key }).FirstOrDefault();
            }
        }

        public void Save(T record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(_upsertSql, record);
            }
        }
    }
}
