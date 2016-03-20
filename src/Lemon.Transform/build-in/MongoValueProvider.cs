using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class MongoValueProvider : IValueProvider
    {
        private BsonDocument _document;

        public MongoValueProvider(BsonDocument document)
        {
            _document = document;
        }

        public BsonValue GetValue(string name)
        {
            BsonValue value;

            if(_document.TryGetValue(name, out value))
            {
                return value;
            }

            return BsonNull.Value;
        }
    }
}
