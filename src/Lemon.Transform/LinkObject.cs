using System;
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

        private bool _linkInitialized = false;


        public LinkObject()
        {
            _targets = new List<ITargetBlock<BsonDataRow>>();
        }


        private void InitializeLink()
        {
            if(_linkInitialized)
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

                source.LinkTo(DataflowBlock.NullTarget<BsonDataRow>());

                source.Completion.ContinueWith(t => {
                    foreach (var target in _targets)
                    {
                        target.Complete();
                    }
                });



                _linkInitialized = true;
            }
            catch (System.NotSupportedException)
            {
                // if not support as source, skip the complete chain
            }
        }

        public void LinkTo(LinkObject action)
        {
            InitializeLink();

            var target = action.AsTarget();

            AsSource().LinkTo(target, row => row != null);

            _targets.Add(target);
        }

        public void LinkTo(LinkObject action, Predicate<BsonDataRow> predicate)
        {
            InitializeLink();

            var target = action.AsTarget();

            AsSource().LinkTo(target, row => (row != null && predicate(row)));

            _targets.Add(target);
        }
    }
}
