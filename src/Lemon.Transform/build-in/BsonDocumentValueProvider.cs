using MongoDB.Bson;

namespace Lemon.Transform
{
    public class BsonDocumentValueProvider : IValueProvider
    {
        private BsonDocument _document;

        public BsonDocumentValueProvider(BsonDocument document)
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
