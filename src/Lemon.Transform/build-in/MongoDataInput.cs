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

        private string[] _columnNames;

        private int _batchSize = 1000;

        private string _filter;

        private IDictionary<string, string> _parameters = new Dictionary<string, string>();

        public MongoDataInput(DataInputModel model)
        {
            _connectionString = model.Connection.ConnectionString;

            var temp = model.Object.Split('.');

            _databaseName = temp[0];

            _collectionName = temp[1];

            _filter = string.IsNullOrWhiteSpace(model.Filter)
                        ? "{}"
                        : model.Filter;

            if(model.Schema != null)
            {
                _columnNames = model.Schema.Columns.Select(m => m.Name).ToArray();
            }         

            if(model.Parameters != null)
            {
                foreach (var parameter in model.Parameters)
                {
                    _parameters.Add(parameter.Name, parameter.Value);
                }
            } 
        }

        private void ForEach(Action<BsonDataRow> forEach)
        {
            var parameters = FillParameters(_parameters);

            string collectionName = _collectionName;

            string filter = _filter;

            foreach (var parameter in parameters)
            {
                filter = filter.Replace("{{" + parameter.Key + "}}", parameter.Value as string);

                collectionName = collectionName.Replace("{{" + parameter.Key + "}}", parameter.Value as string);
            }

            var client = new MongoClient(_connectionString);

            var database = client.GetDatabase(_databaseName);

            var collection = database.GetCollection<BsonDocument>(collectionName);

            var start = 0;

            bool hasMore = true;

            ProjectionDefinition<BsonDocument> projection = null;

            if(_columnNames != null && _columnNames.Count() > 0)
            {
                var firstColumnName = _columnNames.First();

                projection = Builders<BsonDocument>.Projection.Include(firstColumnName);

                foreach (var columnName in _columnNames.Skip(1))
                {
                    projection = projection.Include(columnName);
                }
            }
                                   
            while (hasMore)
            {
                var find = collection.Find(filter)
                    .Skip(start)
                    .Limit(_batchSize + 1);

                if(projection != null)
                {
                    find = find.Project(projection);
                }

                var list = find.ToList();

                list.ForEach(m => forEach(new BsonDataRow(m)));

                if (list.Count <= _batchSize)
                {
                    hasMore = false;
                }

                start = start + _batchSize;
            }
        }

        public override void Start()
        {
            ForEach(Post);

            Complete();
        }
    }
}
