namespace Lemon.Data.Core
{
    public interface IDataWriter<TRecord> : System.IDisposable
    {
        void Write(TRecord record);
    }
}
