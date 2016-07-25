using Lemon.Transform.Models;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class LinkObject : PipelineObject
    {
        internal abstract ISourceBlock<DataRowTransformWrapper<BsonDataRow>> AsSource();

        internal abstract ITargetBlock<DataRowTransformWrapper<BsonDataRow>> AsTarget();

        private LinkManagement _linkManagement;

        public abstract Task Compltetion { get;}


        public LinkObject()
        {
            _linkManagement = new LinkManagement(new Lazy<ISourceBlock<DataRowTransformWrapper<BsonDataRow>>>(AsSource));
        }

        public LinkManagement Link { get { return _linkManagement; } }
    }
}
