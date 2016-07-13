using System;
using System.Threading.Tasks;
using DF = System.Threading.Tasks.Dataflow;
using System.Threading.Tasks.Dataflow;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public abstract class TransformSingleAction : LinkObject
    {
        private DF.TransformBlock<DataRowWrapper<BsonDataRow>, DataRowWrapper<BsonDataRow>> _transformBlock;


        public TransformSingleAction(int maxDegreeOfParallelism = 0)
        {
            var transform = new Func<DataRowWrapper<BsonDataRow>, DataRowWrapper<BsonDataRow>>(InternalTransform);

            if(maxDegreeOfParallelism == 0)
            {
                _transformBlock = new DF.TransformBlock<DataRowWrapper<BsonDataRow>, DataRowWrapper<BsonDataRow>>(transform);
            }
            else
            {
                var options = new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                };

                _transformBlock = new DF.TransformBlock<DataRowWrapper<BsonDataRow>, DataRowWrapper<BsonDataRow>>(transform, options);
            }
        }

        internal override DF.ISourceBlock<DataRowWrapper<BsonDataRow>> AsSource()
        {
            return _transformBlock as DF.ISourceBlock<DataRowWrapper<BsonDataRow>>;
        }

        internal override DF.ITargetBlock<DataRowWrapper<BsonDataRow>> AsTarget()
        {
            return _transformBlock as DF.ITargetBlock<DataRowWrapper<BsonDataRow>>;
        }

        public override Task Compltetion
        {
            get
            {
                return _transformBlock.Completion;
            }
        }

        public abstract BsonDataRow Transform(BsonDataRow row);

        protected DataRowWrapper<BsonDataRow> InternalTransform(DataRowWrapper<BsonDataRow> data)
        {
            Context.ProgressIndicator.Increment(string.Format("{0}.process", Name));

            try
            {
                var row = Transform(data.Row);

                Context.ProgressIndicator.Increment(string.Format("{0}.output", Name));

                return new DataRowWrapper<BsonDataRow>
                {
                    Success = true,
                    Row = row
                };
            }catch(Exception ex)
            {
                Context.ProgressIndicator.Increment(string.Format("{0}.error", Name));

                LogService.Default.Error(string.Format("{0} transform failed", Name), ex);

                return new DataRowWrapper<BsonDataRow> {
                    Success = false,
                    Row = data.Row,
                    Exception = ex
                };
            }
        }
    }
}
