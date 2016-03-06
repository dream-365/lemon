namespace Lemon.Core
{
    public interface IMessageQueueProvider
    {
        IMessageQueue Get(string name, bool createIfNotExists);
    }
}
