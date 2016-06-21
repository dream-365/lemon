using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class BroadcastAction : LinkObject
    {
        private BroadcastBlock<BsonDataRow> _broadcastBlock;

        public BroadcastAction()
        {
            _broadcastBlock = new BroadcastBlock<BsonDataRow>(row => {
                return row;
            });
        }

        internal override ISourceBlock<BsonDataRow> AsSource()
        {
            return _broadcastBlock as ISourceBlock<BsonDataRow>;
        }

        public override Task Compltetion
        {
            get
            {
                return _broadcastBlock.Completion;
            }
        }

        internal override ITargetBlock<BsonDataRow> AsTarget()
        {
            return _broadcastBlock as ITargetBlock<BsonDataRow>;
        }
    }
}
