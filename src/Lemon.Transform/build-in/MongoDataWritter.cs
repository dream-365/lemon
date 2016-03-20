using MongoDB.Bson;
using MongoDB.Driver;

namespace Lemon.Transform
{
    public class MongoDataWritter : ITransformDataWritter
    {
        private IMongoCollection<BsonDocument> _collection;

        public MongoDataWritter(IMongoCollection<BsonDocument> collection)
        {
            _collection = collection;
        }

        public string PrimaryKey
        {
            get
            {
                return "_id";
            }
        }

        public void Flush()
        {
            // DO NOTHING
        }

        public IValueSetter GetValueSetter(object id)
        {
            return new MongoValueSetter(_collection, id, PrimaryKey);
        }
    }
}
