using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public static class DataReaderExtension
    {
        public static IEnumerator<T> AsEnumerator<T>(this IDataReader<T> reader)
        {
            return new DataEnumerator<T>(reader);
        }
    }
}
