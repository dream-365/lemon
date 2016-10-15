namespace Lemon.Data.Core
{
    public interface ICompareObserver<T>
    {
        void OnAdd(T message);

        void OnDelete(T message);

        void OnChange(T previous, T current);
    }
}
