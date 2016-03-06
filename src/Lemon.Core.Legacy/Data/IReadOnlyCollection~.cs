using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Data
{
    public interface IReadOnlyCollection<TRecord>
    {
        Task ForEachAsync(Func<TRecord, Task> processor);

        Task ForEachAsync(Action<TRecord> processor);
    }
}
