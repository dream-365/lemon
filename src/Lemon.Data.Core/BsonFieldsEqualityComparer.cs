using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public class BsonFieldsEqualityComparer : IEqualityComparer<BsonDocument>
    {
        private string[] _fieldsToCompare;

        public BsonFieldsEqualityComparer(string[] fieldsToCompare)
        {
            _fieldsToCompare = fieldsToCompare;
        }

        public bool Equals(BsonDocument x, BsonDocument y)
        {
            foreach(var column in _fieldsToCompare)
            {
                if(!x.GetValue(column).Equals(y.GetValue(column)))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(BsonDocument obj)
        {
            throw new NotSupportedException();
        }
    }
}
