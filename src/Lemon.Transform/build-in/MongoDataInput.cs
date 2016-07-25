using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemon.Transform
{
    public class MongoDataInput : AbstractDataInput
    {
        private string _connectionString;

        private string _databaseName;

        private string _collectionName;

        private bool _limitSpeed;

        private IMongoCollection<BsonDocument> _collection;     

        private int _batchSize = 1000;

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
            var dictionary = new Dictionary<string, string>();

            var attributes = model.Connection.ConnectionString.Split(';');

            foreach (var attribute in attributes)
            {
                var splits = attribute.Split('=');

                var key = splits[0];

                var value = splits[1];

                dictionary.Add(key, value);
            }

            _connectionString = dictionary["Data Source"];

            _limitSpeed = dictionary.ContainsKey("Speed");

            if (_limitSpeed)
            {
                _batchSize = int.Parse(dictionary["Speed"]);
            }

            var temp = model.Schema.ObjectName.Split('.');

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
                    .Limit(_batchSize + 1)
                    .ToList();

                list.ForEach(m => forEach(new BsonDataRow(m)));

                if (list.Count <= _batchSize)
                {
                    hasMore = false;
                }

                start = start + _batchSize;

                if(_limitSpeed)
                {
                    System.Threading.Thread.Sleep(1000);
                }
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

            Complete();
        }
    }
}
