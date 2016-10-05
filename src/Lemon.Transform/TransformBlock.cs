using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public interface ITransformBlock<ISource, ITarget>
    {
        ITarget Transform(ISource record);
    }
}
