namespace Lemon.Transform
{
    public interface IDataReader<TRecord> : IDataReader
    {
        TRecord Read();
    }


    public interface IDataReader
    {
        object ReadObject();

        bool End();

        void Close();
    }
}
