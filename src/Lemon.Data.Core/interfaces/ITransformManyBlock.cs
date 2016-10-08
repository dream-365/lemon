using Lemon.Transform.Models;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public interface ITransformManyBlock<TSource, TTarget>
    {
        IEnumerable<TTarget> Transform(TSource record);
    }
}
