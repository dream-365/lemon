using MongoDB.Bson;
using System;

namespace Lemon.Transform
{
    internal class CastUtil
    {
        /// <summary>
        /// cast the BsonValue to .NET object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Cast(BsonValue value)
        {
            object dotNetObject;

            switch (value.BsonType)
            {
                case BsonType.Int32: dotNetObject = value.AsInt32; break;
                case BsonType.Int64: dotNetObject = value.AsInt64; break;
                case BsonType.String: dotNetObject = value.AsString; break;
                case BsonType.Double: dotNetObject = value.AsDouble; break;
                case BsonType.Boolean: dotNetObject = value.AsBoolean; break;
                case BsonType.DateTime: dotNetObject = value.ToLocalTime(); break;
                case BsonType.Null: dotNetObject = null; break;
                default: throw new NotSupportedException(string.Format("the bson type [{0}] is not supported", value.BsonType));
            }

            return dotNetObject;
        }

        /// <summary>
        /// cast the .NET object to BsonValue
        /// </summary>
        /// <param name="dotNetValue"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static BsonValue Cast(object dotNetValue, Type sourceType)
        {
            if (sourceType == ObjectType.String)
            {
                return new BsonString(dotNetValue as string);
            }
            else if (sourceType == ObjectType.Int32)
            {
                return new BsonInt32((int)dotNetValue);
            }
            else if (sourceType == ObjectType.Int64)
            {
                return new BsonInt64((Int64)dotNetValue);
            }
            else if (sourceType == ObjectType.Boolean)
            {
                return new BsonBoolean((bool)dotNetValue);
            }
            else if (sourceType == ObjectType.DateTime)
            {
                return new BsonDateTime((DateTime)dotNetValue);
            }

            throw new NotSupportedException(string.Format("Unable to cast type {0} to BsonValue", sourceType.FullName));
        }
    }
}
