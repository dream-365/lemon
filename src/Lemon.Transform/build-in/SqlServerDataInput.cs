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

        private bool _limitSpeed;

        private long _speed;

        private long _count;

        private IDictionary<string, object> _parameters;

        public SqlServerDataInput(DataInputModel model)
        {
            var dictionary = new Dictionary<string, string>();

            var attributes = model.Connection.Split(';');

            foreach (var attribute in attributes)
            {
                var splits = attribute.Split('=');

                var key = splits[0];

                var value = splits[1];

                dictionary.Add(key, value);
            }

            _limitSpeed = dictionary.ContainsKey("Speed");

            if(_limitSpeed)
            {
                dictionary.Remove("Speed");
            }

            _connectionString = string.Join(";", dictionary.Select(m => string.Format("{0}={1}", m.Key, m.Value)));

            _sql = SqlNamedQueryProvider.Instance.Get(model.ObjectName);

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

                if (_limitSpeed && (_count % _speed) == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                }
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
