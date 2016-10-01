using Lemon.Transform.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DF = System.Threading.Tasks.Dataflow;
using System.Linq;

namespace Lemon.Transform
{
    public abstract class TransformManyAction : LinkObject
    {
        private DF.TransformManyBlock<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>> _transformBlock;


        public TransformManyAction()
        {
            var transform = new Func<DataRowTransformWrapper<BsonDataRow>, IEnumerable<DataRowTransformWrapper<BsonDataRow>>>(Transform);

            var options = new DF.ExecutionDataflowBlockOptions
            {
                BoundedCapacity = GlobalConfiguration.TransformConfiguration.BoundedCapacity ?? DF.ExecutionDataflowBlockOptions.Unbounded
            };

            _transformBlock = new DF.TransformManyBlock<DataRowTransformWrapper<BsonDataRow>, DataRowTransformWrapper<BsonDataRow>>(transform, options);
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

        protected abstract void InternalTransform(BsonDataRow row, ConcurrentQueue<BsonDataRow> queue);

        private IEnumerable<DataRowTransformWrapper<BsonDataRow>> Transform(DataRowTransformWrapper<BsonDataRow> data)
        {
            var queue = new ConcurrentQueue<BsonDataRow>();

            try
            {
                Context.ProgressIndicator.Increment(string.Format("{0}.process", Name));

                InternalTransform(data.Row, queue);

                Context.ProgressIndicator.Increment(string.Format("{0}.output", Name), queue.Count);

                return queue.Select(m => new DataRowTransformWrapper<BsonDataRow> { Success = true, Row = m });
            }
            catch (Exception ex)
            {
                Context.ProgressIndicator.Increment(string.Format("{0}.error", Name));

                LogService.Default.Error(string.Format("{0} trasform failed", Name), ex);

                return new List<DataRowTransformWrapper<BsonDataRow>>();
            }
        }
    }
}
