using System;

namespace Lemon.Data.Core
{
    public class DataSetCompareEngine
    {
        public IComareExecute<T> Compare<T>(DataSet<T> set, DataSet<T> with, CompareOptions options)
        {
            return new ComareExecutor<T>(set, with, options);
        }
    }
}
