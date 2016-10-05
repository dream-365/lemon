using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class BlockBuilder
    {
        public static object CreateBufferBlock(Type type, DataflowBlockOptions options)
        {
            var bufferBlockClass = typeof(BufferBlock<>).MakeGenericType(type);

            return Activator.CreateInstance(bufferBlockClass, new object[] { options } );
        }

        public static object CreateActionBlock(Type type, object block, ExecutionDataflowBlockOptions options)
        {
            var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(typeof(IDictionary<string, object>));

            var actionBlockInstance = Activator.CreateInstance(actionBlockClass, new object[] { block, options });
            
            return actionBlockInstance;
        }

        public static object CreateTransformBlock(Type source, Type target, object block, ExecutionDataflowBlockOptions options)
        {
            var transformBlockClass = typeof(TransformBlock<,>).MakeGenericType(source, target);

            var transformBlockInstance = Activator.CreateInstance(transformBlockClass, new object[] { block, options });

            return transformBlockInstance;
        }
    }
}
