using System;

namespace Lemon.Transform
{
    public interface ITransformDataReader2
    {
        void ForEach(Action<BsonDataRow> forEachRow);
    }
}
