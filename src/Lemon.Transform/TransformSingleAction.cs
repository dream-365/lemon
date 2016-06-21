using System;
using System.Threading.Tasks;
using DF = System.Threading.Tasks.Dataflow;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class TransformSingleAction : LinkObject
    {
        private DF.TransformBlock<BsonDataRow, BsonDataRow> _transformBlock;

        public TransformSingleAction(int maxDegreeOfParallelism = 0)
        {

            var transform = new Func<BsonDataRow, BsonDataRow>(Transform);

            if(maxDegreeOfParallelism == 0)
            {
                _transformBlock = new DF.TransformBlock<BsonDataRow, BsonDataRow>(transform);
            }
            else
            {
                var options = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                };

                _transformBlock = new DF.TransformBlock<BsonDataRow, BsonDataRow>(transform, options);
            }
        }

        internal override DF.ISourceBlock<BsonDataRow> AsSource()
        {
            return _transformBlock as DF.ISourceBlock<BsonDataRow>;
        }

        internal override DF.ITargetBlock<BsonDataRow> AsTarget()
        {
            return _transformBlock as DF.ITargetBlock<BsonDataRow>;
        }

        public override Task Compltetion
        {
            get
            {
                return _transformBlock.Completion;
            }
        }

        public abstract BsonDataRow Transform(BsonDataRow row);
    }
}
