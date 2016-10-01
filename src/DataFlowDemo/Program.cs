using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var bufferBlock = new BufferBlock<int>(new DataflowBlockOptions { BoundedCapacity = 100 });

            var transformBlock = new TransformBlock<int, int>(item => { return item + 10000; }, new ExecutionDataflowBlockOptions { BoundedCapacity = 10 });

            var actionBlock = new ActionBlock<int>((item) => { Task.Delay(100).Wait(); Console.WriteLine("action: {0}", item); }, new ExecutionDataflowBlockOptions { BoundedCapacity = 10 });

            bufferBlock.LinkTo(transformBlock, new DataflowLinkOptions { PropagateCompletion = true });

            transformBlock.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });


            //Task.Run(() => {
            //    while (true)
            //    {
            //        int value = bufferBlock.Receive();

            //        actionBlock.SendAsync(value).Wait();
            //    }
            //});

            for (int i = 0; i < 1000; i++)
            {
                var success = bufferBlock.SendAsync(i).Result;

                Console.WriteLine(i + ":" + success);
            }

            bufferBlock.Complete();

            actionBlock.Completion.Wait();
        }
    }
}
