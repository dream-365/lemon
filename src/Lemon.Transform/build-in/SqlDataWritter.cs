using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace Lemon.Transform
{
    public class SqlDataWritter : ITransformDataWritter
    {
        private const int BAT_SIZE = 100;

        private SqlConnection _connection;

        private SQLInserOrUpdateQueryBuilder _sqlInserOrUpdateQueryBuilder;

        private string _tableName;

        private string _primaryKey;

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

        public SqlDataWritter(SqlConnection connection, string tableName, string primaryKey = null)
        {
            _connection = connection;

            _tableName = tableName;

            _primaryKey = primaryKey;

            _sqlInserOrUpdateQueryBuilder = new SQLInserOrUpdateQueryBuilder(_tableName, PrimaryKey);
        }

        public IValueSetter GetValueSetter(object id)
        {
            return new SqlValueSetter(this, id, PrimaryKey);
        }

        public void Add(IDictionary<string, object> record)
        {
            var sql = _sqlInserOrUpdateQueryBuilder.Build(record.Keys);

            _connection.Execute(sql, record);
        }

        public void Flush()
        {
            
        }
    }
}
