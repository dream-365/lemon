using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lemon.Data.Core
{
    public interface IExecute
    {
        Task<bool> RunAsync(IDictionary<string, object> namedParameters);
    }
}
