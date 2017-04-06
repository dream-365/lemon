namespace Lemon.IO
{
    public interface IRecordAccess<T>
    {
        T Load(object key);
        void Save(T record);
    }
}
