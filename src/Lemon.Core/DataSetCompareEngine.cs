using System;

namespace Lemon.Core
{
    [Obsolete]
    public class DataSetCompareEngine
    {
        public IComareExecute<T> Compare<T>(DataSet<T> set, DataSet<T> with, CompareOptions options)
        {
            return new CompareExecutor<T>(set, with, options);
        }
    }
}
