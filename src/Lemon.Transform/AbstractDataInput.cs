using Lemon.Transform.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace Lemon.Transform
{
    public abstract class AbstractDataInput : PipelineObject
    {
        private ITargetBlock<DataRowTransformWrapper<BsonDataRow>> _targetBlock;

        public void LinkTo(LinkObject target)
        {
            _targetBlock = target.AsTarget();
        }

        protected void Post(BsonDataRow row)
        {
            Context.ProgressIndicator.Increment(Name);

            _targetBlock.Post(new DataRowTransformWrapper<BsonDataRow> { Success = true, Row = row });
        }

        protected void Complete()
        {
            _targetBlock.Complete();
        }

        protected Dictionary<string, object> FillParameters(IDictionary<string, string> defaultParameters)
        {
            var parameters = new Dictionary<string, object>();

            foreach (var key in defaultParameters.Keys)
            {
                var value = Context.GetNamedParameterValue(key);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    parameters[key] = new ValueExpression(value).Value;
                }
                else if (defaultParameters[key] != null)
                {
                    parameters[key] = defaultParameters[key];
                }
                else
                {
                    throw new ArgumentNullException("Can not find the argument " + key);
                }
            }

            return parameters;
        }

        public abstract void Start();
    }
}
