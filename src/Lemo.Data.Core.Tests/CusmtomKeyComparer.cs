using System.Collections.Generic;

namespace Lemo.Data.Core.Tests
{
    public class CusmtomKeyComparer : IComparer<DataRow>
    {
        public CusmtomKeyComparer(string primaryKey)
        {

        }

        public int Compare(DataRow x, DataRow y)
        {
            return 0;
        }
    }
}
