using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Core
{
    public class DataflowBlockReflectionWrapper
    {
        private const string TARGET_BLOCK_INTERFACE_NAME = "ITargetBlock";
        private const string SOURCE_BLOCK_INTERFACE_NAME = "ISourceBlock";

        private object _block;

        private MethodExecutorBuilder.MethodExecutor _sendMethodExecutor;

        public DataflowBlockReflectionWrapper(object obj)
        {
            if (obj.GetType() == typeof(DataflowBlockReflectionWrapper))
            {
                _block = (obj as DataflowBlockReflectionWrapper)._block;
            } else
            {
                _block = obj;
            }
        }

        public IDisposable LinkTo(DataflowBlockReflectionWrapper targetBlock, DataflowLinkOptions options)
        {
            return LinkTo(targetBlock._block, options);
        }

        public IDisposable LinkTo(object targetBlock, DataflowLinkOptions options)
        {
            if (targetBlock.GetType() == typeof(DataflowBlockReflectionWrapper))
            {
                return LinkTo(targetBlock as DataflowBlockReflectionWrapper, options);
            }

            var sourceInterfaceType = _block.GetType()
                .GetInterfaces()
                .Where(m => m.Name.StartsWith(SOURCE_BLOCK_INTERFACE_NAME))
                .FirstOrDefault();

            if (sourceInterfaceType == null)
            {
                throw new NotImplementedException(string.Format("type {0} does not implemented {1} interface", _block.GetType(), SOURCE_BLOCK_INTERFACE_NAME));
            }

            var targetInterfaceType = targetBlock.GetType()
                                        .GetInterfaces()
                                        .Where(m => m.Name.StartsWith(TARGET_BLOCK_INTERFACE_NAME))
                                        .FirstOrDefault();

            if (targetInterfaceType == null)
            {
                throw new NotImplementedException(string.Format("type {0} does not implemented {1} interface", targetBlock.GetType(), TARGET_BLOCK_INTERFACE_NAME));
            }

            var sourceRecordType = sourceInterfaceType.GetGenericArguments().FirstOrDefault();

            var targetRecordType = targetInterfaceType.GetGenericArguments().FirstOrDefault();

            if(sourceRecordType != targetRecordType)
            {
                throw new Exceptions.BlockLinkException(string.Format("can not link type {0} to type {1}", sourceRecordType.GetGenericArguments().First(), targetRecordType.GetGenericArguments().First()));
            }

            var linkToMethodWithThreeParameters = typeof(DataflowBlock).GetMethods().Where(m => m.Name.Equals("LinkTo")).Last();
            var nullTargetMethod = typeof(DataflowBlock).GetMethods().Where(m => m.Name.Equals("NullTarget")).First();

            linkToMethodWithThreeParameters = linkToMethodWithThreeParameters.MakeGenericMethod(targetRecordType);
            nullTargetMethod = nullTargetMethod.MakeGenericMethod(targetRecordType);

            var predicateMakerClass = typeof(MessagePredicateMaker<>).MakeGenericType(targetRecordType.GetGenericArguments().First());

            var predicateInstance = Activator.CreateInstance(predicateMakerClass, new object[] { });

            var isNotBroken = predicateMakerClass.GetProperty("IsNotBroken").GetValue(predicateInstance);
            var isBroken = predicateMakerClass.GetProperty("IsBroken").GetValue(predicateInstance);

            var nullTarget = nullTargetMethod.Invoke(null, new object[] { });

            linkToMethodWithThreeParameters.Invoke(null, new object[] { _block, targetBlock, options, isNotBroken});
            linkToMethodWithThreeParameters.Invoke(null, new object[] { _block, nullTarget, options, isBroken });

            return null;
        }

        public Task<bool> SendAsync(object message)
        {
            if(_sendMethodExecutor == null)
            {
                var sendAsyncMethod = typeof(DataflowBlock).GetMethods().Where(m => m.Name.Equals("SendAsync")).First();

                sendAsyncMethod = sendAsyncMethod.MakeGenericMethod(message.GetType());

                _sendMethodExecutor = MethodExecutorBuilder.Build(sendAsyncMethod);
            }

            return (Task<bool>)_sendMethodExecutor(null, new object[] { _block, message });
        }

        public void Complete()
        {
            var method = _block.GetType().GetMethod("Complete");

            method.Invoke(_block, new object[] { });
        }

        public Task Completion { get { return _block.GetType().GetProperty("Completion").GetValue(_block) as Task; } }

    }
}
