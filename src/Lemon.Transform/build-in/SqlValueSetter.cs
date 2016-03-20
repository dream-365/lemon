using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Lemon.Transform
{
    public class SqlValueSetter : IValueSetter
    {
        internal IDictionary<string, object> _values;

        private SqlDataWritter _writter;

        public SqlValueSetter(SqlDataWritter writter, object id, string primaryKey)
        {
            _values = new Dictionary<string, object>();

            _values.Add(primaryKey, id);

            _writter = writter;
        }

        public void Apply()
        {
            _writter.Add(_values);
        }

        public void SetValue(string name, BsonValue value)
        {
            object dotNetObject;

            switch(value.BsonType)
            {
                case BsonType.Int32: dotNetObject = value.AsInt32; break;
                case BsonType.Int64: dotNetObject = value.AsInt64; break;
                case BsonType.String: dotNetObject = value.AsString; break;
                case BsonType.Double: dotNetObject = value.AsDouble; break;
                case BsonType.Boolean: dotNetObject = value.AsBoolean; break;
                case BsonType.DateTime: dotNetObject = value.ToUniversalTime(); break;
                case BsonType.Null: dotNetObject = null; break;
                default: throw new NotSupportedException(string.Format("the bson type [{0}] is not supported", value.BsonType));
            }

            _values.Add(name, dotNetObject);
        }
    }
}
