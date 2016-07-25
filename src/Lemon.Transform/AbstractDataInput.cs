using Lemon.Transform.Models;
using System;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class AbstractDataInput : PipelineObject
    {
        private ITargetBlock<DataRowTransformWrapper<BsonDataRow>> _targetBlock;

        public void LinkTo(LinkObject target)
        {
            _targetBlock = target.AsTarget();
        }

        protected void Post(BsonDataRow row)
        {
            Context.ProgressIndicator.Increment(Name);

            _targetBlock.Post(new DataRowTransformWrapper<BsonDataRow> { Success = true, Row = row });
        }

        protected void Complete()
        {
            _targetBlock.Complete();
        }

        public abstract void Start();
    }
}
