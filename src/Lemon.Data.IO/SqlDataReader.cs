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

        private bool _buffered = true;

        private object _parameters;

        public bool Buffered {
            get { return _buffered; }
            set { _buffered = value; }
        }

        public SqlDataReader(string connectionString, string table, string orderBy, string whereClause = null)
        {
            _connectionString = connectionString;

            var schema = Util.BuildSchemaFromType(typeof(T));

            schema.Name = table;

            _sql = Util.BuildSelectSql(schema, orderBy, whereClause);

            _connection = new SqlConnection(_connectionString);
        }

        public SqlDataReader(string connectionString, string query, object parameters = null)
        {
            _connectionString = connectionString;

            _parameters = parameters;

            _sql = query;
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
                var result = _connection.Query<T>(_sql, param: _parameters, buffered: _buffered);

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
