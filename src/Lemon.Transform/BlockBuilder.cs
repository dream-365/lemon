using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class BlockBuilder
    {
        public static DataflowBlockReflectionWrapper CreateBufferBlock(Type type, DataflowBlockOptions options)
        {
            var bufferBlockClass = typeof(BufferBlock<>).MakeGenericType(type);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(bufferBlockClass, new object[] { options } ));
        }

        public static DataflowBlockReflectionWrapper CreateActionBlock(Type type, object block, ExecutionDataflowBlockOptions options)
        {
            var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(type);
            
            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(actionBlockClass, new object[] { block, options }));
        }

        public static DataflowBlockReflectionWrapper CreateTransformBlock(Type source, Type target, object block, ExecutionDataflowBlockOptions options)
        {
            var transformBlockClass = typeof(TransformBlock<,>).MakeGenericType(source, target);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(transformBlockClass, new object[] { block, options }));
        }
    }
}
