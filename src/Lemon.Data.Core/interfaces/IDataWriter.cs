namespace Lemon.Transform
{
    public interface IDataWriter<TRecord> : System.IDisposable
    {
        void Write(TRecord record);
    }
}
