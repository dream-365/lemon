using Lemon.Data.Core;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Lemon.Data.IO
{
    public class SqlDataWriter<T> : IDataWriter<T>
    {
        private readonly string _connectionString;

        private SqlConnection _connection;

        private readonly string _sql;

        public SqlDataWriter(string connectionString, string name)
        {
            _connectionString = connectionString;

            var schema = Util.BuildSchemaFromType(typeof(T));

            schema.Name = name;

            _sql = Util.BuildInsertSql(schema);

            _connection = new SqlConnection(_connectionString);
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public void Write(T record)
        {
            _connection.Execute(_sql, record);
        }
    }
}
