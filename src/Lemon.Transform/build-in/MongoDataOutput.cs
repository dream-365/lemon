using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public class MongoDataOutput : IDataOutput
    {
        private string _connectionString;

        private string _databaseName;

        private string _collectionName;

        private IMongoCollection<BsonDocument> _collection;

        private IEnumerable<string> _columnNames;


        public MongoDataOutput(DataOutputModel model)
        {
            _columnNames = model.ColumnNames;

            _connectionString = model.Connection;

            var temp = model.ObjectName.Split('.');

            _databaseName = temp[0];

            _collectionName = temp[1];

            Connect();
        }

        private void Connect()
        {
            var client = new MongoClient(_connectionString);

            var database = client.GetDatabase(_databaseName);

            _collection = database.GetCollection<BsonDocument>(_collectionName);
        }

        public void Input(BsonDataRow inputRow)
        {
            var identity = Builders<BsonDocument>.Filter.Eq("_id", inputRow.GetValue("_id"));

            UpdateDefinition<BsonDocument> updateDefinition = null;

            foreach (var columName in _columnNames)
            {
                if(updateDefinition == null)
                {
                    updateDefinition = Builders<BsonDocument>.Update.Set(columName, inputRow.GetValue(columName));
                }
                else
                {
                    updateDefinition = updateDefinition.Set(columName, inputRow.GetValue(columName));
                }
            }

            var task = _collection.UpdateOneAsync(identity, updateDefinition, new UpdateOptions { IsUpsert = true });

            task.Wait();
        }
    }
}
