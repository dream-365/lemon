using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class ValueExpression
    {
        private string _expression;

        public ValueExpression(string expression)
        {
            _expression = expression;
        }

        public object Value
        {
            get
            {
                return Eavaluate();
            }
        }

        private object Eavaluate()
        {
            if(string.IsNullOrWhiteSpace(_expression))
            {
                return null;
            }

            var idx = _expression.IndexOf(':');

            var dataType = _expression.Substring(0, idx);

            string value = _expression.Substring(idx + 1);

            switch (dataType)
            {
                case "string": return value;
                case "datetime": return DateTime.Parse(value);
                case "int": return int.Parse(value);
                case "double": return double.Parse(value);
                default:
                    throw new NotSupportedException("data type " + dataType + " is not a supported parameter data type");
            }
        }
    }
}
