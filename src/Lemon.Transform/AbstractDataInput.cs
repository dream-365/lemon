using Lemon.Transform.Models;
using System;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class AbstractDataInput
    {
        private string _name;

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    _name = this.GetType().Name + "_" + Guid.NewGuid().ToString();
                }

                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public PipelineContext Context { get; set;}

        private ITargetBlock<DataRowWrapper<BsonDataRow>> _targetBlock;

        public void LinkTo(LinkObject target)
        {
            _targetBlock = target.AsTarget();
        }

        protected void Post(BsonDataRow row)
        {
            if (Context != null)
            {
                Context.ProgressIndicator.Increment(Name);
            }

            _targetBlock.Post(new DataRowWrapper<BsonDataRow> { Success = true, Row = row });
        }

        protected void Complete()
        {
            _targetBlock.Complete();
        }

        public abstract void Start();
    }
}
