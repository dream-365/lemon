using System;
using System.Threading.Tasks.Dataflow;
using Lemon.Data.Core.Models;

namespace Lemon.Data.Core
{
    public class BlockBuilder
    {
        public static DataflowBlockReflectionWrapper CreateBufferBlock(Type type, DataflowBlockOptions options)
        {
            var messageType = typeof(MessageWrapper<>).MakeGenericType(type);

            var bufferBlockClass = typeof(BufferBlock<>).MakeGenericType(messageType);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(bufferBlockClass, new object[] { options } ));
        }

        public static DataflowBlockReflectionWrapper CreateActionBlock(Type type, object block, ExecutionDataflowBlockOptions options)
        {
            var messageType = typeof(MessageWrapper<>).MakeGenericType(type);

            var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(messageType);

            var actionBlockMakerClass = typeof(MessageActionBlockMaker<>).MakeGenericType(type);

            var actionBlockInstance = Activator.CreateInstance(actionBlockMakerClass, new object[] { block });

            var blockWrapper = actionBlockMakerClass.GetProperty("Action").GetValue(actionBlockInstance);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(actionBlockClass, new object[] { blockWrapper, options }));
        }

        public static DataflowBlockReflectionWrapper CreateTransformBlock(Type source, Type target, object block, ExecutionDataflowBlockOptions options)
        {
            var sourceMessageType = typeof(MessageWrapper<>).MakeGenericType(source);

            var targetMessageType = typeof(MessageWrapper<>).MakeGenericType(target);

            var transformBlockClass = typeof(TransformBlock<,>).MakeGenericType(sourceMessageType, targetMessageType);

            var transformBlockMakerClass = typeof(MessageTransformBlockMaker<,>).MakeGenericType(source, target);

            var transformBlockMakerInstance = Activator.CreateInstance(transformBlockMakerClass, new object[] { block });

            var blockWrapper = transformBlockMakerClass.GetProperty("Transform").GetValue(transformBlockMakerInstance);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(transformBlockClass, new object[] { blockWrapper, options }));
        }
    }
}
