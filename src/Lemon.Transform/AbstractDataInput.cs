using Lemon.Transform.Models;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class AbstractDataInput : PipelineObject
    {
        private ITargetBlock<DataRowTransformWrapper<BsonDataRow>> _targetBlock;

        protected ParametersInfo PrametersInfo = new ParametersInfo();

        public void LinkTo(LinkObject target)
        {
            _targetBlock = target.AsTarget();
        }

        protected void Post(BsonDataRow row)
        {
            Context.ProgressIndicator.Increment(Name);

            _targetBlock.Post(new DataRowTransformWrapper<BsonDataRow> { Success = true, Row = row });
        }

        /// <summary>
        /// singal the completion of data input
        /// </summary>
        protected void Complete()
        {
            _targetBlock.Complete();
        }

        public abstract void Start(IDictionary<string, object> parameters = null);
    }
}
