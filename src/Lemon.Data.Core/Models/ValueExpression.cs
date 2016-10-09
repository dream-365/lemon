using Newtonsoft.Json;
using System;

namespace Lemon.Data.Core
{
    /// <summary>
    /// a class to represent value with data type specified
    /// </summary>
    public class ValueObject
    {
        private string _text;

        private string _dataType;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public ValueObject(string text, string dataType)
        {
            _text = text;

            _dataType = dataType;
        }

        [JsonIgnore]
        public object Value
        {
            get
            {
                return Eavaluate();
            }
        }

        public static ValueObject Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var idx = text.IndexOf(':');

            if(idx < 0)
            {
                throw new Exception("can not find the data type specified");
            }

            var dataType = text.Substring(0, idx);

            string value = text.Substring(idx + 1);

            return new ValueObject(value, dataType);
        }

        private object Eavaluate()
        {
            if(string.IsNullOrWhiteSpace(_text))
            {
                return null;
            }

            switch (_dataType)
            {
                case "string": return _text;
                case "datetime": return DateTime.Parse(_text);
                case "int": return int.Parse(_text);
                case "double": return double.Parse(_text);
                default:
                    throw new NotSupportedException("data type " + _text + " is not a supported parameter data type");
            }
        }
    }
}
