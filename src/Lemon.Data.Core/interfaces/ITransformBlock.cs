using Lemon.Data.Core.Models;

namespace Lemon.Data.Core
{
    public interface ITransformBlock<ISource, ITarget>
    {
        ITarget Transform(ISource record);
    }
}
