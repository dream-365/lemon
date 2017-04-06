using Lemon.Core.Models;

namespace Lemon.Core
{
    public interface ITransformBlock<ISource, ITarget>
    {
        ITarget Transform(ISource record);
    }
}
