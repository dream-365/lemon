using System;

namespace Lemon.Transform.Models
{
    public class MessageWrapper<TMessage>
    {
        private Guid _pipelineId;

        public MessageWrapper()
        {

        }

        public MessageWrapper(TMessage message, Guid pipelineId)
        {
            Message = message;

            _pipelineId = pipelineId;
        }

        public TMessage Message { get; set; }

        public Guid PipelineId { get { return _pipelineId; } }

        public bool IsBroken { get; set; }
    }
}
