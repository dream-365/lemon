using System;
using System.Threading.Tasks.Dataflow;
using Lemon.Core.Models;

namespace Lemon.Core
{
    internal class BlockBuilder
    {
        private static Type MakeMessageWrapperType(Type type)
        {
            return typeof(MessageWrapper<>).MakeGenericType(type);
        }

        public static DataflowBlockReflectionWrapper CreateBufferBlock(Type type, DataflowBlockOptions options)
        {
            var messageType = MakeMessageWrapperType(type);

            var bufferBlockClass = typeof(BufferBlock<>).MakeGenericType(messageType);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(bufferBlockClass, new object[] { options } ));
        }

        public static DataflowBlockReflectionWrapper CreateActionBlock(Type type, object block, ExecutionDataflowBlockOptions options)
        {
            var messageType = MakeMessageWrapperType(type);

            var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(messageType);

            var actionBlockMakerClass = typeof(MessageActionBlockMaker<>).MakeGenericType(type);

            var actionBlockInstance = Activator.CreateInstance(actionBlockMakerClass, new object[] { block });

            var blockWrapper = actionBlockMakerClass.GetProperty("Action").GetValue(actionBlockInstance);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(actionBlockClass, new object[] { blockWrapper, options }));
        }

        public static DataflowBlockReflectionWrapper CreateTransformBlock(Type source, Type target, object block, ExecutionDataflowBlockOptions options)
        {
            var sourceMessageType = MakeMessageWrapperType(source);

            var targetMessageType = MakeMessageWrapperType(target);

            var transformBlockClass = typeof(TransformBlock<,>).MakeGenericType(sourceMessageType, targetMessageType);

            var transformBlockMakerClass = typeof(MessageTransformBlockMaker<,>).MakeGenericType(source, target);

            var transformBlockMakerInstance = Activator.CreateInstance(transformBlockMakerClass, new object[] { block });

            var blockWrapper = transformBlockMakerClass.GetProperty("Transform").GetValue(transformBlockMakerInstance);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(transformBlockClass, new object[] { blockWrapper, options }));
        }

        public static DataflowBlockReflectionWrapper CreateTransformManyBlock(Type source, Type target, object expression, ExecutionDataflowBlockOptions options)
        {
            var sourceMessageType = MakeMessageWrapperType(source);

            var targetMessageType = MakeMessageWrapperType(target);

            var transformManyBlockClass = typeof(TransformManyBlock<,>).MakeGenericType(sourceMessageType, targetMessageType);

            var transformManyBlockMakerClass = typeof(MessageTransformManyBlockMaker<,>).MakeGenericType(source, target);

            var transformManyBlockMakerInstance = Activator.CreateInstance(transformManyBlockMakerClass, new object[] { expression });

            var blockWrapper = transformManyBlockMakerClass.GetProperty("TransformMany").GetValue(transformManyBlockMakerInstance);

            return new DataflowBlockReflectionWrapper(Activator.CreateInstance(transformManyBlockClass, new object[] { blockWrapper, options }));
        }
    }
}
