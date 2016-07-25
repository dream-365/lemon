using Lemon.Transform.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;

namespace Lemon.Transform
{
    internal class JsonDataInputModel
    {
        public string Name { get; set; }

        public string Object { get; set; }

        public string ConnectionId { get; set; }

        public string SchemaId { get; set; }

        public string Filter { get; set; }

        public NamedParameter[] Parameters { get; set; }
    }

    internal class JsonDataOutputMoel
    {

    }

    public class JsonDataSourcesRepository : IDataSourcesRepository
    {
        private DataStoreModel _datastore;

        private class DataStoreModel
        {
            public Dictionary<string, DataConnection> Connections { get; set; }

            public Dictionary<string, DataTableSchema> Schemas { get; set; }

            public Dictionary<string, JsonDataInputModel> Inputs { get; set; }

            public Dictionary<string, JsonDataOutputMoel> Outputs { get; set; }
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
            DataInputModel model;

            if(_datastore.Inputs.TryGetValue(name, out model))
            {
                return model;
            }

            return null;
        }

        public DataOutputModel GetDataOutput(string name)
        {
            DataOutputModel model;

            if(_datastore.Outputs.TryGetValue(name, out model))
            {
                return model;
            }

            return null;
        }
    }
}
