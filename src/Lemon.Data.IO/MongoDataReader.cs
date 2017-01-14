using Lemon.Data.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Lemon.Data.IO
{
    public class MongodbDataReader : MongodbCollectionBase, IDataReader<BsonDocument>
    {
        long _count;

        private IEnumerator<BsonDocument> _enumerator;
        public MongodbDataReader(
            string connectionString,
            string databaseName,
            string collectionName,
            string filter = "{}") : base(connectionString, databaseName, collectionName)
        {
            _enumerator = _collection.Find(filter).ToEnumerable().GetEnumerator();
            _count = 0;
        }

        public void Dispose()
        {
            LogService.Default.Info(string.Format("Read: {0}", _count));
        }

        public bool Next()
        {
            _count++;
            return _enumerator.MoveNext();
        }

        public BsonDocument Read()
        {
            return _enumerator.Current;
        }

        object IDataReader.Read()
        {
            return Read();
        }
    }
}
