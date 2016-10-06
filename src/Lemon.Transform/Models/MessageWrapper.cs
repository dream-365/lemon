namespace Lemon.Transform.Models
{
    public class MessageWrapper<TMessage>
    {
        public TMessage Message { get; set; }

        public bool IsBroken { get; set; }
    }
}
