namespace Lemon.Transform.Models
{
    public class MessageWrapper<TMessage>
    {
        public MessageWrapper()
        {

        }

        public MessageWrapper(TMessage message)
        {
            Message = message;
        }

        public TMessage Message { get; set; }

        public bool IsBroken { get; set; }
    }
}
