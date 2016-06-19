using System;
using DF = System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class TransformSingleAction : LinkObject
    {
        private DF.TransformBlock<BsonDataRow, BsonDataRow> _transformBlock;

        public TransformSingleAction()
        {
            var transform = new Func<BsonDataRow, BsonDataRow>(Transform);

            _transformBlock = new DF.TransformBlock<BsonDataRow, BsonDataRow>(transform);
        }

        internal override DF.ISourceBlock<BsonDataRow> AsSource()
        {
            return _transformBlock as DF.ISourceBlock<BsonDataRow>;
        }

        internal override DF.ITargetBlock<BsonDataRow> AsTarget()
        {
            return _transformBlock as DF.ITargetBlock<BsonDataRow>;
        }
        
        public abstract BsonDataRow Transform(BsonDataRow row);
    }
}
