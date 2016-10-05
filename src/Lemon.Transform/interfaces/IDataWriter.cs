namespace Lemon.Transform
{
    public interface IDataWriter<TRecord>
    {
        void Write(TRecord record);
    }
}
