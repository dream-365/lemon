using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Lemon.Transform
{
    /// <summary>
    /// Microsoft SQL Server data input
    /// </summary>
    public class SqlServerDataInput : AbstractDataInput, IDisposable
    {
        private SqlConnection _connection;

        private string _connectionString;

        private string _sql;

        private long _count;

        private IDictionary<string, string> _parameters = new Dictionary<string, string>();

        public SqlServerDataInput(DataInputModel model)
        {
            _connectionString = model.Connection.ConnectionString;

            _sql = BuildSqlText(model);

            _connection = new SqlConnection(_connectionString);

            if(model.Parameters != null)
            {
                PrametersInfo.RegisterParameters(model.Parameters);
            }
        }

        /// <summary>
        /// Build T-SQL text from the data input model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string BuildSqlText(DataInputModel model)
        {
            string sql = null;

            var indexOfFirstColon = model.Object.IndexOf(':');

            var objectType = model.Object.Substring(0, indexOfFirstColon).Trim();

            var objectContent = model.Object.Substring(indexOfFirstColon + 1).Trim();

            if (objectType == "table")
            {
                var sb = new StringBuilder();

                var firstColumnName = model.Schema.Columns.First().Name;

                sb.AppendLine("SELECT [" + firstColumnName + "]");

                foreach (var column in model.Schema.Columns.Skip(1))
                {
                    sb.AppendLine(",[" + column.Name + "]");
                }

                sb.AppendLine("FROM [" + objectContent + "]");

                if (!string.IsNullOrWhiteSpace(model.Filter))
                {
                    sb.AppendLine(model.Filter);
                }

                sql = sb.ToString();
            }
            else if (objectType == "sqlfile")
            {
                var tempSql = SqlNamedQueryProvider.Instance.Get(objectContent);

                if (!string.IsNullOrWhiteSpace(model.Filter))
                {
                    sql = "SELECT * FROM (" + tempSql + ") [generated_sql_temp] " + model.Filter;
                }
            }
            else
            {
                throw new NotSupportedException("object type " + objectType + " is not supported by Microsoft.SqlServer intput");
            }

            return sql;
        }

        /// <summary>
        /// excute the data reader
        /// </summary>
        /// <param name="forEach"></param>
        public void Excute(Action<BsonDataRow> forEach, IDictionary<string, object> parameters)
        {
            var executeParameters = PrametersInfo.ValidateParameters(parameters);

            SqlCommand command = new SqlCommand(_sql, _connection);

            if(executeParameters != null)
            {
                foreach (var parameter in executeParameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }

            _connection.Open();

            FieldTypeMappingCache cache = new FieldTypeMappingCache();

            SqlDataRowReader reader = new SqlDataRowReader(command.ExecuteReader(), cache);

            while (reader.Read())
            {
                forEach(reader.GetDataRow());

                _count++;
            }

            reader.Close();
        }

        /// <summary>
        /// start the data reader with parameters
        /// </summary>
        /// <param name="parameters"></param>
        public override void Start(IDictionary<string, object> parameters = null)
        {
            _count = 0;

            Excute(Post, parameters);

            Complete();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
