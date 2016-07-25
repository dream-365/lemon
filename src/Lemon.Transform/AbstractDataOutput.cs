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

        public Action<BsonDataRow> BeforeWrite;

        public Action<BsonDataRow> AfterWrite;

        protected Func<BsonDataRow, bool> DetermineWriteOrNot;

        public AbstractDataOutput()
        {
            _actionBlock = new ActionBlock<DataRowWrapper<BsonDataRow>>(new Action<DataRowWrapper<BsonDataRow>>(OnReceive));

            BeforeWrite = Dummy;

            AfterWrite = Dummy;

            OnError = Dummy;

            DetermineWriteOrNot = (row) => { return true; };
        }

        private static void Dummy(BsonDataRow row) { }

        protected Func<BsonDataRow, bool> BuildDetermineWriteOrNotFunction(WriteOnChangeConfiguration writeOnChange)
        {
            Func<BsonDataRow, bool> func = (row) => { return true; };

            if (writeOnChange != null && writeOnChange.Enabled)
            {
                var context = BuildDataRowStatusContext(writeOnChange.ExcludedColumNames);

                func = (row) => {

                    var status = context.Compare(row);

                    #region debug section

                    if(GlobalConfiguration.TransformConfiguration.Debug)
                    {
                        LogService.Default.Info(row.ToString());

                        LogService.Default.Info(string.Format("status:", status));
                    }

                    #endregion

                    return status != DataRowCompareStatus.NoChange;
                };
            }

            return func;
        }

        protected virtual DataRowStatusContext BuildDataRowStatusContext(string[] excludes)
        {
            throw new NotSupportedException();
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
            Context.ProgressIndicator.Increment(string.Format("{0}.process", Name));

            try
            {
                if(DetermineWriteOrNot(data.Row))
                {
                    BeforeWrite(data.Row);

                    OnReceive(data.Row);

                    Context.ProgressIndicator.Increment(string.Format("{0}.output", Name));

                    AfterWrite(data.Row);
                }
                else
                {
                    Context.ProgressIndicator.Increment(string.Format("{0}.nochange", Name));
                }  
            }
            catch (Exception ex)
            {
                OnError(data.Row);

                Context.ProgressIndicator.Increment(string.Format("{0}.error", Name));

                LogService.Default.Error(string.Format("{0} - failed", Name), ex);
            }
        }

        protected abstract void OnReceive(BsonDataRow row);
    }
}
