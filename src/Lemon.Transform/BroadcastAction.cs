using Lemon.Transform.Models;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class BroadcastAction : LinkObject
    {
        private BroadcastBlock<DataRowWrapper<BsonDataRow>> _broadcastBlock;

        public BroadcastAction()
        {
            _broadcastBlock = new BroadcastBlock<DataRowWrapper<BsonDataRow>>(row => {
                return row;
            });
        }

        internal override ISourceBlock<DataRowWrapper<BsonDataRow>> AsSource()
        {
            return _broadcastBlock as ISourceBlock<DataRowWrapper<BsonDataRow>>;
        }

        public override Task Compltetion
        {
            get
            {
                return _broadcastBlock.Completion;
            }
        }

        internal override ITargetBlock<DataRowWrapper<BsonDataRow>> AsTarget()
        {
            return _broadcastBlock as ITargetBlock<DataRowWrapper<BsonDataRow>>;
        }
    }
}
