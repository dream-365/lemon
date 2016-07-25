using Lemon.Transform.Models;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class BroadcastAction : LinkObject
    {
        private BroadcastBlock<DataRowTransformWrapper<BsonDataRow>> _broadcastBlock;

        public BroadcastAction()
        {
            _broadcastBlock = new BroadcastBlock<DataRowTransformWrapper<BsonDataRow>>(row => {
                return row;
            });
        }

        internal override ISourceBlock<DataRowTransformWrapper<BsonDataRow>> AsSource()
        {
            return _broadcastBlock as ISourceBlock<DataRowTransformWrapper<BsonDataRow>>;
        }

        public override Task Compltetion
        {
            get
            {
                return _broadcastBlock.Completion;
            }
        }

        internal override ITargetBlock<DataRowTransformWrapper<BsonDataRow>> AsTarget()
        {
            return _broadcastBlock as ITargetBlock<DataRowTransformWrapper<BsonDataRow>>;
        }
    }
}
