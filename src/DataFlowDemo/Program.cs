using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowDemo
{

    public interface ISouceNode
    {
        Type SourceType { get; }
    }

    public class Node<T> : ISouceNode
    {
        private Type _sourceType;

        public Node()
        {
            _sourceType = typeof(T);
        }

        public Type SourceType
        {
            get
            {
                return _sourceType;
            }
        }
    }


    class Program
    {
        private delegate object LinkToExecutor(object instance, object[] parameters);

        static void Main(string[] args)
        {
            var node = new Node<int>();

            Type sourceType = typeof(ISouceNode).GetProperty("SourceType").GetValue(node) as Type;

            var bufferBlockClass = typeof(BufferBlock<>).MakeGenericType(sourceType);

            var actionBlockClass = typeof(ActionBlock<>).MakeGenericType(sourceType);

            // DataflowLinkOptions linkOptions

            var sourceBlockType = typeof(ISourceBlock<>).MakeGenericType(sourceType);

            var targetBlockType = typeof(ITargetBlock<>).MakeGenericType(sourceType);

            var bufferBlockInstance = Activator.CreateInstance(bufferBlockClass);

            var actionBlockInstance = Activator.CreateInstance(actionBlockClass, new object [] {
                new Action<int>((val)=> {
                    Console.WriteLine(val);
                })
            });

            var method = sourceBlockType.GetMethod("LinkTo", new Type[] { targetBlockType,  typeof(DataflowLinkOptions) } );

            var sendAsyncMethod = typeof(DataflowBlock).GetMethods().Where(m => m.Name.Equals("SendAsync")).First();

            sendAsyncMethod = sendAsyncMethod.MakeGenericMethod(sourceType);

            var executor = MethodExecutorBuilder.Build(method);

            var sendExecutor = MethodExecutorBuilder.Build(sendAsyncMethod);

            executor(bufferBlockInstance, new[] { actionBlockInstance, new DataflowLinkOptions { PropagateCompletion = true } });

            for (int i = 0; i < 1000; i++)
            {
                // var success = bufferBlock.SendAsync(i).Result;

                var task = sendExecutor(null, new object[] { bufferBlockInstance, i }) as Task<bool>;

                task.Wait();
            }

            System.Threading.Thread.Sleep(10000000);
        }
    }
}
