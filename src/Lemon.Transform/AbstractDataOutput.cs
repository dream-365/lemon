using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public abstract class AbstractDataOutput : LinkObject
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

        public PipelineContext Context { get; set; }

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
            if(Context != null)
            {
                Context.ProgressIndicator.Increment(Name);
            }

            try
            {
                OnReceive(data.Row);
            }
            catch (Exception ex)
            {
                LogService.Default.Error(string.Format("{0} - failed", Name), ex);
            }
        }

        protected abstract void OnReceive(BsonDataRow row);
    }
}
