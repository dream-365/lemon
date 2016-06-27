using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public abstract class AbstractDataOutput : LinkObject
    {
        private ActionBlock<DataRowWrapper<BsonDataRow>> _actionBlock;

        public AbstractDataOutput()
        {
            _actionBlock = new ActionBlock<DataRowWrapper<BsonDataRow>>(new Action<DataRowWrapper<BsonDataRow>>(OnReceive));
        }

        internal override ISourceBlock<DataRowWrapper<BsonDataRow>> AsSource()
        {
            throw new NotSupportedException();
        }

        internal override ITargetBlock<DataRowWrapper<BsonDataRow>> AsTarget()
        {
            return _actionBlock as ITargetBlock<DataRowWrapper<BsonDataRow>>;
        }

        public override Task Compltetion
        {
            get
            {
                return _actionBlock.Completion;
            }
        }

        protected void OnReceive(DataRowWrapper<BsonDataRow> data)
        {
            OnReceive(data.Row);
        }

        protected abstract void OnReceive(BsonDataRow row);
    }
}
