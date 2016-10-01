using Lemon.Transform;

namespace Demo001
{
    public class SlowAction : TransformSingleAction
    {
        public override BsonDataRow Transform(BsonDataRow row)
        {
            System.Threading.Thread.Sleep(1000);

            return row;
        }
    }
}
