using System.Collections.Generic;

namespace Lemon.Transform
{
    public class TransformObject
    {
        public ITransformDataReader DataReader { get; set; }

        public ITransformDataWritter DataWritter { get; set; }

        public IEnumerable<TransformColumnDefinition> TransformColumnDefinitions { get; set; }

        public IEnumerable<CalculationColumnDefinition> CalculationColumnDefinition { get; set; }
    }
}
