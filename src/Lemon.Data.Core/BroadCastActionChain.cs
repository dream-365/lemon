namespace Lemon.Data.Core
{
    public class BroadCastActionChain<T>
    {
        private BroadCastNode<T> _node;

        public BroadCastActionChain(BroadCastNode<T> node)
        {
            _node = node;
        }

        public TransformActionChain<T> Branch()
        {
            return new TransformActionChain<T>(_node);
        }
    }
}
