using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Lemon.Transform
{
    public class SqlServerRecordStatusContext : DataRowStatusContext
    {
        private string _connectionString;

        private string _sql;

        private string _tableName;

        private FieldTypeMappingCache _fieldsCache;

        public SqlServerRecordStatusContext
            (string connectionString,
            string tableName,
            string[] primaryKeys,
            string[] columnsToCompare)
        : base(primaryKeys, columnsToCompare)
        {
            _connectionString = connectionString;

            _tableName = tableName;

            _fieldsCache = new FieldTypeMappingCache();
        }

        protected string BuildQuerySql()
        {
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.AppendLine("SELECT");

            sqlBuilder.AppendLine("[" + ColumnsToCompare.FirstOrDefault() + "]");

            foreach (var column in ColumnsToCompare.Skip(1))
            {
                sqlBuilder.AppendLine(",[" + column + "]");
            }

            sqlBuilder.AppendLine(" FROM " + _tableName);

            sqlBuilder.Append("WHERE ");

            sqlBuilder.Append(string.Format("[{0}] = @{0}", PrimaryKeys.FirstOrDefault()));

            foreach (var primaryKey in PrimaryKeys.Skip(1))
            {
                sqlBuilder.Append(string.Format("AND [{0}] = @{0}", primaryKey));
            }

            return sqlBuilder.ToString();
        }

        protected string Sql
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sql))
                {
                    lock (this)
                    {
                        _sql = BuildQuerySql();
                    }
                }

                return _sql;
            }
        }

        protected override BsonDataRow GetTargetRow(BsonDataRow row)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(Sql, connection);

                foreach (var key in PrimaryKeys)
                {
                    command.Parameters.Add(new SqlParameter(
                        key,
                        CastUtil.Cast(row.GetValue(key))));
                }

                connection.Open();

                SqlDataRowReader reader = new SqlDataRowReader(command.ExecuteReader(), _fieldsCache);

                if(!reader.Read())
                {
                    return null;
                }

                return reader.GetDataRow();
            }
        }
    }
}
