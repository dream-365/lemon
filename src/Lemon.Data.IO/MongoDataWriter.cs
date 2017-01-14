using System.Collections.Generic;
using Lemon.Data.Core;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lemon.Data.IO
{
    public class MongoDataWriter : IDataWriter<BsonDocument>
    {
        private IMongoCollection<BsonDocument> _collection;

        public MongoDataWriter(string connectionString, string database, string collectionName)
        {
            var client = new MongoClient(connectionString);

            _collection = client.GetDatabase(database).GetCollection<BsonDocument>(collectionName);
        }

        public void Dispose()
        {
            
        }

        public void Write(IEnumerable<BsonDocument> records)
        {
            _collection.InsertMany(records);
        }

        public void Write(BsonDocument record)
        {
            _collection.InsertOne(record);
        }
    }
}
