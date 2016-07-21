using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public abstract class AbstractDataOutput : LinkObject
    {
        private ActionBlock<DataRowWrapper<BsonDataRow>> _actionBlock;

        public Action<BsonDataRow> OnError;

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

        public virtual DataRowStatusContext GetDataRowStatusContext(string [] excludes)
        {
            throw new NotSupportedException();
        }

        protected void OnReceive(DataRowWrapper<BsonDataRow> data)
        {
            Context.ProgressIndicator.Increment(string.Format("{0}.process", Name));

            try
            {
                OnReceive(data.Row);

                Context.ProgressIndicator.Increment(string.Format("{0}.output", Name));
            }
            catch (Exception ex)
            {
                if(OnError != null)
                {
                    OnError(data.Row);
                }

                Context.ProgressIndicator.Increment(string.Format("{0}.error", Name));

                LogService.Default.Error(string.Format("{0} - failed", Name), ex);
            }
        }

        protected abstract void OnReceive(BsonDataRow row);
    }
}
