using Lemon.Transform.Models;

namespace Lemon.Transform
{
    public interface ITransformBlock
    {
        DataRowTransformWrapper<BsonDataRow> Transform(DataRowTransformWrapper<BsonDataRow> data);
    }
}
