using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class AbstractDataInput
    {
        private ITargetBlock<BsonDataRow> _targetBlock;

        public void LinkTo(LinkObject target)
        {
            _targetBlock = target.AsTarget();
        }

        protected void Post(BsonDataRow row)
        {
            _targetBlock.Post(row);
        }


        protected void Complete()
        {
            _targetBlock.Complete();
        }

        public abstract void Start();
    }
}
