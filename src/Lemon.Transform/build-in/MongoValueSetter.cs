using MongoDB.Bson;
using MongoDB.Driver;

namespace Lemon.Transform
{
    public class MongoValueSetter : IValueSetter
    {
        private object _id;
    
        private IMongoCollection<BsonDocument> _collection;

        private UpdateDefinition<BsonDocument> _updateDefinition;

        private string _primaryKey;

        private bool _isUpsert;

        public MongoValueSetter(IMongoCollection<BsonDocument> collection, object id, string primaryKey, bool isUpsert = true)
        {
            _collection = collection;

            _id = id;

            _primaryKey = primaryKey;

            _isUpsert = isUpsert;
        }

        public void Apply()
        {
            var identity = Builders<BsonDocument>.Filter.Eq("_id", _id);

            var task = _collection.UpdateOneAsync(identity, _updateDefinition, new UpdateOptions { IsUpsert = _isUpsert });

            task.Wait();
        }

        public void SetValue(string name, BsonValue value)
        {
            if(_updateDefinition == null)
            {
                _updateDefinition = Builders<BsonDocument>.Update.Set(name, value);
            }
            else
            {
                _updateDefinition = _updateDefinition.Set(name, value);
            }
        }
    }
}
