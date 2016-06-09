using MongoDB.Bson;
using System;

namespace Lemon.Transform
{
    public class CalculationColumnDefinition
    {
        public string TargetColumnName { get; set; }

        public Func<BsonDataRow, BsonValue> CalculateFunction { get; set; }
    }
}
