using Lemon.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Configuration;

namespace Lemon.Storage
{
    public class MongoDBPersistence : IDocumentPersistence
    {
        public void Persist(BsonDocument document)
        {
            document.Set("timestamp", DateTime.Now);

            var id = Builders<BsonDocument>.Filter.Eq("_id", document.GetValue("_id"));

            _collection.DeleteOneAsync(id).Wait();

            _collection.InsertOneAsync(document).Wait();
        }


        private IMongoCollection<BsonDocument> _collection;

        private static MongoClient _client;

        private static object _lock = new object();

        protected MongoClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (_lock)
                    {
                        var connectionString = ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString;

                        _client = new MongoClient(connectionString);
                    }
                }

                return _client;
            }
        }

        public MongoDBPersistence(string name)
        {
            var temp = name.Split('.');

            var dbName = temp[0];

            var collectionName = temp[1];

            _collection = Client.GetDatabase(dbName).GetCollection<BsonDocument>(collectionName);
        }
    }
}
