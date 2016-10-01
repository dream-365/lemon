using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                PrametersInfo.RegisterParameters(model.Parameters);
            }
        }

        /// <summary>
        /// apply parameter expressions to mongo
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string ApplyParameters(string text, IDictionary<string, object> parameters)
        {
            foreach(var parameter in parameters)
            {
                string expression = null;

                if(parameter.Value.GetType() == typeof(DateTime))
                {
                    var utc = ((DateTime)parameter.Value).ToUniversalTime();

                    expression = string.Format("new ISODate('{0:O}')", utc);
                }
                else if (parameter.Value.GetType() == typeof(string))
                {
                    expression = string.Format("'{0}'", parameter.Value);
                }else if (parameter.Value.GetType() == typeof(int))
                {
                    expression = parameter.Value.ToString();
                }

                if(expression == null)
                {
                    throw new ArgumentException("parameter type [" + parameter.Value.GetType() + "] is not supported for mongo");
                }

                text = text.Replace("@" + parameter.Key, expression);
            }

            return text;
        }

        /// <summary>
        /// excute the expression, only the string type paramaters will apply this
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string ExecuteExpressions(string text, IDictionary<string, object> parameters)
        {
            foreach(var parameter in parameters)
            {
                if(parameter.Value.GetType() == typeof(string))
                {
                    text = text.Replace("{{" + parameter.Key + "}}", parameter.Value as string);
                }
            }

            return text;
        }

        private async Task Execute(Func<BsonDataRow, Task<bool>> forEach, IDictionary<string, object> parameters)
        {
            var executeParameters = PrametersInfo.ValidateParameters(parameters);

            string collectionName = executeParameters == null
                                    ? _collectionName
                                    : ExecuteExpressions(_collectionName, executeParameters);

            string filter = executeParameters == null 
                        ? _filter
                        : ApplyParameters(_filter, executeParameters);

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

                await find.ForEachAsync(async m => await forEach(new BsonDataRow(m)));

                if (find.Count() < _batchSize)
                {
                    hasMore = false;
                }

                start = start + _batchSize;
            }
        }

        public override async Task StartAsync(IDictionary<string, object> parameters = null)
        {
            await Execute(SendAsync, parameters);

            Complete();
        }
    }
}
