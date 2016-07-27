using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Lemon.Transform
{
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

            var indexOfFirstColon = model.Object.IndexOf(':');

            var objectType = model.Object.Substring(0, indexOfFirstColon).Trim();

            var objectContent = model.Object.Substring(indexOfFirstColon + 1).Trim();

            if(objectType == "table")
            {
                var sb = new StringBuilder();

                var firstColumnName = model.Schema.Columns.First().Name;

                sb.AppendLine("SELECT [" + firstColumnName + "]");

                foreach(var column in model.Schema.Columns.Skip(1))
                {
                    sb.AppendLine(",[" + column.Name + "]");
                }

                sb.AppendLine("FROM [" + objectContent + "]");

                if(!string.IsNullOrWhiteSpace(model.Filter))
                {
                    sb.AppendLine(model.Filter);
                }

                _sql = sb.ToString();
            }
            else if(objectType == "sqlfile")
            {
                var sql = SqlNamedQueryProvider.Instance.Get(objectContent);

                if(!string.IsNullOrWhiteSpace(model.Filter))
                {
                    _sql = "SELECT * FROM (" + sql + ") [generated_sql_temp] " + model.Filter;
                }
            }
            else
            {
                throw new NotSupportedException("object type " + objectType + " is not supported by Microsoft.SqlServer intput");
            }

            foreach(var parameter in model.Parameters)
            {
                _parameters.Add(parameter.Name, parameter.Value);
            }

            _connection = new SqlConnection(_connectionString);
        }
        
        public void ForEach(Action<BsonDataRow> forEach)
        {
            var parameters = FillParameters(_parameters);

            SqlCommand command = new SqlCommand(_sql, _connection);

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value));
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

        public override void Start()
        {
            _count = 0;

            ForEach(Post);

            Complete();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
