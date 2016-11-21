using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public interface IDataWriter<TRecord> : System.IDisposable
    {
        void Write(TRecord record);
        void Write(IEnumerable<TRecord> records);
    }
}
