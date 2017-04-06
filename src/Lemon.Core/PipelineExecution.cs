using System;
using System.Threading.Tasks;

namespace Lemon.Core
{
    public class PipelineExecution : IExecuteable
    {
        private Func<Task<bool>> _block;

        public PipelineExecution(Func<Task<bool>> block)
        {
            _block = block;
        }

        public Task<bool> RunAsync()
        {
            return _block();
        }
    }
}
