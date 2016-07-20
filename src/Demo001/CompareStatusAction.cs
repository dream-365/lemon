using Lemon.Transform;

namespace Demo001
{
    public class CompareStatusAction : TransformSingleAction
    {
        private DataRowStatusContext _context;

        public CompareStatusAction(DataRowStatusContext context)
        {
            _context = context;
        }

        public override BsonDataRow Transform(BsonDataRow row)
        {
            var status = _context.Compare(row);

            if(status == DataRowCompareStatus.NoChange)
            {
                return null;
            }

            return row;
        }
    }
}
