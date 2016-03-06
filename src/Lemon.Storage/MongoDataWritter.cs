using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ProcessingService
{
    class MongoDataWritter
    {
        private IMongoCollection<BsonDocument> _collection;

        private static MongoClient _client;

        private static object _lock = new object();

        protected MongoClient Client { get
            {
                if(_client == null)
                {
                    lock(_lock)
                    {
                        var connectionString = ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString;

                        _client = new MongoClient(connectionString);
                    }
                }

                return _client;
            }
        }

        public MongoDataWritter(string name)
        {
            var temp = name.Split('.');

            var dbName = temp[0];

            var collectionName = temp[1];

            _collection = Client.GetDatabase(dbName).GetCollection<BsonDocument>(collectionName);
        }

        public void Save(IDictionary<string, object> data)
        {
            var id = Builders<BsonDocument>.Filter.Eq("_id", data["_id"] as string);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            using (var jsonReader = new JsonReader(jsonString))
            {
                var context = BsonDeserializationContext.CreateRoot(jsonReader);

                var document = _collection.DocumentSerializer.Deserialize(context);

                document.Set("timestamp", DateTime.Now);

                _collection.DeleteOneAsync(id).Wait();

                _collection.InsertOneAsync(document).Wait();
            }
        }
    }
}
