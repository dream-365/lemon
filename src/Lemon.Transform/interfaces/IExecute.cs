using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface IExecute
    {
        Task<bool> RunAsync(IDictionary<string, object> namedParameters);
    }
}
