using System;

namespace Lemon.Data.IO.Azure
{
    internal class Assert
    {
        public static void AreEqual(object expected, object actual)
        {
            if (expected == null)
            {
                throw new ArgumentNullException();
            }

            if (!expected.Equals(actual))
            {
                string assertMessage = string.Format("expect the slibing node name = {0}, but the actual node name = {1}", expected, actual);
                throw new Exception(assertMessage);
            }
        }

        public static void IsNotNull(object value, string fieldName)
        {
            if (value == null)
            {
                throw new Exception(string.Format("filed {0} value can not be null", fieldName));
            }
        }

        public static void IsNotNullOrWhiteSpace(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception(string.Format("filed {0} value can not be null, empty or white space", fieldName));
            }
        }
    }
}
