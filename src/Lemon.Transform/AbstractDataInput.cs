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

            Node.AddChildNode(target.Node);
        }

        public void SetDefaultParameterValue(string name, object value)
        {
            PrametersInfo.SetParameterDefultValue(name, value);
        }

        protected void Send(BsonDataRow row)
        {
            Context.ProgressIndicator.Increment(Name);

            _targetBlock.SendAsync(new DataRowTransformWrapper<BsonDataRow> { Success = true, Row = row }).Wait();
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
