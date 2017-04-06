using MongoDB.Bson;
using System.Collections.Generic;

namespace Lemon.Core
{
    public class BsonPrimaryKeyComparer : IComparer<BsonDocument>
    {
        private string _primaryKey;

        public BsonPrimaryKeyComparer(string primaryKey)
        {
            _primaryKey = primaryKey;
        }

        public int Compare(BsonDocument x, BsonDocument y)
        {
            var left = x.GetValue(_primaryKey);
            var right = y.GetValue(_primaryKey);

            return left.CompareTo(right);
        }
    }
}
