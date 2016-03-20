using MongoDB.Bson;
using System;

namespace Lemon.Transform
{
    public class CalculationColumnDefinition
    {
        public string TargetColumnName { get; set; }

        public Func<IValueProvider, BsonValue> CalculateFunction { get; set; }
    }
}
