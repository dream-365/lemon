﻿using System;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class AbstractDataOutput : LinkObject
    {
        private ActionBlock<BsonDataRow> _actionBlock;

        public AbstractDataOutput()
        {
            _actionBlock = new ActionBlock<BsonDataRow>(new Action<BsonDataRow>(OnReceive));
        }

        internal override ISourceBlock<BsonDataRow> AsSource()
        {
            throw new NotSupportedException();
        }

        internal override ITargetBlock<BsonDataRow> AsTarget()
        {
            return _actionBlock as ITargetBlock<BsonDataRow>;
        }

        protected abstract void OnReceive(BsonDataRow row);
    }
}