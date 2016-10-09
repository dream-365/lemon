namespace Lemon.Transform
{
    public interface IDataReader<TRecord> : IDataReader
    {
        TRecord Read();
    }


    public interface IDataReader : System.IDisposable
    {
        object ReadObject();

        bool End();
    }
}
