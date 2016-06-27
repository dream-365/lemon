using Lemon.Transform.Models;
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
        private ITargetBlock<DataRowWrapper<BsonDataRow>> _targetBlock;

        public void LinkTo(LinkObject target)
        {
            _targetBlock = target.AsTarget();
        }

        protected void Post(BsonDataRow row)
        {
            _targetBlock.Post(new DataRowWrapper<BsonDataRow> { Success = true, Row = row });
        }

        protected void Complete()
        {
            _targetBlock.Complete();
        }

        public abstract void Start();
    }
}
