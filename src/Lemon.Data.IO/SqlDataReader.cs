using Lemon.Data.Core;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace Lemon.Data.IO
{
    public class SqlDataReader<T> : IDataReader<T>
    {
        private readonly string _connectionString;

        private SqlConnection _connection;

        private readonly string _sql;

        private IEnumerator<T> _enumerator;

        public SqlDataReader(string connectionString, string table, string whereClause = null)
        {
            _connectionString = connectionString;

            var schema = Util.BuildSchemaFromType(typeof(T));

            schema.Name = table;

            _sql = Util.BuildSelectSql(schema, whereClause);

            _connection = new SqlConnection(_connectionString);
        }

        public void Dispose()
        {
            if(_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public bool Next()
        {
            if(_enumerator == null)
            {
                var result = _connection.Query<T>(_sql);

                if(result == null)
                {
                    return false;
                }

                _enumerator = result.GetEnumerator();
            }

            return _enumerator.MoveNext();
        }

        public T Read()
        {
            return _enumerator.Current;
        }

        object Core.IDataReader.Read()
        {
            return Read();
        }
    }
}
