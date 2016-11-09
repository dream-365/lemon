using Lemon.Data.Core;
using System.Data.SqlClient;
using Dapper;

namespace Lemon.Data.IO
{
    public class SqlDataWriter<T> : IDataWriter<T>
    {
        private readonly string _connectionString;

        private readonly string _sql;

        public SqlDataWriter(string connectionString, string name)
        {
            _connectionString = connectionString;

            var schema = Util.BuildSchemaFromType(typeof(T));

            schema.Name = name;

            _sql = Util.BuildInsertSql(schema);
        }

        public void Dispose()
        {
        }

        public void Write(T record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(_sql, record);
            }  
        }
    }
}
