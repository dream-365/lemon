using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DF = System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class TransformManyAction : LinkObject
    {
        private DF.TransformManyBlock<BsonDataRow, BsonDataRow> _transformBlock;

        public TransformManyAction()
        {
            var transform = new Func<BsonDataRow, IEnumerable<BsonDataRow>>(Transform);

            _transformBlock = new DF.TransformManyBlock<BsonDataRow, BsonDataRow>(transform);
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

        protected abstract void InternalTransform(BsonDataRow row, ConcurrentQueue<BsonDataRow> queue);

        private IEnumerable<BsonDataRow> Transform(BsonDataRow row)
        {
            var queue = new ConcurrentQueue<BsonDataRow>();

            InternalTransform(row, queue);

            return queue;
        }
    }
}
