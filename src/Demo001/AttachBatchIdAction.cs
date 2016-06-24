using Lemon.Transform;

namespace Demo001
{
    public class AttachBatchIdAction : TransformSingleAction
    {
        private string _batchId;

        private string _fieldName;

        public AttachBatchIdAction(string fieldName, string batchId)
        {
            _fieldName = fieldName;

            _batchId = batchId;
        }

        public override BsonDataRow Transform(BsonDataRow row)
        {
            row.SetValue(_fieldName, _batchId);

            return row;
        }
    }
}
