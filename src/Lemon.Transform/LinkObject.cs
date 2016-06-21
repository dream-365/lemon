using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class LinkObject
    {
        internal abstract ISourceBlock<BsonDataRow> AsSource();

        internal abstract ITargetBlock<BsonDataRow> AsTarget();

        public abstract Task Compltetion { get;}

        private IList<ITargetBlock<BsonDataRow>> _targets;

        private bool _registered = false;


        public LinkObject()
        {
            _targets = new List<ITargetBlock<BsonDataRow>>();
        }


        private void RegisterOnce()
        {
            if(_registered)
            {
                return;
            }

            try
            {
                var source = AsSource();

                if (source == null)
                {
                    return;
                }

                source.Completion.ContinueWith(t => {
                    foreach (var target in _targets)
                    {
                        target.Complete();
                    }
                });

                _registered = true;
            }
            catch (System.NotSupportedException)
            {
                // if not support as source, skip the complete chain
            }
        }

        public void LinkTo(LinkObject action)
        {
            RegisterOnce();

            var target = action.AsTarget();

            AsSource().LinkTo(target, row => row != null);

            AsSource().LinkTo(DataflowBlock.NullTarget<BsonDataRow>());

            _targets.Add(target);
        }
    }
}
