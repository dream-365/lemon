using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class JsonDataStoreService : IDataStoreService
    {
        private DataStoreModel _datastore;

        private class DataStoreModel
        {
            public Dictionary<string, DataInputModel> Inputs { get; set; }

            public Dictionary<string, DataOutputModel> Outputs { get; set; }
        }

        public JsonDataStoreService()
        {
            if(_datastore == null)
            {
                var text = File.ReadAllText("_local_model_sotre_.json");

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
