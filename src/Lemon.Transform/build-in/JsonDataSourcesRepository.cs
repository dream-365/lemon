using Lemon.Transform.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;

namespace Lemon.Transform
{
    internal class DataInputModelProxy
    {
        public string Object { get; set; }

        public string ConnectionId { get; set; }

        public string SchemaId { get; set; }

        public string Filter { get; set; }

        public NamedParameter[] Parameters { get; set; }
    }

    internal class DataOutputModelProxy
    {
        public string Object { get; set; }

        public string ConnectionId { get; set; }

        public string SchemaId { get; set; }

        public bool IsUpsert { get; set; }

        public WriteOnChangeConfiguration WriteOnChange { get; set; }
    }

    public class JsonDataSourcesRepository : IDataSourcesRepository
    {
        private DataStoreModel _datastore;

        private class DataStoreModel
        {
            public Dictionary<string, DataConnection> Connections { get; set; }

            public Dictionary<string, DataTableSchema> Schemas { get; set; }

            public Dictionary<string, DataInputModelProxy> Inputs { get; set; }

            public Dictionary<string, DataOutputModelProxy> Outputs { get; set; }
        }

        public JsonDataSourcesRepository()
        {
            if(_datastore == null)
            {
                var text = File.ReadAllText("_local_model_store_.json");

                _datastore = JsonConvert
                    .DeserializeObject<DataStoreModel>(
                    text,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }
        }

        public DataInputModel GetDataInput(string name)
        {
            DataInputModelProxy proxy;

            if(_datastore.Inputs.TryGetValue(name, out proxy))
            {
                var model = new DataInputModel
                {
                    Name = name,
                    Filter = proxy.Filter,
                    Object = proxy.Object,
                    Parameters = proxy.Parameters,
                    Schema = FindSchemaById(proxy.SchemaId),
                    Connection = FindDataConnectionById(proxy.ConnectionId)
                };

                return model;
            }

            throw new System.Exception("Can not find the data input " + name);
        }

        public DataOutputModel GetDataOutput(string name)
        {
            DataOutputModelProxy proxy;

            if(_datastore.Outputs.TryGetValue(name, out proxy))
            {
                var model = new DataOutputModel
                {
                    Name = name,
                    IsUpsert = proxy.IsUpsert,
                    Object = proxy.Object,
                    WriteOnChange = proxy.WriteOnChange,
                    Schema = FindSchemaById(proxy.SchemaId),
                    Connection = FindDataConnectionById(proxy.ConnectionId)
                };

                return model;
            }

            throw new System.Exception("Can not find the data output " + name);
        }

        private DataTableSchema FindSchemaById(string schemaId)
        {
            DataTableSchema schema;

            if(_datastore.Schemas.TryGetValue(schemaId, out schema))
            {
                return schema;
            }

            throw new System.Exception("Can not find the schema " + schemaId);
        }

        private DataConnection FindDataConnectionById(string connectionId)
        {
            DataConnection connection;

            if(_datastore.Connections.TryGetValue(connectionId, out connection))
            {
                return connection;
            }

            throw new System.Exception("Can not find the connection " + connectionId);
        }

    }
}
