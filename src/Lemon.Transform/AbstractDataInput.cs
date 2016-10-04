using Lemon.Transform.Models;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Lemon.Transform
{
    public abstract class AbstractDataInput : LinkObject
    {
        private BufferBlock<DataRowTransformWrapper<BsonDataRow>> _bufferBlock;

        public AbstractDataInput()
        {
            _bufferBlock = new BufferBlock<DataRowTransformWrapper<BsonDataRow>>(new DataflowBlockOptions { BoundedCapacity = 5 });
        }

        protected ParametersInfo PrametersInfo = new ParametersInfo();


        public void SetDefaultParameterValue(string name, object value)
        {
            PrametersInfo.SetParameterDefultValue(name, value);
        }

        protected async Task<bool> SendAsync(BsonDataRow row)
        {
            Context.ProgressIndicator.Increment(Name);

            Console.WriteLine("buffer_count: " + _bufferBlock.Count);

            var result = await _bufferBlock.SendAsync(new DataRowTransformWrapper<BsonDataRow> { Success = true, Row = row });

            Console.WriteLine("{0}-{1}", result, row.GetValue("id"));

            return result;
        }

        public override Task Compltetion
        {
            get
            {
                return _bufferBlock.Completion;
            }
        }

        /// <summary>
        /// signal complete
        /// </summary>
        public void Complete ()
        {
            _bufferBlock.Complete();
        }

        internal override ISourceBlock<DataRowTransformWrapper<BsonDataRow>> AsSource()
        {
            return _bufferBlock as ISourceBlock<DataRowTransformWrapper<BsonDataRow>>;
        }

        internal override ITargetBlock<DataRowTransformWrapper<BsonDataRow>> AsTarget()
        {
            return _bufferBlock as ITargetBlock<DataRowTransformWrapper<BsonDataRow>>;
        }

        public abstract Task StartAsync(IDictionary<string, object> parameters = null);
    }
}
