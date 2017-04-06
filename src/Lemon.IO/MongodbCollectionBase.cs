using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Lemon.IO
{
    public class MongodbCollectionBase
    {
        protected readonly string _connectionString;
        protected readonly string _collectionName;
        protected readonly string _databaseName;
        protected IMongoCollection<BsonDocument> _collection;

        public MongodbCollectionBase(
            string connectionString,
            string databaseName,
            string collectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionString) ||
               string.IsNullOrWhiteSpace(databaseName) ||
               string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentNullException("connectionString, databaseName and collectionName could not be null or empty");
            }
            _connectionString = connectionString;
            _collectionName = collectionName;
            _databaseName = databaseName;
            Initialize();
        }

        private void Initialize()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);
            _collection = database.GetCollection<BsonDocument>(_collectionName);
        }
    }
}
