using Lemon.Core.Models;
using System.Collections.Generic;

namespace Lemon.Core
{
    public interface ITransformManyBlock<TSource, TTarget>
    {
        IEnumerable<TTarget> Transform(TSource record);
    }
}
