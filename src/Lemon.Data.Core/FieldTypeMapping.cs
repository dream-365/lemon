using System;

namespace Lemon.Data.Core
{
    internal class FieldTypeMapping
    {
        public int Ordinal { get; set; }

        public string FieldName { get; set; }

        public Type DataType { get; set; }
    }
}
