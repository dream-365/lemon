using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lemon.Core
{
    public interface IExecute
    {
        Task<bool> RunAsync(IDictionary<string, object> namedParameters);

        bool Run(IDictionary<string, object> namedParameters);
    }
}
