using Lemon.Data.Core.Models;
using System.Collections.Generic;

namespace Lemon.Data.Core
{
    public interface ITransformManyBlock<TSource, TTarget>
    {
        IEnumerable<TTarget> Transform(TSource record);
    }
}
