using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class LinkObject
    {
        internal abstract ISourceBlock<BsonDataRow> AsSource();

        internal abstract ITargetBlock<BsonDataRow> AsTarget();

        public void LinkTo(LinkObject action)
        {
            AsSource().LinkTo(action.AsTarget());
        }
    }
}
