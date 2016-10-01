using Lemon.Transform.Models;
using System.Collections.Generic;

namespace Lemon.Transform
{
    public interface ITransformManyBlock
    {
        IEnumerable<DataRowTransformWrapper<BsonDataRow>> Transform(DataRowTransformWrapper<BsonDataRow> data);
    }
}
