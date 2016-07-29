using Lemon.Transform;

namespace Demo001
{
    public class DoNothingAction : TransformSingleAction
    {
        public override BsonDataRow Transform(BsonDataRow row)
        {
            return row;
        }
    }
}
