using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace Lemon.Transform
{
    public class SqlServerDataOutput : AbstractDataOutput, IDisposable
    {
        private string _connectionString;

        private string _tableName;

        private string[] _primaryKeys;

        private IEnumerable<string> _columnNames;

        private SqlConnection _connection;

        private string _sql;

        private SQLInserOrUpdateQueryBuilder _builder;

        public SqlServerDataOutput(DataOutputModel model)
        {
            var table = model.Schema;

            _connectionString = model.Connection.ConnectionString;

            _tableName = table.ObjectName;

            _primaryKeys = table.PrimaryKeys;

            _columnNames = table.ColumnNames;
            
            _builder = new SQLInserOrUpdateQueryBuilder(_tableName, _primaryKeys);

            _sql = _builder.Build(model.Schema.ColumnNames, model.IsUpsert);

            DetermineWriteOrNot = BuildDetermineWriteOrNotFunction(model.WriteOnChange);

            Connect();
        }

        private void Connect()
        {
            _connection = new SqlConnection(_connectionString);
        }

        protected override DataRowStatusContext BuildDataRowStatusContext(string[] excludes)
        {
            var columns = new List<string>();

            foreach (var column in _columnNames)
            {
                if (_primaryKeys.Any(exclude => exclude == column) || 
                    excludes.Any(exclue => exclue == column))
                {
                    continue;
                }

                columns.Add(column);
            }

            return new SqlServerRecordStatusContext(
                    _connectionString,
                    _tableName,
                    _primaryKeys,
                    columns.ToArray());
        }

        private void Input(BsonDataRow inputRow)
        {
            if(!DetermineWriteOrNot(inputRow))
            {
                return;
            }

            var dict = new Dictionary<string, object>();

            foreach (var columName in _columnNames)
            {
                dict.Add(columName, CastUtil.Cast(inputRow.GetValue(columName)));
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
