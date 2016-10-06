using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class MessageTransformBlock<TInput, TOuput> : ITransformBlock<TInput, TOuput>
    {
        private Func<TInput, TOuput> _func;

        public MessageTransformBlock(Func<TInput, TOuput> func)
        {
            _func = func;
        }

        public TOuput Transform(TInput record)
        {
            return _func(record);
        }
    }
}
