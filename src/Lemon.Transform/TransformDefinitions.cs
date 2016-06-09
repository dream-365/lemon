using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public abstract class TransformDefinitions
    {
        protected IList<TransformColumnDefinition> _transformColumnDefinitions;

        protected IList<CalculationColumnDefinition> _calculationColumnDefinitions;

        public TransformDefinitions()
        {
            _transformColumnDefinitions = new List<TransformColumnDefinition>();

            _calculationColumnDefinitions = new List<CalculationColumnDefinition>();

            Build();
        }

        protected abstract void Build();

        protected void Copy(string source, string target = null)
        {
            _transformColumnDefinitions.Add(new TransformColumnDefinition { SourceColumnName = source, TargetColumnName = target });
        }

        protected void Transform(string source, Func<BsonValue, BsonValue> func, string target = null)
        {
            _transformColumnDefinitions.Add(new TransformColumnDefinition { SourceColumnName = source, TargetColumnName = target, TransformFunction = func });
        }

        protected void Calculate(string name, Func<BsonDataRow, BsonValue> func)
        {
            _calculationColumnDefinitions.Add(new CalculationColumnDefinition { TargetColumnName = name, CalculateFunction = func });
        }


        public IEnumerable<TransformColumnDefinition> GetTransformColumnDefinitions()
        {
            return _transformColumnDefinitions;
        }

        public IEnumerable<CalculationColumnDefinition> GetCalculationColumnDefinitions()
        {
            return _calculationColumnDefinitions;
        }
    }
}
