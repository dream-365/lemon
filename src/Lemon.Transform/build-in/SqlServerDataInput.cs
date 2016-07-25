using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Lemon.Transform
{
    public class SqlServerDataInput : AbstractDataInput, IDisposable
    {
        private SqlConnection _connection;

        private string _connectionString;

        private string _sql;

        private long _count;

        private IDictionary<string, object> _parameters;

        public SqlServerDataInput(DataInputModel model)
        {
            var dictionary = new Dictionary<string, string>();

            _connectionString = model.Connection.ConnectionString;

            _sql = SqlNamedQueryProvider.Instance.Get(model.Schema.ObjectName);

            _parameters = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(model.Filter);

            _connection = new SqlConnection(_connectionString);
        }
        
        public void ForEach(Action<BsonDataRow> forEach)
        {
            SqlCommand command = new SqlCommand(_sql, _connection);

            foreach(var parameter in _parameters)
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
