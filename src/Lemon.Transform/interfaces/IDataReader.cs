namespace Lemon.Transform
{
    public interface IDataReader<TRecord>
    {
        TRecord Read();

        bool End();
    }
}
