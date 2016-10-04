using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Linq;

namespace Lemon.Transform
{
    /// <summary>
    /// a class used to manage link object routes (success, error, default)
    /// </summary>
    public class LinkObjectRouter
    {
        private LinkObject _sourceLinkObject;

        public LinkObjectRouter(LinkObject sourceLinkObject)
        {
            _sourceLinkObject = sourceLinkObject;
        }

        public void BroadCast(params LinkObject[] linkObjects)
        {
            var targets = linkObjects.Select(m => m.AsTarget());

            var actionBlock = new ActionBlock<DataRowTransformWrapper<BsonDataRow>>(async item => {
                foreach(var target in targets)
                {
                    await target.SendAsync(item);
                }
            }, new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = ExecutionDataflowBlockOptions.Unbounded
            });

            _sourceLinkObject.AsSource().LinkTo(actionBlock);

            _sourceLinkObject.AsSource().Completion.ContinueWith(task => {
                actionBlock.Complete();
            });

            actionBlock.Completion.ContinueWith(task => {
                foreach (var linkObject in linkObjects)
                {
                    linkObject.AsTarget().Complete();
                }
            });

            foreach(var linkObject in linkObjects)
            {
                _sourceLinkObject.Node.AddChildNode(linkObject.Node);
            }
        }

        /// <summary>
        /// default success route
        /// </summary>
        /// <param name="linkObject"></param>
        /// <returns></returns>
        public LinkObjectRouter SuccessTo(LinkObject linkObject)
        {
            var target = linkObject.AsTarget();

            _sourceLinkObject.AsSource().LinkTo(target, data => data.Success);

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
            var target = linkObject.AsTarget();

            _sourceLinkObject.AsSource().LinkTo(target, new DataflowLinkOptions { PropagateCompletion = true }, data => data.Success && predicate(data.Row));

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
            var target = linkObject.AsTarget();

            _sourceLinkObject.AsSource().LinkTo(target, data => !data.Success);

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
    }
}
