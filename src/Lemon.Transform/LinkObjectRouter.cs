using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    /// <summary>
    /// a class used to manage link object routes (success, error, default)
    /// </summary>
    public class LinkObjectRouter
    {
        private LinkObject _sourceLinkObject;

        private IList<ITargetBlock<DataRowTransformWrapper<BsonDataRow>>> _targets;

        private bool _linkInitialized = false;

        public LinkObjectRouter(LinkObject sourceLinkObject)
        {
            _sourceLinkObject = sourceLinkObject;

            _targets = new List<ITargetBlock<DataRowTransformWrapper<BsonDataRow>>>();
        }

        /// <summary>
        /// default success route
        /// </summary>
        /// <param name="linkObject"></param>
        /// <returns></returns>
        public LinkObjectRouter SuccessTo(LinkObject linkObject)
        {
            InitializeLink();

            var target = linkObject.AsTarget();

            _sourceLinkObject.AsSource().LinkTo(target, data => data.Success);

            _targets.Add(target);

            _sourceLinkObject.Node.AddChildNode(linkObject.Node);

            return this;
        }

        /// <summary>
        /// success route with predicate
        /// </summary>
        /// <param name="linkObject"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public LinkObjectRouter SuccessTo(LinkObject linkObject, Predicate<BsonDataRow> predicate)
        {
            InitializeLink();

            var target = linkObject.AsTarget();

            _sourceLinkObject.AsSource().LinkTo(target, data => data.Success && predicate(data.Row));

            _targets.Add(target);

            _sourceLinkObject.Node.AddChildNode(linkObject.Node);

            return this;
        }

        /// <summary>
        /// on error route
        /// </summary>
        /// <param name="linkObject"></param>
        /// <returns></returns>
        public LinkObjectRouter ErrorTo(LinkObject linkObject)
        {
            InitializeLink();

            var target = linkObject.AsTarget();

            _sourceLinkObject.AsSource().LinkTo(target, data => !data.Success);

            _targets.Add(target);

            _sourceLinkObject.Node.AddChildNode(linkObject.Node);

            return this;
        }


        /// <summary>
        /// all other route goes here
        /// </summary>
        public void End()
        {
            _sourceLinkObject.AsSource().LinkTo(DataflowBlock.NullTarget<DataRowTransformWrapper<BsonDataRow>>());
        }

        /// <summary>
        /// bind the on complete event
        /// </summary>
        private void InitializeLink()
        {
            if (_linkInitialized)
            {
                return;
            }

            try
            {
                if (_sourceLinkObject == null)
                {
                    return;
                }

                _sourceLinkObject.AsSource().Completion.ContinueWith(t => {
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
