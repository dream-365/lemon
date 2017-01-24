namespace Lemon.Data.Core
{
    public interface IDataReader<TRecord> : IDataReader
    {
        new TRecord Read();
    }


    public interface IDataReader : System.IDisposable
    {
        object Read();

        bool Next();
    }
}
