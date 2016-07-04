using Lemon.Transform.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DF = System.Threading.Tasks.Dataflow;
using System.Linq;

namespace Lemon.Transform
{
    public abstract class TransformManyAction : LinkObject
    {
        private DF.TransformManyBlock<DataRowWrapper<BsonDataRow>, DataRowWrapper<BsonDataRow>> _transformBlock;

        public PipelineContext Context { get; set; }

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


        public TransformManyAction()
        {
            var transform = new Func<DataRowWrapper<BsonDataRow>, IEnumerable<DataRowWrapper<BsonDataRow>>>(Transform);

            _transformBlock = new DF.TransformManyBlock<DataRowWrapper<BsonDataRow>, DataRowWrapper<BsonDataRow>>(transform);
        }

        internal override DF.ISourceBlock<DataRowWrapper<BsonDataRow>> AsSource()
        {
            return _transformBlock as DF.ISourceBlock<DataRowWrapper<BsonDataRow>>;
        }

        internal override DF.ITargetBlock<DataRowWrapper<BsonDataRow>> AsTarget()
        {
            return _transformBlock as DF.ITargetBlock<DataRowWrapper<BsonDataRow>>;
        }

        public override Task Compltetion
        {
            get
            {
                return _transformBlock.Completion;
            }
        }

        protected abstract void InternalTransform(BsonDataRow row, ConcurrentQueue<BsonDataRow> queue);

        private IEnumerable<DataRowWrapper<BsonDataRow>> Transform(DataRowWrapper<BsonDataRow> data)
        {
            var queue = new ConcurrentQueue<BsonDataRow>();

            try
            {
                if (Context != null)
                {
                    Context.ProgressIndicator.Increment(Name);
                }

                InternalTransform(data.Row, queue);

                return queue.Select(m => new DataRowWrapper<BsonDataRow> { Success = true, Row = m });
            }
            catch (Exception)
            {
                return new List<DataRowWrapper<BsonDataRow>>();
            }
        }
    }
}
