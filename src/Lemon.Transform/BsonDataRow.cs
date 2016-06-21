using MongoDB.Bson;
using MongoDB.Bson.IO;
using System;

namespace Lemon.Transform
{
    public class BsonDataRow : ICloneable
    {
        private BsonDocument _internalDocument;

        public BsonDataRow()
        {
            _internalDocument.Clone();

            _internalDocument = new BsonDocument();
        }

        public BsonDataRow(BsonDocument document)
        {
            _internalDocument = document;
        }

        public void CombineWith(BsonDataRow row)
        {
            var document = row._internalDocument;

            foreach(var element in document.Elements)
            {
                _internalDocument.SetElement(element);
            }
        }

        public void SetValue(string name, BsonValue value)
        {
            _internalDocument.Set(name, value);
        }

        public BsonValue GetValue(string name)
        {
            BsonValue value;

            if (_internalDocument.TryGetValue(name, out value))
            {
                return value;
            }

            return BsonNull.Value;
        }

        public override string ToString()
        {
            return _internalDocument.ToJson(JsonWriterSettings.Defaults);
        }

        public object Clone()
        {
            return new BsonDataRow(_internalDocument.Clone().ToBsonDocument());
        }
    }
}
