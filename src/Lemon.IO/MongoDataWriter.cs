using System.Collections.Generic;
using Lemon.Data.Core;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lemon.IO
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
            throw new System.NotImplementedException();
        }

        public void Write(BsonDocument record)
        {
            _collection.ReplaceOne(
                Builders<BsonDocument>.Filter.Eq("_id", record.GetValue("_id")),
                record,
                new UpdateOptions { IsUpsert = true });
        }
    }
}
