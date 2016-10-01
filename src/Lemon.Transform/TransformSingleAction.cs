using System;
using System.Threading.Tasks;
using DF = System.Threading.Tasks.Dataflow;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public abstract class TransformSingleAction : LinkObject
    {
        private DF.TransformBlock<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>> _transformBlock;

        public TransformSingleAction(int maxDegreeOfParallelism = 0)
        {
            var transform = new Func<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>>(InternalTransform);

            var options = new DF.ExecutionDataflowBlockOptions
            {
                BoundedCapacity = GlobalConfiguration.TransformConfiguration.BoundedCapacity ?? DF.ExecutionDataflowBlockOptions.Unbounded
            };

            if (maxDegreeOfParallelism == 0)
            {
                _transformBlock = new DF.TransformBlock<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>>(transform, options);
            }
            else
            {
                options.MaxDegreeOfParallelism = maxDegreeOfParallelism;

                _transformBlock = new DF.TransformBlock<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>>(transform, options);
            }
        }

        internal override DF.ISourceBlock<DataRowTransformWrapper<BsonDataRow>> AsSource()
        {
            return _transformBlock as DF.ISourceBlock<DataRowTransformWrapper<BsonDataRow>>;
        }

        internal override DF.ITargetBlock<DataRowTransformWrapper<BsonDataRow>> AsTarget()
        {
            return _transformBlock as DF.ITargetBlock<DataRowTransformWrapper<BsonDataRow>>;
        }

        public override Task Compltetion
        {
            get
            {
                return _transformBlock.Completion;
            }
        }

        public abstract BsonDataRow Transform(BsonDataRow row);

        protected DataRowTransformWrapper<BsonDataRow> InternalTransform(DataRowTransformWrapper<BsonDataRow> data)
        {
            Context.ProgressIndicator.Increment(string.Format("{0}.process", Name));

            try
            {
                var row = Transform(data.Row);

                Context.ProgressIndicator.Increment(string.Format("{0}.output", Name));

                return new DataRowTransformWrapper<BsonDataRow>
                {
                    Success = true,
                    Row = row
                };
            }catch(Exception ex)
            {
                Context.ProgressIndicator.Increment(string.Format("{0}.error", Name));

                LogService.Default.Error(string.Format("{0} transform failed", Name), ex);

                return new DataRowTransformWrapper<BsonDataRow> {
                    Success = false,
                    Row = data.Row,
                    Exception = ex
                };
            }
        }
    }
}
