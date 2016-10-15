using System.Threading.Tasks;

namespace Lemon.Data.Core
{
    public interface IComareExecute<T>
    {
        ICompareObserver<T> Observer { get; set; }

        Task RunAsync();
    }
}
