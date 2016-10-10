using Lemon.Data.Core;
using Lemon.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

            var schema = BuildSchemaFromType(typeof(T));

            schema.Name = name;

            _sql = SqlUtil.BuildInsertSql(schema);

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

        private DataTableSchema BuildSchemaFromType(Type type)
        {
            var columns = new List<Lemon.Data.Core.Models.DataColumn>();

            var primaryKeys = new List<string>();

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var notMapped = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), false)
                                        .FirstOrDefault();

                if (notMapped != null)
                {
                    continue;
                }

                columns.Add(new Core.Models.DataColumn { Name = property.Name });

                var key = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), false)
                                  .FirstOrDefault();

                if (key != null)
                {
                    primaryKeys.Add(property.Name);
                }
            }

            return new DataTableSchema
            {
                Columns = columns.ToArray(),
                PrimaryKeys = primaryKeys.ToArray()
            };
        }
    }
}
