using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Lemon.Transform
{
    public class MongoDataInput : AbstractDataInput
    {
        private string _connectionString;

        private string _databaseName;

        private string _collectionName;

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

        public MongoDataInput(DataInputModel model)
        {
            _connectionString = model.Connection;

            var temp = model.ObjectName.Split('.');

            _databaseName = temp[0];

            _collectionName = temp[1];

            _filter = model.Filter;
        }

        private void ForEach(Action<BsonDataRow> forEach)
        {
            var start = 0;

            bool hasMore = true;

            while (hasMore)
            {
                var list = _collection.Find(_filter)
                    .Skip(start)
                    .Limit(BAT_SZIE + 1)
                    .ToList();

                list.ForEach(m => forEach(new BsonDataRow(m)));

                if (list.Count <= BAT_SZIE)
                {
                    hasMore = false;
                }

                start = start + BAT_SZIE;
            }
        }

        private void Connect()
        {
            var client = new MongoClient(_connectionString);

            var database = client.GetDatabase(_databaseName);

            _collection = database.GetCollection<BsonDocument>(_collectionName);
        }

        public override void Start()
        {
            Connect();

            ForEach(Post);
        }
    }
}
