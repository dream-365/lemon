using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Lemon.Transform
{
    public class MongoDataReader : ITransformDataReader
    {
        private IMongoCollection<BsonDocument> _collection;

        private const int BAT_SZIE = 1000;

        private string _filter;

        public string PrimaryKey
        {
            get
            {
                return "_id";
            }
        }

        public MongoDataReader(IMongoCollection<BsonDocument> collection, string filter)
        {
            _collection = collection;

            _filter = filter;
        }

        public void ForEach(Action<IValueProvider> forEach)
        {
            var start = 0;

            bool hasMore = true;

            while(hasMore)
            {
                var list = _collection.Find(_filter)
                    .Skip(start)
                    .Limit(BAT_SZIE + 1)
                    .ToList();

                list.ForEach(m => forEach(new BsonDocumentValueProvider(m)));

                if(list.Count <= BAT_SZIE)
                {
                    hasMore = false;
                }

                start = start + BAT_SZIE;
            }
        }
    }
}
