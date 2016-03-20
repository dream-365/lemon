using MongoDB.Bson;
using System;

namespace Lemon.Transform
{
    public class TransformColumnDefinition
    {
        private string _targetColumnName;

        public string SourceColumnName { get; set; }

        public string TargetColumnName {
            get {
                if(string.IsNullOrWhiteSpace(_targetColumnName))
                {
                    return SourceColumnName;
                }

                return _targetColumnName;
            }

            set
            {
                _targetColumnName = value;
            }
        }

        public Func<BsonValue, BsonValue> TransformFunction { get; set; }
    }
}
