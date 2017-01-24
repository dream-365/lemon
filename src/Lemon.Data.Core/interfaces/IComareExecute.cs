using System;
using System.Threading.Tasks;

namespace Lemon.Data.Core
{
    [Obsolete]
    public interface IComareExecute<T>
    {
        ICompareObserver<T> Observer { get; set; }

        Task RunAsync();

        void Run();
    }
}
