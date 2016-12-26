using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lemon.Data.Core
{
    public class Execution : IExecute
    {
        private Func<IDictionary<string, object>, Task<bool>> _block;

        public Execution(Func<IDictionary<string, object>, Task<bool>> block)
        {
            _block = block;
        }

        public bool Run(IDictionary<string, object> namedParameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RunAsync(IDictionary<string, object> namedParameters)
        {
            return _block(namedParameters);
        }
    }
}
