using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class BsonDataRow
    {
        private BsonDocument _internalDocument;

        public BsonDataRow()
        {
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

        public IEnumerable<string> ColumnNames { get; private set; }

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
    }
}
