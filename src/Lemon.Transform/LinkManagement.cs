using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public class LinkManagement
    {
        private Lazy<ISourceBlock<DataRowTransformWrapper<BsonDataRow>>> _source;

        private IList<ITargetBlock<DataRowTransformWrapper<BsonDataRow>>> _targets;

        private bool _linkInitialized = false;

        public LinkManagement(Lazy<ISourceBlock<DataRowTransformWrapper<BsonDataRow>>> source)
        {
            _source = source;

            _targets = new List<ITargetBlock<DataRowTransformWrapper<BsonDataRow>>>();
        }

        public LinkManagement SuccessTo(LinkObject linkObject)
        {
            InitializeLink();

            var target = linkObject.AsTarget();

            _source.Value.LinkTo(target, data => data.Success);

            _targets.Add(target);

            return this;
        }

        public LinkManagement SuccessTo(LinkObject linkObject, Predicate<BsonDataRow> predicate)
        {
            InitializeLink();

            var target = linkObject.AsTarget();

            _source.Value.LinkTo(target, data => data.Success && predicate(data.Row));

            _targets.Add(target);

            return this;
        }


        public LinkManagement ErrorTo(LinkObject linkObject)
        {
            InitializeLink();

            var target = linkObject.AsTarget();

            _source.Value.LinkTo(target, data => !data.Success);

            _targets.Add(target);

            return this;
        }


        public void End()
        {
            _source.Value.LinkTo(DataflowBlock.NullTarget<DataRowTransformWrapper<BsonDataRow>>());
        }


        private void InitializeLink()
        {
            if (_linkInitialized)
            {
                return;
            }

            try
            {
                if (_source == null)
                {
                    return;
                }

                _source.Value.Completion.ContinueWith(t => {
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
    }
}
