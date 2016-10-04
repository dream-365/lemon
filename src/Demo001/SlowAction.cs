using Lemon.Transform;
using System.Threading.Tasks;

namespace Demo001
{
    public class SlowAction : TransformSingleAction
    {
        public override BsonDataRow Transform(BsonDataRow row)
        {
            Task.Delay(1000).Wait();

            return row;
        }
    }
}
